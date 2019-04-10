using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    private GameObject g;
    private GameObject objVideo;
    //Control
    private bool xButton;
    private bool yButton;
    private bool bButton;
    private bool prevXButton = false;
    private bool prevYButton = false;
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
    //X Movement
    private float minArmDistanceX = 3.5f;
    private float maxArmDistanceX = 4.5f;
    private bool moveArmX = false;
    private int dirArmX = 0;
    // Start is called before the first frame update
    void Start()
    {
        g = GameObject.Find("Global");
        objVideo = GameObject.Find("Video");
        double quadHeight = Camera.main.orthographicSize / 1.5;
        double quadWidth = quadHeight * Screen.width / Screen.height;
        objVideo.transform.localScale = new Vector3((float)quadWidth, (float)quadHeight,1);
        AddVideo(ref objVideo, g.GetComponent<GlobalVars>().nameResource);
        objVideo.GetComponent<VideoPlayer>().Pause();

        //Tracker
        bufferTracker = new Queue<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTimeHalf)
        {
            nextActionTimeHalf += periodHalf;
            InputDataTracker("Tracker0@10.3.137.218");
            InputTrackerGloves();
        }
        InputDataControl("Joylin1@10.3.136.131");
        InputControl();
        prevXButton = xButton;
        prevYButton = yButton;
    }

    void AddVideo(ref GameObject obj, string videoName)
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


    void InputDataControl(string address)
    {
        if (Time.time > nextActionTime)
        {
            bButton = VRPN.vrpnButton(address, 1);
            yButton = VRPN.vrpnButton(address, 4);
            xButton = VRPN.vrpnButton(address, 3);
        }
    }

    void InputControl()
    {
        if (bButton)
        {
            SceneManager.LoadScene("SampleScene");
        }
        if (!xButton && prevXButton)
        {
            if (objVideo.GetComponent<VideoPlayer>().isPlaying)
            {
                objVideo.GetComponent<VideoPlayer>().Pause();
            }
            else
            {
                objVideo.GetComponent<VideoPlayer>().Play();
            }
        }
        if (!yButton && prevYButton)
        {
            objVideo.GetComponent<VideoPlayer>().Stop();
            objVideo.GetComponent<VideoPlayer>().Play();
            objVideo.GetComponent<VideoPlayer>().Pause();
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
        ZMovement();
        XMovement();
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
                float dif = Mathf.Abs(bufferTracker.ElementAt(tamBuffer - 1).z - bufferTracker.ElementAt(tamBuffer - i - 1).z);
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
            //Play Video
            if (moveArmZ)
            {
                //Right move
                if (dirArmZ == 1)
                {
                    objVideo.GetComponent<VideoPlayer>().Play();
                }
                //Left move
                else if (dirArmZ == -1)
                {
                    objVideo.GetComponent<VideoPlayer>().Pause();
                }
                bufferTracker.Clear();
            }
            //Debug.Log("¿hay movimiento? "+move);
        }
    }
    void XMovement()
    {
        dirArmX = 0;
        moveArmX = false;
        int i = 1;
        if (bufferTracker.Count >= 20)
        {
            while (!moveArmX && i < tamBuffer - 1)
            {
                float dif = Mathf.Abs(bufferTracker.ElementAt(tamBuffer - 1).x - bufferTracker.ElementAt(tamBuffer - i - 1).x);
                if (dif >= minArmDistanceX && dif <= maxArmDistanceX)
                {
                    moveArmX = true;
                    if (bufferTracker.ElementAt(tamBuffer - 1).x < bufferTracker.ElementAt(tamBuffer - i - 1).x)
                    {
                        dirArmX = -1;
                    }
                    else
                    {
                        dirArmX = 1;
                    }
                }
                i++;
            }
            //Stop Video
            if (moveArmX)
            {
                if (dirArmX == 1)
                {
                    objVideo.GetComponent<VideoPlayer>().Stop();
                    objVideo.GetComponent<VideoPlayer>().Play();
                    objVideo.GetComponent<VideoPlayer>().Pause();
                }
                bufferTracker.Clear();
            }
            //Debug.Log("¿hay movimiento? "+move);
        }
    }
}
