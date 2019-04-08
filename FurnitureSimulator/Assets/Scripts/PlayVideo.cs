using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    private GameObject g;
    private GameObject objVideo;
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
        ReturnMainMenu();
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

    void ReturnMainMenu()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
