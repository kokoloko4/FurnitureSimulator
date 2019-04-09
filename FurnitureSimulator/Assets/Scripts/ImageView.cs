using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImageView : MonoBehaviour
{
    private GameObject g;
    private GameObject objImage;
    //Control
    private bool bButton;
    //Time
    private float nextActionTime = 0.0f;
    public float period = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        g = GameObject.Find("Global");
        objImage = GameObject.Find("Image");
        double quadHeight = Camera.main.orthographicSize / 3.9;
        double quadWidth = quadHeight * Screen.width / Screen.height;
        objImage.transform.localScale = new Vector3((float)quadWidth, (float)quadHeight, 1);
        objImage.GetComponent<Renderer>().material = Resources.Load<Material>(g.GetComponent<GlobalVars>().nameResource) as Material;
        Texture2D myTexture = Resources.Load("gatito") as Texture2D;
        objImage.GetComponent<Renderer>().material.mainTexture = myTexture;
    }

    // Update is called once per frame
    void Update()
    {
        InputDataControl("Joylin1@10.3.136.131");
        if (bButton)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    void InputDataControl(string address)
    {
        if (Time.time > nextActionTime)
        {
            bButton = VRPN.vrpnButton(address, 1);
        }
    }

}
