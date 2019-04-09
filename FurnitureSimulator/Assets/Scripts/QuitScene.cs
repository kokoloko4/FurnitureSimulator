using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitScene : MonoBehaviour
{
    //Tracker
    private Vector3 posVRPN;
    private float x;
    private float y;
    private float z;
    private Queue<Vector3> bufferTracker;
    private int tamBuffer = 20;
    //Z Movement
    private float minArmDistanceZ = 1.8f;
    private float maxArmDistanceZ = 2.2f;
    private bool moveArmZ = false;
    private int dirArmZ = 0;
    //Y Movement
    private float minArmDistanceY = 6.0f;
    private float maxArmDistanceY = 7.0f;
    private bool moveArmY = false;
    private int dirArmY = 0;
    //Time
    private float nextActionTime = 0.0f;
    public float period = 0.1f;
    private float nextActionTimeHalf = 0.0f;
    public float periodHalf = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        //Tracker
        bufferTracker = new Queue<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTimeHalf)
        {
            nextActionTimeHalf += periodHalf;
            InputDataTracker("Tracker0@10.3.137.218");
            InputTrackerGloves();
        }
    }

    void InputDataTracker(string address)
    {
        posVRPN = VRPN.vrpnTrackerPos(address, 1);

        x = -posVRPN.y * 10 / 0.5f;
        y = posVRPN.z * 10 / 0.5f;
        z = -posVRPN.x * 10;
        Vector3 qVector = new Vector3(x, y, z);
        bufferTracker.Enqueue(qVector);
        if (bufferTracker.Count > tamBuffer)
        {
            bufferTracker.Dequeue();
        }
    }

    void InputTrackerGloves()
    {
        Debug.Log("tbuffer" + bufferTracker.ElementAt(tamBuffer - 1));
        YMovement();

    }

    void YMovement()
    {
        dirArmY = 0;
        moveArmY = false;
        int i = 1;
        if (bufferTracker.Count >= 20)
        {
            while (!moveArmY && i < tamBuffer - 1)
            {
                float dif = Math.Abs(bufferTracker.ElementAt(tamBuffer - 1).y - bufferTracker.ElementAt(tamBuffer - i - 1).y);
                if (dif >= minArmDistanceY && dif <= maxArmDistanceY)
                {
                    moveArmY = true;
                    if (bufferTracker.ElementAt(tamBuffer - 1).y > bufferTracker.ElementAt(tamBuffer - i - 1).y)
                    {
                        dirArmY = 1;
                    }
                    else
                    {
                        dirArmY = -1;
                    }
                }
                i++;
            }
            //Enter option
            if (moveArmY)
            {
                if (dirArmY == 1)
                {
                    SceneManager.LoadScene("SampleScene");
                }
                bufferTracker.Clear();
            }
            //Debug.Log("¿hay movimiento? "+ moveArmY);
        }
    }
}
