using System.Collections;
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
    public GameObject vista;
    public Vector3 actualScale;
    private string nameFurni;

    // Start is called before the first frame update
    void Start()
    {
        Gloves = new Gloves5DT(Application.dataPath + "/DataGloves/");
        vista = GameObject.FindGameObjectWithTag("vista");
    }

    // Update is called once per frame
    void Update()
    {
        Gloves.GetFingersData("Glove14Left@10.3.136.131", "Glove14Right@10.3.136.131");
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
        if (TouchingFurniture()  && OpenFingersRight <= 2)
        {            
            transform.SetParent(RightHand.transform.parent.transform);
            transform.position = new Vector3(vista.transform.position.x, vista.transform.position.y, vista.transform.position.z);
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
            isGrabbed = true;
            WiimoteManager.FindWiimotes();
            controlWii = WiimoteManager.Wiimotes[0];
            controlWii.SetupIRCamera(IRDataType.BASIC);
            if (!transform.name.Equals(nameFurni))
            {
                nameFurni = transform.name;
                actualScale = transform.localScale;
            }
        }
    }

    public bool TouchingFurniture()
    {
        if(RightHand != null)
        {
            return true;
        }
        return false;
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
