using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsMovement : MonoBehaviour
{
    //Virtual limits
    public Vector3 MinPositionsV;
    public Vector3 MaxPositionsV;
    //Real limits
    public Vector3 MinPositionsR = new Vector3(-0.3622f, 0.0233f, -0.1581f);
    public Vector3 MaxPositionsR = new Vector3(-0.1452f, 0.2159f, 0.2722f);
    //Tracker
    private Vector3 PosVRPN;
    private Vector3 PosTracker = new Vector3();
    private Vector3 PosVirtual = new Vector3();
    //Convertions
    private Vector3 RealDeltas = new Vector3();
    private Vector3 VirtualDeltas = new Vector3();
    private int Steps = 1000;
    //Head
    public GameObject Head;
    private Vector3 MaxDistanceV = new Vector3(2, 2, 2);
    private Vector3 MaxDistanceR = new Vector3(0.2f, 0.2f, 0.2f);

    // Start is called before the first frame update
    void Start()
    {
        /*
        MinPositionsV = MinPositionsV * 0.6f;
        MaxPositionsV = MaxPositionsV * 0.6f;
        InitializeDeltas();
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.tag == "righthand")
        {
            GetTrackerData("Tracker0@10.3.136.131", 2);
        }
        if (transform.tag == "lefthand")
        {
            GetTrackerData("Tracker0@10.3.136.131", 3);
        }
        UpdateMinMaxPositions();
        InitializeDeltas();
        ConvertRealToVirtual();
        MoveObject();
    }

    private void GetTrackerData(string name, int sensor)
    {
        PosVRPN = VRPN.vrpnTrackerPos(name, sensor);
        PosTracker.x = PosVRPN.x;
        PosTracker.y = -PosVRPN.z;
        PosTracker.z = -PosVRPN.y;
        //Debug.Log("X: "+PosTracker.x.ToString("N5")+" Y: " + PosTracker.y.ToString("N5") + " Z: "+ PosTracker.z.ToString("N5"));
    }

    private void UpdateMinMaxPositions()
    {
        MinPositionsR = Head.GetComponent<TrackerMovement>().PosTracker - MaxDistanceR;
        MaxPositionsR = Head.GetComponent<TrackerMovement>().PosTracker + MaxDistanceR;
        MinPositionsV = Head.GetComponent<TrackerMovement>().PosVirtual - MaxDistanceV;
        MaxPositionsV = Head.GetComponent<TrackerMovement>().PosVirtual + MaxDistanceV;
    }

    private void InitializeDeltas()
    {
        RealDeltas.x = (MaxPositionsR.x - MinPositionsR.x) / Steps;
        RealDeltas.y = (MaxPositionsR.y - MinPositionsR.y) / Steps;
        RealDeltas.z = (MaxPositionsR.z - MinPositionsR.z) / Steps;
        VirtualDeltas.x = (MaxPositionsV.x - MinPositionsV.x) / Steps;
        VirtualDeltas.y = (MaxPositionsV.y - MinPositionsV.y) / Steps;
        VirtualDeltas.z = (MaxPositionsV.z - MinPositionsV.z) / Steps;
    }

    private void ConvertRealToVirtual()
    {
        //Convert X
        float pX = (PosTracker.x - MinPositionsR.x) / RealDeltas.x;
        float xT = (pX * VirtualDeltas.x) + MinPositionsV.x;
        //Convert Y
        float pY = (PosTracker.y - MinPositionsR.y) / RealDeltas.y;
        float yT = (pY * VirtualDeltas.y) + MinPositionsV.y;
        //Convert Z
        float pZ = (PosTracker.z - MinPositionsR.z) / RealDeltas.z;
        float zT = (pZ * VirtualDeltas.z) + MinPositionsV.z;

        PosVirtual.x = xT;
        PosVirtual.y = yT;
        PosVirtual.z = zT;
    }

    private void MoveObject()
    {
        if (PosVirtual.x > MaxPositionsV.x)
        {
            PosVirtual.x = MaxPositionsV.x;
        }
        if (PosVirtual.x < MinPositionsV.x)
        {
            PosVirtual.x = MinPositionsV.x;
        }
        if (PosVirtual.y > MaxPositionsV.y)
        {
            PosVirtual.y = MaxPositionsV.y;
        }
        if (PosVirtual.y < MinPositionsV.y)
        {
            PosVirtual.y = MinPositionsV.y;
        }
        if (PosVirtual.z > MaxPositionsV.z)
        {
            PosVirtual.z = MaxPositionsV.z;
        }
        if (PosVirtual.z < MinPositionsV.z)
        {
            PosVirtual.z = MinPositionsV.z;
        }
        transform.position = PosVirtual;
    }

}
