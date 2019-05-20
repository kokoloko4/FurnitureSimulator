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
        //RightHand != null && LeftHand != null && GetComponent<CollisionHands>().isGrabbed
        if (true)
        {       
                if (Input.GetKey(KeyCode.O))
                {
                    transform.Rotate(new Vector3(0, 10, 0),Space.Self);
                }
                else if (Input.GetKey(KeyCode.L))
                {
                    transform.Rotate(new Vector3(0, -10, 0), Space.Self);
            }
                else if (Input.GetKey(KeyCode.P))
                {
                transform.Rotate(new Vector3(10, 0, 0), Space.Self);
            }
                else if (Input.GetKey(KeyCode.I))
                {
                transform.Rotate(new Vector3(-10, 0, 0), Space.Self);
            }
                else if (Input.GetKey(KeyCode.K))
                {
                transform.Rotate(new Vector3(0, 0, 10), Space.Self);
            }
                else if (Input.GetKey(KeyCode.M))
                {
                transform.Rotate(new Vector3(0, 0, -10), Space.Self);
            }



        }
    }
}