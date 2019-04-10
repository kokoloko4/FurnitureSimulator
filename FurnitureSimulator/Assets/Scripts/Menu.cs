using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Linq;

public class Menu : MonoBehaviour
{
    //Global Variables
    private GameObject g;
    //Menu
    public int cantOpt;
    public float r;
    private GameObject menu;
    public float distZ;
    private GameObject[] opts;
    public GameObject activeOpt;
    public float targetDistance;
    public LayerMask layerMask;
    private GameObject centerObject;
    //Joystick variables
    private double flechaV;
    private double flechaH;
    private bool aButton;
    private bool changeState = false;
    private bool prevXButton = false;
    //Time
    private float nextActionTime = 0.0f;
    public float period = 0.1f;
    private float nextActionTimeHalf = 0.0f;
    public float periodHalf = 0.05f;
    //Tracker
    private Vector3 posVRPN;
    private float x;
    private float y;
    private float z;
    private Queue<Vector3> bufferTracker;
    private int tamBuffer = 20;
    //Z Movement
    private float minArmDistanceZ = 1.5f;
    private float maxArmDistanceZ = 1.9f;
    private bool moveArmZ = false;
    private int dirArmZ = 0;
    //Y Movement
    private float minArmDistanceY = 6.0f;
    private float maxArmDistanceY = 7.0f;
    private bool moveArmY = false;
    private int dirArmY = 0;

    // Start is called before the first frame update
    void Start()
    {
        g = GameObject.Find("Global");
        menu = GameObject.FindGameObjectWithTag("menu");
        CreateMenu();
        //Tracker
        bufferTracker = new Queue<Vector3>();
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
        if (Time.time > nextActionTimeHalf)
        {
            nextActionTimeHalf += periodHalf;
            InputDataTracker("Tracker0@10.3.137.218");
            InputTrackerGloves();
        }
    }

    void CreateMenu()
    {
        int anguloSep = 360 / cantOpt;
        float angCubo = 90;
        for (int i = 0; i < cantOpt; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Quad);
            cube.name = i.ToString();
            float x = r * Mathf.Cos(angCubo * Mathf.Deg2Rad);
            float y = r * Mathf.Sin(angCubo * Mathf.Deg2Rad);
            if (i == 0)
            {
                cube.tag = "active";
                cube.transform.position = new Vector3(0,0, distZ - (0.4f));
            }
            else
            {
                cube.tag = "not active";
                cube.transform.position = new Vector3(x, y, distZ);
            }
            if(i%3 == 0)
            {
                Texture2D myTexture = Resources.Load("gatito") as Texture2D;
                cube.GetComponent<Renderer>().material.mainTexture = myTexture;
            }
            else if(i%3 == 1)
            {
                AddVideo(ref cube, "video");
                cube.GetComponent<VideoPlayer>().SetDirectAudioMute(0,true);
            }
            else
            {
                AddModel(ref cube);
            }
            cube.transform.localScale = new Vector3(0.3f, 0.2f, 0.01f);
            cube.transform.parent = menu.transform;
            angCubo += anguloSep;
        }
        opts = new GameObject[menu.transform.childCount];
        for (int i = 0; i < menu.transform.childCount; ++i)
        {
            opts[i] = menu.transform.GetChild(i).gameObject;
        }
    }

    void AddModel(ref GameObject parent)
    {
        parent.GetComponent<Renderer>().material = Resources.Load<Material>("ColorMenu2") as Material;
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        obj.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV();
        obj.transform.parent = parent.transform;
        obj.transform.localScale = new Vector3(parent.transform.localScale.x - 0.5f, parent.transform.localScale.y - 0.5f , parent.transform.localScale.z);
        obj.transform.localEulerAngles = new Vector3(40f, 0f, 0f);
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
        if (aButton)
        {
            EnterOption();
        }
    }

    void InputDataTracker(string address)
    {
        posVRPN = VRPN.vrpnTrackerPos(address, 1);
        
        x = -posVRPN.y * 10 / 0.5f;
        y = posVRPN.z * 10 / 0.5f;
        z = -posVRPN.x * 10;
        Vector3 qVector = new Vector3(x, y, z);
        bufferTracker.Enqueue(qVector);
        if (bufferTracker.Count > tamBuffer)
        {
            bufferTracker.Dequeue();
        }
    }

    void InputTrackerGloves()
    {
        Debug.Log("tbuffer" + bufferTracker.ElementAt(tamBuffer-1));

        ZMovement();
        YMovement();

    }

    void ZMovement()
    {
        dirArmZ = 0;
        moveArmZ = false;
        int i = 1;
        if (bufferTracker.Count >= 20)
        {
            while (!moveArmZ && i < tamBuffer - 1)
            {
                float dif = Math.Abs(bufferTracker.ElementAt(tamBuffer - 1).z - bufferTracker.ElementAt(tamBuffer - i - 1).z);
                if (dif >= minArmDistanceZ && dif <= maxArmDistanceZ)
                {
                    moveArmZ = true;
                    if (bufferTracker.ElementAt(tamBuffer - 1).z > bufferTracker.ElementAt(tamBuffer - i - 1).z)
                    {
                        dirArmZ = -1;
                    }
                    else
                    {
                        dirArmZ = 1;
                    }
                }
                i++;
            }
            //Move menu
            if (moveArmZ)
            {
                //Right move
                if (dirArmZ == 1)
                {
                    RightOption(ref activeOpt);
                }
                //Left move
                else if (dirArmZ == -1)
                {
                    LeftOption(ref activeOpt);
                }
                bufferTracker.Clear();
            }
            //Debug.Log("¿hay movimiento? "+move);
        }
    }

    void YMovement()
    {
        dirArmY = 0;
        moveArmY = false;
        int i = 1;
        if (bufferTracker.Count >= 20)
        {
            while (!moveArmY && i < tamBuffer - 1)
            {
                float dif = Math.Abs(bufferTracker.ElementAt(tamBuffer - 1).y - bufferTracker.ElementAt(tamBuffer - i - 1).y);
                if (dif >= minArmDistanceY && dif <= maxArmDistanceY)
                {
                    moveArmY = true;
                    if (bufferTracker.ElementAt(tamBuffer - 1).y > bufferTracker.ElementAt(tamBuffer - i - 1).y)
                    {
                        dirArmY = 1;
                    }
                    else
                    {
                        dirArmY = -1;
                    }
                }
                i++;
            }
            //Enter option
            if (moveArmY)
            {
                if (dirArmY == 1)
                {
                    EnterOption();
                }
                bufferTracker.Clear();
            }
            //Debug.Log("¿hay movimiento? "+ moveArmY);
        }
    }

    public void EnterOption()
    {
        if (activeOpt.transform.childCount > 0)
        {
            SceneManager.LoadScene("ModelScene");
        }
        else if (activeOpt.GetComponent<VideoPlayer>() != null)
        {
            g.GetComponent<GlobalVars>().nameResource = "video";
            SceneManager.LoadScene("VideoScene");
        }
        else if (activeOpt.GetComponent<VideoPlayer>() == null)
        {
            g.GetComponent<GlobalVars>().nameResource = "ColorMenu1";
            SceneManager.LoadScene("ImageScene");
        }
    }
}
