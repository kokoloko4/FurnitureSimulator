using System.Collections;
using System.Collections.Generic;
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
    }

    // Update is called once per frame
    void Update()
    {
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
}
