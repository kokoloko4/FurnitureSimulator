using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackRotation : MonoBehaviour
{
    //Tracker
    private Quaternion QuatVRPN;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetTrackerData("Tracker0@10.3.136.131", 1);
        transform.rotation = QuatVRPN;
    }

    private void GetTrackerData(string name, int sensor)
    {
        QuatVRPN = VRPN.vrpnTrackerQuat(name, sensor);
    }
}
