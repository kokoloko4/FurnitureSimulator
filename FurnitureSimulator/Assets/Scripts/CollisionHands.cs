﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class CollisionHands : MonoBehaviour
{
    public GameObject RightHand = null;
    public GameObject LeftHand = null;
    public bool isGrabbed = false;
    private Gloves5DT Gloves;
    private int OpenFingersRight;
    private int OpenFingersLeft;
    public Wiimote controlWii = null;

    // Start is called before the first frame update
    void Start()
    {
        Gloves = new Gloves5DT(Application.dataPath + "/DataGloves/");
    }

    // Update is called once per frame
    void Update()
    {
        Gloves.GetFingersData("Glove14Left@10.3.136.131", "Glove14Right@10.3.136.131");
        //Gloves.TestInfo();
        /*Debug.Log("OpenR: "+Gloves.OpenFingers(Gloves.InfoRight));
        Debug.Log("CloseR: " + Gloves.CloseFingers(Gloves.InfoRight));
        Debug.Log("OpenL: "+Gloves.OpenFingers(Gloves.InfoLeft));
        Debug.Log("CloseL: " + Gloves.CloseFingers(Gloves.InfoLeft));*/
        //Debug.Log("entro");
        OpenFingersRight = Gloves.OpenFingers(Gloves.InfoRight);
        OpenFingersLeft = Gloves.OpenFingers(Gloves.InfoLeft);
        ThrowFurniture();
      
    }

    private void OnCollisionStay(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.name.Equals("RightHand"))
        {
            RightHand = collision.gameObject;
        }
        else if (obj.name.Equals("LeftHand"))
        {
            LeftHand = collision.gameObject;
        }
        if ( RightHand != null && LeftHand != null && OpenFingersRight <= 2 && OpenFingersLeft <= 2)
        {
            
            transform.SetParent(RightHand.transform.parent.transform);
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
            isGrabbed = true;
            Debug.Log(isGrabbed);
            WiimoteManager.FindWiimotes();
            controlWii = WiimoteManager.Wiimotes[0];
            //controlWii = transform.parent.GetComponent<MoveCamera>().controlWii;
            controlWii.SetupIRCamera(IRDataType.BASIC);
        }
    }

    private void ThrowFurniture()
    {
        if (OpenFingersRight >= 3 && OpenFingersLeft >= 3)
        {
            transform.SetParent(null);
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            RightHand = null;
            LeftHand = null;
            isGrabbed = false;
            controlWii = null;
        }
    }
}
