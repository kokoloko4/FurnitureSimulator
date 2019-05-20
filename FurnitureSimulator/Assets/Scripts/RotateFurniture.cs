using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;


public class RotateFurniture : MonoBehaviour
{

    private GameObject RightHand = null;
    private GameObject LeftHand = null;
    public Wiimote controlWii = null;

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
                    controlWii = GetComponent<CollisionHands>().controlWii;


            if (controlWii != null)
            {
                int ret;
                do
                {
                    ret = controlWii.ReadWiimoteData();

                    if (ret > 0 && controlWii.current_ext == ExtensionController.MOTIONPLUS)
                    {
                        Vector3 offset = new Vector3(-controlWii.MotionPlus.PitchSpeed,
                                                        controlWii.MotionPlus.YawSpeed,
                                                        controlWii.MotionPlus.RollSpeed) / 95f; // Divide by 95Hz (average updates per second from wiimote)
                                                                                                //wmpOffset += offset;

                        //model.rot.Rotate(offset, Space.Self);
                    }
                } while (ret > 0);

                NunchuckData data = controlWii.Nunchuck;

                Debug.Log("Stick: " + data.stick[0] + ", " + data.stick[1]);


                if (data.stick[0] - 140 > 10 && data.c && !data.z)
                {
                    transform.Rotate(0, 10, 0);
                }
                else if (data.stick[0] - 140 < -10 && data.c && !data.z)
                {
                    transform.Rotate(0, -10, 0);

                }
                else if (data.stick[1] - 130 > 10 && data.c && !data.z)
                {
                    transform.Rotate(10, 0, 0);
                }
                else if (data.stick[1] - 130 < -10 && data.c && !data.z)
                {
                    transform.Rotate(-10, 0, 0);
                }
                else if (data.stick[1] - 130 > 10 && data.c && data.z)
                {
                    transform.Rotate(0, 0, 10);
                }
                else if (data.stick[1] - 130 < -10 && data.c && data.z)
                {
                    transform.Rotate(0, 0, -10);
                }

            }


        }
    }
}