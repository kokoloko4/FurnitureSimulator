﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Menu : MonoBehaviour
{
    public int cantOpt;
    public float r;
    private GameObject menu;
    public Material m1;
    public float distZ;
    private GameObject[] opts;
    public GameObject activeOpt;
    public float targetDistance;
    public LayerMask layerMask;
    private GameObject centerObject;
    private double flechaV;
    private double flechaH;
    private bool xButton;
    private bool yButton;
    private bool zButton;
    private bool aButton;
    private bool bButton;
    private bool cButton;
    private bool lTrigger;
    private bool rTrigger;
    private float nextActionTime = 0.0f;
    public float period = 0.1f;
    private bool changeState = false;
    private bool prevXButton = false;
    private bool prevYButton = false;
    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.FindGameObjectWithTag("menu");
        CreateMenu();
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        while (i < opts.Length)
        {
            if (opts[i].tag == "active")
            {
                activeOpt = opts[i];
            }
            i++;
        }
        InputDataControl("Joylin1@10.3.136.131");
        InputControl();
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightOption(ref activeOpt);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftOption(ref activeOpt);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && menu.activeSelf == false)
        {
            menu.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && menu.activeSelf == true)
        {
            menu.SetActive(false);
        }
        RaycastHit optHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out optHit, targetDistance, layerMask))
        {
            if (Input.GetMouseButtonDown(0) && optHit.collider.tag.Equals("active") && !centerObject.activeSelf)
            {
                centerObject.SetActive(true);
                if (activeOpt.GetComponent<VideoPlayer>() != null)
                {
                    AddVideo(ref centerObject, "video");
                }
                else if(activeOpt.GetComponent<VideoPlayer>() == null)
                {
                    Destroy(centerObject.GetComponent<VideoPlayer>());
                    centerObject.GetComponent<Renderer>().material = activeOpt.GetComponent<Renderer>().material;
                    /*Scene sceneToLoad = SceneManager.GetSceneAt(1);
                    SceneManager.LoadScene(sceneToLoad.name);
                    SceneManager.MoveGameObjectToScene(centerObject, sceneToLoad);*/
                }
            }
        }
        if (Input.GetMouseButtonDown(1) && centerObject.activeSelf)
        {
            centerObject.SetActive(false);
        }
        prevXButton = xButton;
        prevYButton = yButton;
    }

    void CreateMenu()
    {
        int anguloSep = 360 / cantOpt;
        float angCubo = 90;
        for (int i = 0; i < cantOpt; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = i.ToString();
            float x = r * Mathf.Cos(angCubo * Mathf.Deg2Rad);
            float y = r * Mathf.Sin(angCubo * Mathf.Deg2Rad);
            if (i == 0)
            {
                cube.tag = "active";
                cube.transform.position = new Vector3(x, y, distZ - (0.4f));
            }
            else
            {
                cube.tag = "not active";
                cube.transform.position = new Vector3(x, y, distZ);
            }
            if(i%2 == 0)
            {
                cube.GetComponent<Renderer>().material = m1;
            }
            else
            {
                AddVideo(ref cube, "video");
                cube.GetComponent<VideoPlayer>().SetDirectAudioMute(0,true);
                //AddModel(ref cube);
            }
            cube.transform.localScale = new Vector3(0.2f, 0.1f, 0.01f);
            cube.transform.localEulerAngles = new Vector3(0, 0, 180);
            cube.transform.parent = menu.transform;
            angCubo += anguloSep;
        }
        opts = new GameObject[menu.transform.childCount];
        for (int i = 0; i < menu.transform.childCount; ++i)
        {
            opts[i] = menu.transform.GetChild(i).gameObject;
        }
        centerObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        centerObject.tag = "centerObject";
        centerObject.transform.position = new Vector3(0f, 0f, -9.5f);
        centerObject.transform.parent = menu.transform;
        centerObject.transform.localScale = new Vector3(1.2f, 0.8f, 0.0f);
        centerObject.SetActive(false);
    }

    void AddModel(ref GameObject parent)
    {
        parent.GetComponent<Renderer>().material = Resources.Load<Material>("ColorMenu2") as Material;
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        obj.GetComponent<Renderer>().material.color = Random.ColorHSV();
        obj.transform.parent = parent.transform;
        obj.transform.localScale = new Vector3(parent.transform.localScale.x, parent.transform.localScale.y, parent.transform.localScale.z);
        obj.transform.localEulerAngles = new Vector3(40f,0f,0f);
    }

    void AddVideo (ref GameObject obj, string videoName)
    {
        VideoPlayer video = obj.AddComponent<VideoPlayer>();
        AudioSource audio = obj.AddComponent<AudioSource>();
        video.clip = Resources.Load<VideoClip>(videoName) as VideoClip;
        video.isLooping = true;
        video.renderMode = VideoRenderMode.MaterialOverride;
        video.targetMaterialRenderer = GetComponent<Renderer>();
        video.targetMaterialProperty = "_MainTex";
        video.audioOutputMode = VideoAudioOutputMode.AudioSource;
        video.SetTargetAudioSource(0, audio);
        video.Play();
    }

    public void RightOption(ref GameObject activeOpt)
    {
        float x = opts[cantOpt - 1].transform.position.x;
        float y = opts[cantOpt - 1].transform.position.y;
        float z = opts[cantOpt - 1].transform.position.z;
        for (int i = cantOpt - 1; i > 0; i--)
        {
            opts[i].transform.position = new Vector3(opts[i - 1].transform.position.x, opts[i - 1].transform.position.y, opts[i - 1].transform.position.z);
            if (i == int.Parse(activeOpt.name) + 1)
            {
                opts[i].tag = "active";
            }
        }
        if (int.Parse(activeOpt.name) + 1 == cantOpt)
        {
            opts[0].tag = "active";
        }
        opts[0].transform.position = new Vector3(x, y, z);
        activeOpt.tag = "not active";
    }

    void LeftOption(ref GameObject activeOpt)
    {
        float x = opts[0].transform.position.x;
        float y = opts[0].transform.position.y;
        float z = opts[0].transform.position.z;
        for (int i = 0; i < cantOpt - 1; i++)
        {
            opts[i].transform.position = new Vector3(opts[i + 1].transform.position.x, opts[i + 1].transform.position.y, opts[i + 1].transform.position.z);
            if (i == int.Parse(activeOpt.name) - 1)
            {
                opts[i].tag = "active";
            }
        }
        if (int.Parse(activeOpt.name) - 1 == -1)
        {
            opts[cantOpt - 1].tag = "active";
        }
        opts[cantOpt - 1].transform.position = new Vector3(x, y, z);
        activeOpt.tag = "not active";
    }

    void InputDataControl(string address)
    {
        if (Time.time > nextActionTime)
        {
            flechaV = VRPN.vrpnAnalog(address, 3);
            flechaH = VRPN.vrpnAnalog(address, 2);
            aButton = VRPN.vrpnButton(address, 0);
            bButton = VRPN.vrpnButton(address, 1);
            cButton = VRPN.vrpnButton(address, 2);
            xButton = VRPN.vrpnButton(address, 3);
            yButton = VRPN.vrpnButton(address, 4);
            zButton = VRPN.vrpnButton(address, 5);
            lTrigger = VRPN.vrpnButton(address, 6);
            rTrigger = VRPN.vrpnButton(address, 7);
        }
    }

    void InputControl()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            if (flechaH == 1)
            {
                RightOption(ref activeOpt);
            }
            else if (flechaH == -1)
            {
                LeftOption(ref activeOpt);
            }
        }
        if (aButton && !centerObject.activeSelf)
        {
            centerObject.SetActive(true);
            if (activeOpt.GetComponent<VideoPlayer>() != null)
            {
                AddVideo(ref centerObject, "video");
                centerObject.GetComponent<VideoPlayer>().Pause();                
            }
            else if (activeOpt.GetComponent<VideoPlayer>() == null)
            {
                Destroy(centerObject.GetComponent<VideoPlayer>());
                centerObject.GetComponent<Renderer>().material = activeOpt.GetComponent<Renderer>().material;
                /*Scene sceneToLoad = SceneManager.GetSceneAt(1);
                SceneManager.LoadScene(sceneToLoad.name);
                SceneManager.MoveGameObjectToScene(centerObject, sceneToLoad);*/
            }
        }
        if (bButton && centerObject.activeSelf)
        {
            centerObject.SetActive(false);
        }
        if (centerObject.GetComponent<VideoPlayer>() != null)
        {
            if (!xButton && prevXButton)
            {               
                if (centerObject.GetComponent<VideoPlayer>().isPlaying)
                {
                    centerObject.GetComponent<VideoPlayer>().Pause();
                }
                else
                {
                    centerObject.GetComponent<VideoPlayer>().Play();
                }
            }
            if (!yButton && prevYButton)
            {
                centerObject.GetComponent<VideoPlayer>().Stop();
                centerObject.GetComponent<VideoPlayer>().Play();
                centerObject.GetComponent<VideoPlayer>().Pause();
            }
        }
        /*if (!xButton && prevXButton)
        {
            changeState = !changeState;
        }*/
    }
}
