using System;
using System.Collections.Generic;
using UnityEngine;

public class FingersMovement : MonoBehaviour
{
    //Gloves
    private Gloves5DT Gloves;
    //Fingers
    //Pinky
    public GameObject Pinky;
    private double[] PinkyRaw;
    private double[] PinkyOpen;
    //Ring
    public GameObject Ring;
    private double[] RingRaw;
    private double[] RingOpen;
    //Middle
    public GameObject Middle;
    private double[] MiddleRaw;
    private double[] MiddleOpen;
    //Index
    public GameObject Index;
    private double[] IndexRaw;
    private double[] IndexOpen;
    //Thumb
    public GameObject Thumb;
    private double[] ThumbRaw;
    private double[] ThumbOpen;

    // Start is called before the first frame update
    void Start()
    {
        Gloves = new Gloves5DT(Application.dataPath + "/DataGloves/");
        if (transform.tag == "righthand")
            InitializeFingersRight();
        else
            InitializeFingersLeft();
    }

    // Update is called once per frame
    void Update()
    {
        Gloves.GetFingersData("Glove14Left@10.3.136.131", "Glove14Right@10.3.136.131");
        //Gloves.TestInfo();
        if (transform.tag == "righthand")
        {
            RotateRightFingers();
        }
        else if(transform.tag == "lefthand")
        {
            RotateLeftFingers();
        }
    }

    private void InitializeFingersRight()
    {
        //Thumb
        ThumbOpen = new double[2];
        ThumbOpen[0] = 0.36;
        ThumbOpen[1] = 0.91;
        //Index
        IndexOpen = new double[2];
        IndexOpen[0] = 0.23;
        IndexOpen[1] = 0.99;
        //Middle
        MiddleOpen = new double[2];
        MiddleOpen[0] = 0.18;
        MiddleOpen[1] = 0.84;
        //Ring
        RingOpen = new double[2];
        RingOpen[0] = 0.35;
        RingOpen[1] = 0.21;
        //Pinky
        PinkyOpen = new double[2];
        PinkyOpen[0] = 0.24;
        PinkyOpen[1] = 0.43;
    }

    private void InitializeFingersLeft()
    {
        //Thumb
        ThumbOpen = new double[2];
        ThumbOpen[0] = 0.32;
        ThumbOpen[1] = 0.45;
        //Index
        IndexOpen = new double[2];
        IndexOpen[0] = 0.15;
        IndexOpen[1] = 0.88;
        //Middle
        MiddleOpen = new double[2];
        MiddleOpen[0] = 0.25;
        MiddleOpen[1] = 0.24;
        //Ring
        RingOpen = new double[2];
        RingOpen[0] = 0.09;
        RingOpen[1] = 1.00;
        //Pinky
        PinkyOpen = new double[2];
        PinkyOpen[0] = 0.09;
        PinkyOpen[1] = 0.09;
    }

    private void RotateRightFingers()
    {
        //Thumb
        ThumbRaw = Gloves.GetFingersTuple(Gloves.TupleRight, 0);
        RotateFinger(Thumb, ThumbRaw, ThumbOpen, 0, -80, 0.0821f, 0.3341f);
        //Index
        IndexRaw = Gloves.GetFingersTuple(Gloves.TupleRight, 1);
        RotateFinger(Index, IndexRaw, IndexOpen, 0, -40, 0.001078f, 0.006467f);
        //Middle
        MiddleRaw = Gloves.GetFingersTuple(Gloves.TupleRight, 2);
        RotateFinger(Middle, MiddleRaw, MiddleOpen, 0, -70, 0.03014f, 0.1356f);
        //Ring
        RingRaw = Gloves.GetFingersTuple(Gloves.TupleRight, 3);
        RotateFinger(Ring, RingRaw, RingOpen, 0, -90, 0.02542f, 0.355f);
        //Pinky
        PinkyRaw = Gloves.GetFingersTuple(Gloves.TupleRight, 4);
        RotateFinger(Pinky, PinkyRaw, PinkyOpen, 0, -50, 0.05582f, 0.1838f);
    }

    private void RotateLeftFingers()
    {
        //Thumb
        ThumbRaw = Gloves.GetFingersTuple(Gloves.TupleLeft, 0);
        RotateFinger(Thumb, ThumbRaw, ThumbOpen, 0, -40, 0.01309f, 0.1428f);
        //Index
        IndexRaw = Gloves.GetFingersTuple(Gloves.TupleLeft, 1);
        RotateFinger(Index, IndexRaw, IndexOpen, 0, -80, 0.01783f, 0.2347f);
        //Middle
        MiddleRaw = Gloves.GetFingersTuple(Gloves.TupleLeft, 2);
        RotateFinger(Middle, MiddleRaw, MiddleOpen, 0, -30, 0.01462f, 0.2729f);
        //Ring
        RingRaw = Gloves.GetFingersTuple(Gloves.TupleLeft, 3);
        RotateFinger(Ring, RingRaw, RingOpen, 0, -90, 0.00936f, 0.0865f);
        //Pinky
        PinkyRaw = Gloves.GetFingersTuple(Gloves.TupleLeft, 4);
        RotateFinger(Pinky, PinkyRaw, PinkyOpen, 0, -20, 0.01f, 0.2923f);
    }

    private float CalculateDegrees(float minAngle, float maxAngle, float minDistance, float maxDistance, float varDistance)
    {
        float m = (maxAngle - minAngle) / (maxDistance - minDistance);
        return m * varDistance + ((minAngle * minDistance) / m);
    }

    private void RotateFinger(GameObject finger, double[] rawFinger, double[] rawOpenFinger, float minAngle, float maxAngle, float minDistance, float maxDistance)
    {
        float Distance = (float)Gloves.Distance(rawFinger, rawOpenFinger);
        float Degrees = CalculateDegrees(minAngle, maxAngle, minDistance, maxDistance, Distance);
        //Debug.Log("Data " + Distance + " - " + Degrees);
        finger.transform.localEulerAngles = new Vector3(0, 0, Degrees);
        finger.transform.GetChild(0).localEulerAngles = new Vector3(0, 0, Degrees);

    }
}
