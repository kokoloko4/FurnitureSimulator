using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTracker : MonoBehaviour
{
    private float nextActionTime = 0.0f;
    public float period = 0.1f;
    private Vector3 posVRPN;
    private float x;
    private float y;
    private float z;
    private Vector3 vPrevious;
    private Vector3 vActual;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        posVRPN = VRPN.vrpnTrackerPos("Tracker0@10.3.136.131", 2);
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            Debug.Log(posVRPN);
        }
        x = - posVRPN.y * 10 / 0.5f;
        y = - posVRPN.z * 10 / 0.5f;
        z = - posVRPN.x * 10;
        vActual = new Vector3(x, y, z);
        /*if (vPrevious != null)
        {
            if (Mathf.Abs(vActual.x - vPrevious.x) <= 0.5)
            {
                Debug.Log(Mathf.Abs(vActual.x - vPrevious.x));
                transform.position = new Vector3(vActual.x, vPrevious.y, vPrevious.z);
            }
            if (Mathf.Abs(vActual.y - vPrevious.y) <= 0.5)
            {
                transform.position = new Vector3(vPrevious.x, vActual.y, vPrevious.z);
            }
            if (Mathf.Abs(vActual.z - vPrevious.z) <= 0.5)
            {
                transform.position = new Vector3(vPrevious.x, vPrevious.y, vActual.z);
            }
        }  
        vPrevious = new Vector3(x, y, z);*/
        if (vPrevious != null)
        {
            if (Mathf.Abs(vActual.x - vPrevious.x) > 0.5)
            {
                vActual.x *= -1;
            }
            if (Mathf.Abs(vActual.y - vPrevious.y) > 0.5)
            {
                vActual.y *= -1;
            }
            if (Mathf.Abs(vActual.z - vPrevious.z) > 0.5)
            {
                vActual.z *= -1;
            }
        }
        transform.position = vActual;
        vPrevious = new Vector3(x, y, z);
    }
}
