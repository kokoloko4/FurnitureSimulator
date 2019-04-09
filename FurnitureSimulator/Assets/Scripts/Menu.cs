using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Linq;

public class Menu : MonoBehaviour
{
    //Global Variables
    private GameObject g;
    //Menu
    public int cantOpt;
    public float r;
    private GameObject menu;
    public float distZ;
    private GameObject[] opts;
    public GameObject activeOpt;
    public float targetDistance;
    public LayerMask layerMask;
    private GameObject centerObject;
    //Joystick variables
    private double flechaV;
    private double flechaH;
    private bool aButton;
    private bool changeState = false;
    private bool prevXButton = false;
    //Time
    private float nextActionTime = 0.0f;
    public float period = 0.1f;
    private float nextActionTimeHalf = 0.0f;
    public float periodHalf = 0.05f;
    //Tracker
    private Vector3 posVRPN;
    private float x;
    private float y;
    private float z;
    private Queue<Vector3> bufferTracker;
    private int tamBuffer = 20;
    //Z Movement
    private float minArmDistanceZ = 1.5f;
    private float maxArmDistanceZ = 1.9f;
    private bool moveArmZ = false;
    private int dirArmZ = 0;
    //Y Movement
    private float minArmDistanceY = 6.0f;
    private float maxArmDistanceY = 7.0f;
    private bool moveArmY = false;
    private int dirArmY = 0;
    //Gloves
    private string path = "C:\\Users\\Takina\\Documents\\GitHub\\FurnitureSimulator\\FurnitureSimulator\\Assets\\DataGloves\\";
    //    "/Users/licho/Documents/Unity/FurnitureSimulator/FurnitureSimulator/Assets/DataGloves/";
    //Right Glove
    private string[] filenames;
    private double[][][] means;
    private int[] info;
    private double[] tuple;
    //Left Glove
    private string[] filenamesl;
    private double[][][] meansl;
    private int[] infol;
    private double[] tuplel;

    // Start is called before the first frame update
    void Start()
    {
        g = GameObject.Find("Global");
        menu = GameObject.FindGameObjectWithTag("menu");
        CreateMenu();
        //Data Gloves
        filenames = new string[5];
        //Right gloves
        filenames[0] = path + "finger13.txt";
        filenames[1] = path + "finger23.txt";
        filenames[2] = path + "finger33.txt";
        filenames[3] = path + "finger43.txt";
        filenames[4] = path + "finger53.txt";
        //Left gloves
        filenamesl = new string[5];
        filenamesl[0] = path + "finger13l.txt";
        filenamesl[1] = path + "finger23l.txt";
        filenamesl[2] = path + "finger33l.txt";
        filenamesl[3] = path + "finger43l.txt";
        filenamesl[4] = path + "finger53l.txt";

        tuple = new double[14];
        tuplel = new double[14];

        //Tracker
        bufferTracker = new Queue<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        int i = 0;
        while (i < opts.Length)
        {
            if (opts[i].tag == "active")
            {
                activeOpt = opts[i];
            }
            i++;
        }
        InputDataControl("Joylin1@10.3.136.131");
        InputControl();
        if (Time.time > nextActionTimeHalf)
        {
            nextActionTimeHalf += periodHalf;
            InputDataTracker("Tracker0@10.3.137.218");
            InputDataGloves("Glove14Right@10.3.137.218", "Glove14Left@10.3.137.218");
            InputTrackerGloves();
        }
    }

    void CreateMenu()
    {
        int anguloSep = 360 / cantOpt;
        float angCubo = 90;
        for (int i = 0; i < cantOpt; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Quad);
            cube.name = i.ToString();
            float x = r * Mathf.Cos(angCubo * Mathf.Deg2Rad);
            float y = r * Mathf.Sin(angCubo * Mathf.Deg2Rad);
            if (i == 0)
            {
                cube.tag = "active";
                cube.transform.position = new Vector3(0,0, distZ - (0.4f));
            }
            else
            {
                cube.tag = "not active";
                cube.transform.position = new Vector3(x, y, distZ);
            }
            if(i%3 == 0)
            {
                Texture2D myTexture = Resources.Load("gatito") as Texture2D;
                cube.GetComponent<Renderer>().material.mainTexture = myTexture;
            }
            else if(i%3 == 1)
            {
                AddVideo(ref cube, "video");
                cube.GetComponent<VideoPlayer>().SetDirectAudioMute(0,true);
            }
            else
            {
                AddModel(ref cube);
            }
            cube.transform.localScale = new Vector3(0.3f, 0.2f, 0.01f);
            cube.transform.parent = menu.transform;
            angCubo += anguloSep;
        }
        opts = new GameObject[menu.transform.childCount];
        for (int i = 0; i < menu.transform.childCount; ++i)
        {
            opts[i] = menu.transform.GetChild(i).gameObject;
        }
    }

    void AddModel(ref GameObject parent)
    {
        parent.GetComponent<Renderer>().material = Resources.Load<Material>("ColorMenu2") as Material;
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        obj.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV();
        obj.transform.parent = parent.transform;
        obj.transform.localScale = new Vector3(parent.transform.localScale.x - 0.5f, parent.transform.localScale.y - 0.5f , parent.transform.localScale.z);
        obj.transform.localEulerAngles = new Vector3(40f, 0f, 0f);
    }

    void AddVideo (ref GameObject obj, string videoName)
    {
        VideoPlayer video = obj.AddComponent<VideoPlayer>();
        AudioSource audio = obj.AddComponent<AudioSource>();
        video.clip = Resources.Load<VideoClip>(videoName) as VideoClip;
        video.isLooping = true;
        video.renderMode = VideoRenderMode.MaterialOverride;
        video.targetMaterialRenderer = GetComponent<Renderer>();
        video.targetMaterialProperty = "_MainTex";
        video.audioOutputMode = VideoAudioOutputMode.AudioSource;
        video.SetTargetAudioSource(0, audio);
        video.Play();
    }

    public void RightOption(ref GameObject activeOpt)
    {
        float x = opts[cantOpt - 1].transform.position.x;
        float y = opts[cantOpt - 1].transform.position.y;
        float z = opts[cantOpt - 1].transform.position.z;
        for (int i = cantOpt - 1; i > 0; i--)
        {
            opts[i].transform.position = new Vector3(opts[i - 1].transform.position.x, opts[i - 1].transform.position.y, opts[i - 1].transform.position.z);
            if (i == int.Parse(activeOpt.name) + 1)
            {
                opts[i].tag = "active";
            }
        }
        if (int.Parse(activeOpt.name) + 1 == cantOpt)
        {
            opts[0].tag = "active";
        }
        opts[0].transform.position = new Vector3(x, y, z);
        activeOpt.tag = "not active";
    }

    void LeftOption(ref GameObject activeOpt)
    {
        float x = opts[0].transform.position.x;
        float y = opts[0].transform.position.y;
        float z = opts[0].transform.position.z;
        for (int i = 0; i < cantOpt - 1; i++)
        {
            opts[i].transform.position = new Vector3(opts[i + 1].transform.position.x, opts[i + 1].transform.position.y, opts[i + 1].transform.position.z);
            if (i == int.Parse(activeOpt.name) - 1)
            {
                opts[i].tag = "active";
            }
        }
        if (int.Parse(activeOpt.name) - 1 == -1)
        {
            opts[cantOpt - 1].tag = "active";
        }
        opts[cantOpt - 1].transform.position = new Vector3(x, y, z);
        activeOpt.tag = "not active";
    }

    void InputDataControl(string address)
    {
        if (Time.time > nextActionTime)
        {
            flechaV = VRPN.vrpnAnalog(address, 3);
            flechaH = VRPN.vrpnAnalog(address, 2);
            aButton = VRPN.vrpnButton(address, 0);
        }
    }

    void InputControl()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            if (flechaH == 1)
            {
                RightOption(ref activeOpt);
            }
            else if (flechaH == -1)
            {
                LeftOption(ref activeOpt);
            }
        }
        if (aButton)
        {
            EnterOption();
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

    void InputDataGloves(string addressRight, string addressLeft)
    {
        tuplel[0] = VRPN.vrpnAnalog(addressLeft, 0);
        tuplel[1] = VRPN.vrpnAnalog(addressLeft, 1);
        tuplel[2] = VRPN.vrpnAnalog(addressLeft, 2);
        tuplel[3] = VRPN.vrpnAnalog(addressLeft, 3);
        tuplel[4] = VRPN.vrpnAnalog(addressLeft, 4);
        tuplel[5] = VRPN.vrpnAnalog(addressLeft, 5);
        tuplel[6] = VRPN.vrpnAnalog(addressLeft, 6);
        tuplel[7] = VRPN.vrpnAnalog(addressLeft, 7);
        tuplel[8] = VRPN.vrpnAnalog(addressLeft, 8);
        tuplel[9] = VRPN.vrpnAnalog(addressLeft, 9);
        tuplel[10] = VRPN.vrpnAnalog(addressLeft, 10);
        tuplel[11] = VRPN.vrpnAnalog(addressLeft, 11);
        tuplel[12] = VRPN.vrpnAnalog(addressLeft, 12);
        tuplel[13] = VRPN.vrpnAnalog(addressLeft, 13);

        meansl = GetMeansFromFile(filenamesl, 2); //TODO
        infol = TestFingers(tuplel, meansl);

        /*
        Debug.Log("Glove raw left = " + String.Join(", ",
            new List<double>(tuplel)
            .ConvertAll(i => i.ToString())
            .ToArray()));


        Debug.Log("GloveL = " + String.Join(", ",
            new List<int>(infol)
            .ConvertAll(i => i.ToString())
            .ToArray()));
        */

        tuple[0] = VRPN.vrpnAnalog(addressRight, 0);
        tuple[1] = VRPN.vrpnAnalog(addressRight, 1);
        tuple[2] = VRPN.vrpnAnalog(addressRight, 2);
        tuple[3] = VRPN.vrpnAnalog(addressRight, 3);
        tuple[4] = VRPN.vrpnAnalog(addressRight, 4);
        tuple[5] = VRPN.vrpnAnalog(addressRight, 5);
        tuple[6] = VRPN.vrpnAnalog(addressRight, 6);
        tuple[7] = VRPN.vrpnAnalog(addressRight, 7);
        tuple[8] = VRPN.vrpnAnalog(addressRight, 8);
        tuple[9] = VRPN.vrpnAnalog(addressRight, 9);
        tuple[10] = VRPN.vrpnAnalog(addressRight, 10);
        tuple[11] = VRPN.vrpnAnalog(addressRight, 11);
        tuple[12] = VRPN.vrpnAnalog(addressRight, 12);
        tuple[13] = VRPN.vrpnAnalog(addressRight, 13);

        means = GetMeansFromFile(filenames, 2); //TODO
        info = TestFingers(tuple, means); //TODO

        /*
        Debug.Log("Glove raw right= " + String.Join(", ",
            new List<double>(tuple)
            .ConvertAll(i => i.ToString())
            .ToArray()));

        Debug.Log("GloveR = " + String.Join(", ",
            new List<int>(info)
            .ConvertAll(i => i.ToString())
            .ToArray()));
        */
    }

    void InputTrackerGloves()
    {
        Debug.Log("tbuffer" + bufferTracker.ElementAt(tamBuffer-1));

        ZMovement();
        YMovement();

    }

    void ZMovement()
    {
        dirArmZ = 0;
        moveArmZ = false;
        int i = 1;
        if (bufferTracker.Count >= 20)
        {
            while (!moveArmZ && i < tamBuffer - 1)
            {
                float dif = Math.Abs(bufferTracker.ElementAt(tamBuffer - 1).z - bufferTracker.ElementAt(tamBuffer - i - 1).z);
                if (dif >= minArmDistanceZ && dif <= maxArmDistanceZ)
                {
                    moveArmZ = true;
                    if (bufferTracker.ElementAt(tamBuffer - 1).z > bufferTracker.ElementAt(tamBuffer - i - 1).z)
                    {
                        dirArmZ = -1;
                    }
                    else
                    {
                        dirArmZ = 1;
                    }
                }
                i++;
            }
            //Move menu
            if (moveArmZ)
            {
                //Right move
                if (dirArmZ == 1)
                {
                    RightOption(ref activeOpt);
                }
                //Left move
                else if (dirArmZ == -1)
                {
                    LeftOption(ref activeOpt);
                }
                bufferTracker.Clear();
            }
            //Debug.Log("¿hay movimiento? "+move);
        }
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
                    EnterOption();
                }
                bufferTracker.Clear();
            }
            //Debug.Log("¿hay movimiento? "+ moveArmY);
        }
    }

    public void EnterOption()
    {
        if (activeOpt.transform.childCount > 0)
        {
            SceneManager.LoadScene("ModelScene");
        }
        else if (activeOpt.GetComponent<VideoPlayer>() != null)
        {
            g.GetComponent<GlobalVars>().nameResource = "video";
            SceneManager.LoadScene("VideoScene");
        }
        else if (activeOpt.GetComponent<VideoPlayer>() == null)
        {
            g.GetComponent<GlobalVars>().nameResource = "ColorMenu1";
            SceneManager.LoadScene("ImageScene");
        }
    }

    public static double[] GetFingersTuple(double[] tuple, int finger)
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

    public static int[] TestFingers(double[] tuple, double[][][] means)
    {

        int[] r = new int[5];

        for (int i = 0; i < 5; i++) //num dedos
        {
            int minIndex = 0;
            double min = Distance(GetFingersTuple(tuple, i), means[i][0]);
            for (int j = 1; j < means[i].Length; j++)
            {
                if (Distance(GetFingersTuple(tuple, i), means[i][j]) < min)
                {
                    minIndex = j;
                }
            }
            r[i] = TranslateSensor(minIndex, i);
        }
        return r;
    }

    public static int TranslateSensor(int num, int finger)
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
