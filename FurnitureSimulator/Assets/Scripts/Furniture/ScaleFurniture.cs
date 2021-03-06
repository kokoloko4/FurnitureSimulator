﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;


public class ScaleFurniture : MonoBehaviour
{
    private CollisionHands collisionHands;
    private Dictionary<string,Vector3> MinScales = new Dictionary<string, Vector3>();
    private Dictionary<string,Vector3> MaxScales = new Dictionary<string, Vector3>();
    private int steps = 5;
    public Wiimote controlWii = null;
    private NunchuckData data;

    // Start is called before the first frame update
    void Start()
    {
        collisionHands = GetComponent<CollisionHands>();
        MinScales.Add("chair", new Vector3(2,3,2));
        MinScales.Add("bed", new Vector3(2,2.5f,2.5f));
        MinScales.Add("vase", new Vector3(17,17,15));
        MinScales.Add("table", new Vector3(0.8f,0.8f,0.8f));
        MinScales.Add("tv", new Vector3(1.5f,1.5f,0.5f));
        MinScales.Add("bookcase", new Vector3(30,30,10));
        MinScales.Add("sofa", new Vector3(2,2,1.5f));
        MaxScales.Add("chair", new Vector3(9.5f,4,2.8f));
        MaxScales.Add("bed", new Vector3(4,5,5));
        MaxScales.Add("vase", new Vector3(40,40,120));
        MaxScales.Add("table", new Vector3(4,4,3));
        MaxScales.Add("tv", new Vector3(3,3,5));
        MaxScales.Add("bookcase", new Vector3(100,50,50));
        MaxScales.Add("sofa", new Vector3(3.5f,3.5f,4f));
    }

    // Update is called once per frame
    void Update()
    {
        controlWii = GetComponent<CollisionHands>().controlWii;
        data = DataWii();
        if (collisionHands.TouchingFurniture() && controlWii != null && data.z)
        {
            switch (transform.tag)
            {
                case "chair":
                    ScaleX("chair");
                    ScaleY("chair");
                    ScaleZ("chair");
                    break;
                case "sofa":
                    ScaleX("sofa");
                    ScaleY("sofa");
                    ScaleZ("sofa");
                    break;
                case "bookcase":
                    ScaleX("bookcase");
                    ScaleY("bookcase");
                    ScaleZ("bookcase");
                    break;
                case "vase":
                    ScaleX("vase");
                    ScaleY("vase");
                    ScaleZ("vase");
                    break;
                case "tv":
                    ScaleX("tv");
                    ScaleY("tv");
                    ScaleZ("tv");
                    break;
                case "bed":
                    ScaleX("bed");
                    ScaleY("bed");
                    ScaleZ("bed");
                    break;
                case "table":
                    ScaleX("table");
                    ScaleY("table");
                    ScaleZ("table");
                    break;
            }
        }
    }

    void ScaleX(string furniture)
    {
        Vector3 minValues = Vector3.zero;
        Vector3 maxValues = Vector3.zero;
        float xRight = transform.localScale.x;
        float xLeft = transform.localScale.x;
        MinMaxValues(furniture, out minValues, out maxValues);
        if (data.stick[0] - 125 > 10)
        {
            xRight += (maxValues.x - minValues.x)/steps;
        }
        else if (data.stick[0] - 125 < -10)
        {
            xLeft -= (maxValues.x - minValues.x) / steps;
        }
        if (xRight != transform.localScale.x)
        {
            xRight = Mathf.Clamp(xRight, minValues.x, maxValues.x);
            transform.localScale = new Vector3(xRight, transform.localScale.y, transform.localScale.z);
        }
        else if (xLeft != transform.localScale.x)
        {
            xLeft = Mathf.Clamp(xLeft, minValues.x, maxValues.x);
            transform.localScale = new Vector3(xLeft, transform.localScale.y, transform.localScale.z);
        }
    }

    void ScaleY(string furniture)
    {
        Vector3 minValues = Vector3.zero;
        Vector3 maxValues = Vector3.zero;
        float yUp = transform.localScale.y;
        float yDown = transform.localScale.y;
        MinMaxValues(furniture, out minValues, out maxValues);
        NunchuckData data = DataWii();
        Vector3 acc = GetAccelVector();
        if (acc.y >= -0.2f)
        {
            yUp += (maxValues.y - minValues.y) / steps;
        }
        else if (acc.y <= -0.7f)
        {
            yDown -= (maxValues.y - minValues.y) / steps;
        }
        if (yUp != transform.localScale.y)
        {
            yUp = Mathf.Clamp(yUp, minValues.y, maxValues.y);
            transform.localScale = new Vector3(transform.localScale.x, yUp, transform.localScale.z);
        }
        else if (yDown != transform.localScale.y)
        {
            yDown = Mathf.Clamp(yDown, minValues.y, maxValues.y);
            transform.localScale = new Vector3(transform.localScale.x, yDown, transform.localScale.z);
        }
    }

    void ScaleZ(string furniture)
    {
        Vector3 minValues = Vector3.zero;
        Vector3 maxValues = Vector3.zero;
        float zBack = transform.localScale.z;
        float zFront = transform.localScale.z;
        MinMaxValues(furniture, out minValues, out maxValues);
        NunchuckData data = DataWii();
        if (data.stick[1] - 130 > 10)
        {
            zBack += (maxValues.z - minValues.z) / steps;
        }
        else if (data.stick[1] - 130 < -10)
        {
            zFront -= (maxValues.z - minValues.z) / steps;
        }
        if (zBack != transform.localScale.z)
        {
            zBack = Mathf.Clamp(zBack, minValues.z, maxValues.z);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, zBack);
        }
        else if (zFront != transform.localScale.z)
        {
            zFront = Mathf.Clamp(zFront, minValues.z, maxValues.z);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, zFront);
        }
    }

    private NunchuckData DataWii()
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
        return data;
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

    private void MinMaxValues(string furniture, out Vector3 minValues, out Vector3 maxValues)
    {
        minValues = maxValues = Vector3.zero;
        switch (furniture)
        {
            case "chair":
                MinScales.TryGetValue("chair", out minValues);
                MaxScales.TryGetValue("chair", out maxValues);
                break;
            case "sofa":
                MinScales.TryGetValue("sofa", out minValues);
                MaxScales.TryGetValue("sofa", out maxValues);
                break;
            case "bookcase":
                MinScales.TryGetValue("bookcase", out minValues);
                MaxScales.TryGetValue("bookcase", out maxValues);
                break;
            case "vase":
                MinScales.TryGetValue("vase", out minValues);
                MaxScales.TryGetValue("vase", out maxValues);
                break;
            case "tv":
                MinScales.TryGetValue("tv", out minValues);
                MaxScales.TryGetValue("tv", out maxValues);
                break;
            case "bed":
                MinScales.TryGetValue("bed", out minValues);
                MaxScales.TryGetValue("bed", out maxValues);
                break;
            case "table":
                MinScales.TryGetValue("table", out minValues);
                MaxScales.TryGetValue("table", out maxValues);
                break;
        }
    }
}
