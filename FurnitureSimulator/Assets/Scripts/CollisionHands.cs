using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHands : MonoBehaviour
{
    public GameObject RightHand = null;
    public GameObject LeftHand = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        if ( RightHand != null && LeftHand != null && Input.GetKey(KeyCode.F))
        {
            transform.SetParent(RightHand.transform.parent.transform);
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void ThrowFurniture()
    {
        if (Input.GetKey(KeyCode.X))
        {
            transform.SetParent(null);
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            RightHand = null;
            LeftHand = null;
        }
    }
}
