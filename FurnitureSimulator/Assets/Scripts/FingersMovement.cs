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
        Gloves = new Gloves5DT("/Users/takina/Documents/Unity/FurnitureSimulator/FurnitureSimulator/Assets/DataGloves/");
        InitializeFingersRight();
        /*double[] tuple = { 0.36, 0.34};
        double[] mean = { 0.41, 0.79};
        Debug.Log(Gloves.Distance(tuple, mean));*/
    }

    // Update is called once per frame
    void Update()
    {
        Gloves.GetFingersData("Glove14Left@10.3.136.131", "Glove14Right@10.3.136.131");
        RotateRightFingers();
    }

    private void InitializeFingersRight()
    {
        //Thumb
        ThumbOpen = new double[2];
        ThumbOpen[0] = 0.36;
        ThumbOpen[1] = 0.34;
        //Index
        IndexOpen = new double[2];
        IndexOpen[0] = 0.36;
        IndexOpen[1] = 0.34;
        //Middle
        MiddleOpen = new double[2];
        MiddleOpen[0] = 0.36;
        MiddleOpen[1] = 0.34;
        //Ring
        RingOpen = new double[2];
        RingOpen[0] = 0.36;
        RingOpen[1] = 0.34;
        //Pinky
        PinkyOpen = new double[2];
        PinkyOpen[0] = 0.36;
        PinkyOpen[1] = 0.34;
    }

    private void RotateRightFingers()
    {
        //Thumb
        ThumbRaw = Gloves.GetFingersTuple(Gloves.TupleRight, 0);
        RotateFinger(Thumb, ThumbRaw, ThumbOpen, 0, -40, 0.0673f, 0.50f);
        //Index
        IndexRaw = Gloves.GetFingersTuple(Gloves.TupleRight, 1);
        RotateFinger(Index, IndexRaw, IndexOpen, 0, -40, 0.0673f, 0.50f);
        //Middle
        MiddleRaw = Gloves.GetFingersTuple(Gloves.TupleRight, 2);
        RotateFinger(Middle, MiddleRaw, MiddleOpen, 0, -40, 0.0673f, 0.50f);
        //Ring
        RingRaw = Gloves.GetFingersTuple(Gloves.TupleRight, 3);
        RotateFinger(Ring, RingRaw, RingOpen, 0, -40, 0.0673f, 0.50f);
        //Pinky
        PinkyRaw = Gloves.GetFingersTuple(Gloves.TupleRight, 4);
        RotateFinger(Pinky, PinkyRaw, PinkyOpen, 0, -40, 0.0673f, 0.50f);
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
        Debug.Log("Data " + Distance + " - " + Degrees);
        finger.GetComponent<FingerRotation>().RotateFinger(new Vector3(Degrees, Degrees, 0));
    }
}
