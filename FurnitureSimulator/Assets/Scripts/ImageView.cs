using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImageView : MonoBehaviour
{
    private GameObject g;
    private GameObject objImage;
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
        ReturnMainMenu();   
    }

    void ReturnMainMenu()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
