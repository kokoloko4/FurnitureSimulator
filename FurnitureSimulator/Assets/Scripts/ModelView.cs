using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModelView : MonoBehaviour
{
    //Control
    private bool lTrigger;
    private bool rTrigger;
    private double flechaV;
    private double flechaH;
    private bool aButton;
    private bool bButton;
    private bool xButton;
    private bool yButton;
    //Model
    private GameObject objModel; 
    //Time
    private float nextActionTime = 0.0f;
    public float period = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        objModel = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            InputDataControl("Joylin1@10.3.136.131");
            InputControl();
        }

    }

    void InputDataControl(string address)
    {
        flechaV = VRPN.vrpnAnalog(address, 3);
        flechaH = VRPN.vrpnAnalog(address, 2);
        aButton = VRPN.vrpnButton(address, 0);
        bButton = VRPN.vrpnButton(address, 1);
        lTrigger = VRPN.vrpnButton(address, 6);
        rTrigger = VRPN.vrpnButton(address, 7);
        xButton = VRPN.vrpnButton(address, 3);
        yButton = VRPN.vrpnButton(address, 4);
    }


    void InputControl()
    {
        //Back
        if (bButton)
        {
            SceneManager.LoadScene("SampleScene");
        }
        //Zoom
        if (rTrigger)
        {
            objModel.transform.localScale *= 1.5f;
        }
        if (lTrigger)
        {
            objModel.transform.localScale /= 1.5f;
        }
        //Mov camera y
        if (flechaV == 1 && yButton)
        {
            transform.position += new Vector3(0, -1, 0);
        }
        else if (flechaV == -1 && yButton)
        {
            transform.position += new Vector3(0, 1, 0);
        }
        else
        {
            //Rotation x
            if (flechaV == 1 && xButton)
            {
                objModel.transform.Rotate(-10, 0, 0);
            }
            else if (flechaV == -1 && xButton)
            {
                objModel.transform.Rotate(10, 0, 0);
            }
            //Rotation y
            else
            {
                if (flechaV == 1)
                {
                    objModel.transform.Rotate(0, -10, 0);
                }
                else if (flechaV == -1)
                {
                    objModel.transform.Rotate(0, 10, 0);
                }
            }
        }
        //Mov camera x
        if (flechaH == 1 && yButton)
        {
            transform.position += new Vector3(1, 0, 0);
        }
        else if (flechaH == -1 && yButton)
        {
            transform.position += new Vector3(-1, 0, 0);
        }
        else
        {
            //Rotation z
            if (flechaH == 1)
            {
                objModel.transform.Rotate(0, 0, -10);
            }
            else if (flechaH == -1)
            {
                objModel.transform.Rotate(0, 0, 10);
            }
        } 
    }
}
