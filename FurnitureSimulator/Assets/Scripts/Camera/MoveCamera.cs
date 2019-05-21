using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

public class MoveCamera : MonoBehaviour
{
    /*
		Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
		Converted to C# 27-02-13 - no credit wanted.
		Simple flycam I made, since I couldn't find any others made public.  
		Made simple to use (drag and drop, done) for regular keyboard layout  
		wasd : basic movement
		shift : Makes camera accelerate
		space : Moves camera on X and Z axis only.  So camera doesn't gain any height
	*/
    float mainSpeed = 10.0f; //regular speed
    float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 1000.0f; //Maximum speed when holdin gshift
    float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;
    public Wiimote controlWii = null;
    private bool activated = false;
    Vector3 wmpOffset;


    void Start()
    {
        WiimoteManager.FindWiimotes();
        controlWii = WiimoteManager.Wiimotes[0];
    }

    void Update()
    {


        if (controlWii == null)
        {
            /* WiimoteManager.FindWiimotes();

             controlWii = WiimoteManager.Wiimotes[0];
             */
        }


        int ret;
        do
        {
            ret = controlWii.ReadWiimoteData();

            if (ret > 0 && controlWii.current_ext == ExtensionController.MOTIONPLUS)
            {

                MotionPlusData datamp = controlWii.MotionPlus;

                float vPitch = datamp.PitchSpeed;
                float vYaw = datamp.YawSpeed;

                //if(vPitch<-10 || vPitch >10)


                Vector3 offset = new Vector3(vPitch,
                                               -vYaw,
                                                0f) / 95f; // Divide by 95Hz (average updates per second from wiimote)

                if (offset.y > -0.2f && offset.y < 0f)
                {

                    offset.y = 0f;
                }


                //datamp.SetZeroValues();
                //float dPitch = datamp.PitchSpeed;
                //float pitch = dPitch;
                // wmpOffset += offset;
                // Quaternion tRotation = Quaternion.Euler(wmpOffset);
                float internaltr = 80f;
                //transform.rotation = Quaternion.FromToRotation(transform.rotation * GetAccelVector(), Vector3.up)* transform.rotation;
                //transform.rotation = Quaternion.FromToRotation(transform.forward, Vector3.forward) * transform.rotation;
                //  Quaternion.FromToRotation(model.rot.forward, Vector3.forward) * model.rot.rotation;
                // Quaternion.FromToRotation(model.rot.rotation * GetAccelVector(), Vector3.up) * model.rot.rotation;

                //  Debug.Log(offset);


                //transform.Rotate(offset, Space.Self);
                transform.Rotate(offset);
                //transform.Rotate(0,0,0)
            }
        } while (ret > 0);
        //Debug.Log(controlWii.current_ext);

        controlWii.SetupIRCamera(IRDataType.BASIC);

        if (controlWii.Button.a)
        {


            if (controlWii.current_ext == ExtensionController.MOTIONPLUS)
            {
                controlWii.DeactivateWiiMotionPlus();
                //WiimoteManager.Cleanup(controlWii);
                controlWii = null;
                WiimoteManager.FindWiimotes();
                controlWii = WiimoteManager.Wiimotes[0];
                Debug.Log("MP Fuera");
            }
            else
            {
                controlWii.ActivateWiiMotionPlus();
                Debug.Log("MP On");
            }



        }

        //if (controlWii.Button.b)
        //{

        //transform.rotation = Quaternion.FromToRotation(transform.rotation * GetAccelVector(), Vector3.down)* transform.rotation;
        //transform.rotation = Quaternion.FromToRotation(transform.forward, Vector3.forward) * transform.rotation;
        // Debug.Log(Vector3.down);

        //  acc.z = 0;
        //  acc.y = 0;
        //transform.Rotate(new Vector3(-acc.x,0,0)*5);

        if (controlWii.Button.b)
        {

            Vector3 acc = GetAccelVector();
            if (acc.y <= -0.8f)
            {
                transform.Rotate(new Vector3(0, 2, 0), Space.World);

            }
            else if (acc.y >= 0.3f)
            {
                transform.Rotate(new Vector3(0, -2, 0), Space.World);

            }

            int my = 2;
            if (transform.eulerAngles.y >90 && transform.eulerAngles.y <270)
            {
                if (acc.z >= -0.2f)
                {
                    transform.Rotate(new Vector3(my, 0, 0), Space.World);
                    //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

                }
                else if (acc.z <= -0.8f)
                {
                    transform.Rotate(new Vector3(-my, 0, 0), Space.World);
                    //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

                }
            }
            else
            {
                if (acc.z >= -0.2f)
                {
                    transform.Rotate(new Vector3(-my, 0, 0), Space.World);
                    //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

                }
                else if (acc.z <= -0.8f)
                {
                    transform.Rotate(new Vector3(my, 0, 0), Space.World);
                    //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

                }
            }
 
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }
        Debug.Log(transform.eulerAngles.y);




        //  }






        //Keyboard commands
        Vector3 p = GetBaseInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += Time.deltaTime;
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        }

        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.Space))
        { //If player wants to move on X and Z axis only
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else
        {
            transform.Translate(p);
        }

    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();

        if (controlWii.current_ext == ExtensionController.NUNCHUCK)
        {

            NunchuckData data = controlWii.Nunchuck;



            //Debug.Log("Stick: " + data.stick[0] + ", " + data.stick[1]);
            if (data.stick[1] - 130 > 10 && !data.c && !data.z)
            {
                p_Velocity += new Vector3(0, 0, 1);
            }
            if (data.stick[1] - 130 < -10 && !data.c && !data.z)
            {
                p_Velocity += new Vector3(0, 0, -1);
            }
            if (data.stick[0] - 125 < -10 && !data.c && !data.z)
            {
                p_Velocity += new Vector3(-1, 0, 0);
            }
            if (data.stick[0] - 125 > 10 && !data.c && !data.z)
            {
                p_Velocity += new Vector3(1, 0, 0);
            }
        }
        return p_Velocity;
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