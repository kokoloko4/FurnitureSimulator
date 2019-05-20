using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHands : MonoBehaviour
{
    public GameObject RightHand = null;
    public GameObject LeftHand = null;
    public bool isGrabbed = false;
    private Gloves5DT Gloves;
    private int OpenFingersRight;
    private int OpenFingersLeft;
    // Start is called before the first frame update
    void Start()
    {
        Gloves = new Gloves5DT(Application.dataPath + "/DataGloves/");
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
        else if (obj.name.Equals("LeftHand"))
        {
            LeftHand = collision.gameObject;
        }
        if (TouchingFurniture()  && OpenFingersRight <= 2 && OpenFingersLeft <= 2)
        {
            transform.SetParent(RightHand.transform.parent.transform);
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
            isGrabbed = true;
        }
    }

    public bool TouchingFurniture()
    {
        if(RightHand != null && LeftHand != null)
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
        }
    }
}
