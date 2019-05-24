using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;


public class RotateFurniture : MonoBehaviour
{

    private CollisionHands collision;
    private CollisionHands scale;
    public Wiimote controlWii = null;
    public bool rotarZ = false;
    public GameObject camera;
    // Start is called before the first frame update
    void Start()
    {
        collision = GetComponent<CollisionHands>();
        scale = GetComponent<CollisionHands>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        controlWii = GetComponent<CollisionHands>().controlWii;
        if (collision.TouchingFurniture() && collision.isGrabbed && controlWii != null)
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
            if(data.c && !data.z)
            {
                transform.parent = GameObject.FindGameObjectWithTag("vista").transform;
                Vector3 acc = GetAccelVector();
                transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
                transform.localScale = scale.actualScale / 3;
                if (acc.y >= -0.2f)
                {
                    transform.Rotate(new Vector3(0, 0, 10));
                }
                else if (acc.y <= -0.7f)
                {
                    transform.Rotate(new Vector3(0, 0, -10));
                }
                else if (data.stick[0] - 125 > 10)
                {
                    transform.Rotate(0, 10, 0);
                }
                else if (data.stick[0] - 125 < -10)
                {
                    transform.Rotate(0, -10, 0);

                }
                else if (data.stick[1] - 130 > 10)
                {
                    transform.Rotate(10, 0, 0);
                }
                else if (data.stick[1] - 130 < -10)
                {
                    transform.Rotate(-10, 0, 0);
                }
                else
                {
                    transform.localScale = scale.actualScale;
                }
            }            
        }
    }


    private Vector3 GetAccelVector()
    {
        float accel_x;
        float accel_y;
        float accel_z;

        float[] accel = controlWii.Accel.GetCalibratedAccelData();
        accel_x = accel[0];
        accel_y = -accel[2];
        accel_z = -accel[1];


        return new Vector3(accel_x, accel_y, accel_z).normalized;
    }
}