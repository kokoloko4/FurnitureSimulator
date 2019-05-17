using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFurniture : MonoBehaviour
{

    private GameObject RightHand = null;
    private GameObject LeftHand = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        RightHand = GetComponent<CollisionHands>().RightHand;
        LeftHand = GetComponent<CollisionHands>().LeftHand;
        if (RightHand != null && LeftHand != null && GetComponent<CollisionHands>().isGrabbed)
        {

       
                if (Input.GetKey(KeyCode.O))
                {
                    transform.Rotate(0, 10, 0);
                }
                else if (Input.GetKey(KeyCode.L))
                {
                    transform.Rotate(0, -10, 0);
                }
                else if (Input.GetKey(KeyCode.P))
                {
                transform.Rotate(10, 0, 0);
                }
                else if (Input.GetKey(KeyCode.I))
                {
                transform.Rotate(-10, 0, 0);
                }
                else if (Input.GetKey(KeyCode.K))
                {
                transform.Rotate(0, 0, 10);
                }
                else if (Input.GetKey(KeyCode.M))
                {
                transform.Rotate(0, 0, -10);
                }



        }
    }
}