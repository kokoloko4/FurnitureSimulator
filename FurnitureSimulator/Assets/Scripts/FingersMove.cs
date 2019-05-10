using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class FingersMove : MonoBehaviour
{
    //Time
    private float nextActionTime = 0.0f;
    public float period = 0.1f;
    //Fingers
    private string path = "C:\\Users\\Takina\\Documents\\GitHub\\FurnitureSimulator\\FurnitureSimulator\\Assets\\DataGloves\\";
    //Right hand
    private string[] filenames;
    private double[][][] means;
    private int[] info;
    private double[] tuple;
    //Left hand
    private string[] filenamesl;
    private double[][][] meansl;
    private int[] infol;
    private double[] tuplel;
    //States
    private int[] infolAnt = new int[5];
    private int[] infoAnt = new int[5];
    //Animations
    private Animation animationOpenMiddle;
    public AnimationClip clipOpenMiddle;

    // Start is called before the first frame update
    void Start()
    {
        filenames = new string[5];
        //TODO
        filenames[0] = path + "finger13.txt";
        filenames[1] = path + "finger23.txt";
        filenames[2] = path + "finger33.txt";
        filenames[3] = path + "finger43.txt";
        filenames[4] = path + "finger53.txt";

        filenamesl = new string[5];
        filenamesl[0] = path + "finger13l.txt";
        filenamesl[1] = path + "finger23l.txt";
        filenamesl[2] = path + "finger33l.txt";
        filenamesl[3] = path + "finger43l.txt";
        filenamesl[4] = path + "finger53l.txt";

        tuple = new double[14];
        tuplel = new double[14];
        animationOpenMiddle = GetComponent<Animation>();
        animationOpenMiddle.AddClip(clipOpenMiddle, "openMid");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            GetFingersData("Glove14Left@10.3.136.131", "Glove14Right@10.3.136.131");
        }
    }

    private void GetFingersData(string nameLeft, string nameRight)
    {
        //Left Glove
        tuplel[0] = VRPN.vrpnAnalog(nameLeft, 0);
        tuplel[1] = VRPN.vrpnAnalog(nameLeft, 1);
        tuplel[2] = VRPN.vrpnAnalog(nameLeft, 2);
        tuplel[3] = VRPN.vrpnAnalog(nameLeft, 3);
        tuplel[4] = VRPN.vrpnAnalog(nameLeft, 4);
        tuplel[5] = VRPN.vrpnAnalog(nameLeft, 5);
        tuplel[6] = VRPN.vrpnAnalog(nameLeft, 6);
        tuplel[7] = VRPN.vrpnAnalog(nameLeft, 7);
        tuplel[8] = VRPN.vrpnAnalog(nameLeft, 8);
        tuplel[9] = VRPN.vrpnAnalog(nameLeft, 9);
        tuplel[10] = VRPN.vrpnAnalog(nameLeft, 10);
        tuplel[11] = VRPN.vrpnAnalog(nameLeft, 11);
        tuplel[12] = VRPN.vrpnAnalog(nameLeft, 12);
        tuplel[13] = VRPN.vrpnAnalog(nameLeft, 13);
        meansl = GetMeansFromFile(filenamesl, 2); //TODO
        infol = TestFingersRight(tuplel, meansl);
        //Right Glove
        tuple[0] = VRPN.vrpnAnalog(nameRight, 0);
        tuple[1] = VRPN.vrpnAnalog(nameRight, 1);
        tuple[2] = VRPN.vrpnAnalog(nameRight, 2);
        tuple[3] = VRPN.vrpnAnalog(nameRight, 3);
        tuple[4] = VRPN.vrpnAnalog(nameRight, 4);
        tuple[5] = VRPN.vrpnAnalog(nameRight, 5);
        tuple[6] = VRPN.vrpnAnalog(nameRight, 6);
        tuple[7] = VRPN.vrpnAnalog(nameRight, 7);
        tuple[8] = VRPN.vrpnAnalog(nameRight, 8);
        tuple[9] = VRPN.vrpnAnalog(nameRight, 9);
        tuple[10] = VRPN.vrpnAnalog(nameRight, 10);
        tuple[11] = VRPN.vrpnAnalog(nameRight, 11);
        tuple[12] = VRPN.vrpnAnalog(nameRight, 12);
        tuple[13] = VRPN.vrpnAnalog(nameRight, 13);
        means = GetMeansFromFile(filenames, 2); //TODO
        info = TestFingersRight(tuple, means); //TODO

        //Old data
        if (info != null && infol != null)
        {
            infoAnt = info;
            infolAnt = infol;
        }

        /*
                if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            Debug.Log("Glove raw left = " + String.Join(", ",
                new List<double>(tuplel)
                .ConvertAll(i => i.ToString())
                .ToArray()));

            Debug.Log("GloveL = " + String.Join(", ",
                new List<int>(infol)
                .ConvertAll(i => i.ToString())
                .ToArray()));
            Debug.Log("Glove raw right= " + String.Join(", ",
                new List<double>(tuple)
                .ConvertAll(i => i.ToString())
                .ToArray()));

            Debug.Log("GloveR = " + String.Join(", ",
                new List<int>(info)
                .ConvertAll(i => i.ToString())
                .ToArray()));
        }
         */

    }

    public static double[] GetFingersTupleRight(double[] tuple, int finger)
    {
        double[] r = new double[2];
        finger++;
        switch (finger)
        {
            case 1:
                r[0] = tuple[0];
                r[1] = tuple[1];
                break;
            case 2:
                r[0] = tuple[3];
                r[1] = tuple[4];
                break;
            case 3:
                r[0] = tuple[6];
                r[1] = tuple[7];
                break;
            case 4:
                r[0] = tuple[9];
                r[1] = tuple[10];
                break;
            case 5:
                r[0] = tuple[12];
                r[1] = tuple[13];
                break;
            default:
                r[0] = -1;
                r[1] = -1;
                break;
        }
        return r;
    }

    public static double[] GetFingersTupleLeft(double[] tuple, int finger)
    {
        double[] r = new double[2];
        finger++;
        switch (finger)
        {
            case 1:
                r[0] = tuple[1];
                r[1] = tuple[0];
                break;
            case 2:
                r[0] = tuple[4];
                r[1] = tuple[3];
                break;
            case 3:
                r[0] = tuple[7];
                r[1] = tuple[6];
                break;
            case 4:
                r[0] = tuple[10];
                r[1] = tuple[9];
                break;
            case 5:
                r[0] = tuple[13];
                r[1] = tuple[12];
                break;
            default:
                r[0] = -1;
                r[1] = -1;
                break;
        }
        return r;
    }

    public static double[][][] GetMeansFromFile(string[] fileNames, int numK)
    {
        double[][][] means = new double[5][][];

        for (int i = 0; i < fileNames.Length; i++)
        {
            means[i] = new double[numK][];
            string filename = fileNames[i];
            String line; try
            {
                StreamReader sr = new StreamReader(filename);
                line = sr.ReadLine();

                int j = 0;
                while (line != null)
                {
                    Regex reg = new Regex(@"([-+]?[0-9]*\.?[0-9]+)");
                    int tam = 0;
                    foreach (Match match in reg.Matches(line))
                    {
                        tam++;
                    }
                    means[i][j] = new double[tam];

                    int k = 0;
                    foreach (Match match in reg.Matches(line))
                    {
                        means[i][j][k] = double.Parse(match.Value, CultureInfo.InvariantCulture.NumberFormat);
                        k++;
                    }

                    j++;
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block files.");
            }
        }

        return means;
    }

    public static int[] TestFingersRight(double[] tuple, double[][][] means)
    {

        int[] r = new int[5];

        for (int i = 0; i < 5; i++) //num dedos
        {
            int minIndex = 0;
            double min = Distance(GetFingersTupleRight(tuple, i), means[i][0]);
            for (int j = 1; j < means[i].Length; j++)
            {
                if (Distance(GetFingersTupleRight(tuple, i), means[i][j]) < min)
                {
                    minIndex = j;
                }
            }
            r[i] = TranslateSensorLeft(minIndex, i);
        }
        return r;
    }

    public static int[] TestFingersLeft(double[] tuple, double[][][] means)
    {

        int[] r = new int[5];

        for (int i = 0; i < 5; i++) //num dedos
        {
            int minIndex = 0;
            double min = Distance(GetFingersTupleLeft(tuple, i), means[i][0]);
            for (int j = 1; j < means[i].Length; j++)
            {
                if (Distance(GetFingersTupleLeft(tuple, i), means[i][j]) < min)
                {
                    minIndex = j;
                }
            }
            r[i] = TranslateSensorLeft(minIndex, i);
        }
        return r;
    }

    public static int TranslateSensorRight(int num, int finger)
    {
        Debug.Log(num);
        int r = 99;
        if (finger == 2 || finger == 1)
        {
            if (num == 0)
            {
                r = 1;
            }
            else if (num == 1) //TODO
            {
                r = 0;
            }
            else if (num == 2)
            {
                r = -1;
            }
        }
        else
        {
            if (num == 0)
            {
                r = 0;
            }
            else if (num == 1) //TODO
            {
                r = 1;
            }
            else if (num == 2)
            {
                r = -1;
            }
        }
        return r;
    }

    public static int TranslateSensorLeft(int num, int finger)
    {
        //Debug.Log(num);
        int r = 99;
        if (num == 0)
        {
            r = 0;
        }
        else if (num == 1) //TODO
        {
            r = 1;
        }
        else if (num == 2)
        {
            r = -1;
        }
        return r;
    }

    private static double Distance(double[] tuple, double[] mean)
    {
        double sumSquaredDiffs = 0.0;
        for (int j = 0; j < tuple.Length; ++j)
            sumSquaredDiffs += Math.Pow((tuple[j] - mean[j]), 2);
        return Math.Sqrt(sumSquaredDiffs);

    }
}
