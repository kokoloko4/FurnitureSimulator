using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject activeOpt;
    // Menu
    private int cantOpt = 7;
    private float r = 1;
    public float distZ;
    private GameObject menu;
    private GameObject[] opts;
    private bool created = false;
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
    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.FindGameObjectWithTag("menu");
        menu.transform.localPosition = new Vector3(0,0,2.5f);
        bufferTracker = new Queue<Vector3>();

    }

    // Update is called once per frame
    void Update()
    {
        MainMenu();
        /*InputControl();
        if (Time.time > nextActionTimeHalf)
        {
            nextActionTimeHalf += periodHalf;
            InputDataTracker("Tracker0@10.3.136.131");
            InputTrackerGloves();
        }*/
    }

    void MainMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && created == false)
        {
            menu.transform.parent.GetComponent<MoveCamera>().enabled = false;
            CreateMenu();
            created = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && created == true)
        {
            DestroyMenu();
            Destroy(GameObject.FindGameObjectWithTag("selection"));
            menu.transform.parent.GetComponent<MoveCamera>().enabled = true;
        }
        int i = 0;
        while (i < opts.Length)
        {
            if (opts[i].tag == "active")
            {
                activeOpt = opts[i];
            }
            i++;
        }        
    }

    void DestroyMenu()
    {
        foreach (Transform child in menu.transform)
        {
            Destroy(child.gameObject);
        }
        created = false;
    }

    void CreateMenu()
    {
        int anguloSep = 360 / cantOpt;
        float angCubo = 90;
        GameObject parent = menu.transform.parent.gameObject;
        Material quadMaterial = (Material)Resources.Load("menuMatSel") as Material;
        Texture2D myTexture;
        GameObject selection = GameObject.CreatePrimitive(PrimitiveType.Quad);
        selection.name = "selection";
        selection.transform.parent = menu.transform.parent.transform;
        selection.transform.localPosition = new Vector3(0, 1.2f, 3);
        selection.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        selection.tag = "selection";
        selection.transform.localEulerAngles = new Vector3(0,0,0);
        selection.GetComponent<Renderer>().material = quadMaterial;
        for (int i = 0; i < cantOpt; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Quad);
            cube.name = i.ToString();
            float x = r * Mathf.Cos(angCubo * Mathf.Deg2Rad);
            float y = r * Mathf.Sin(angCubo * Mathf.Deg2Rad);
            if (i == 0)
            {
                cube.tag = "active";
            }
            else
            {
                cube.tag = "not active";
            }
            quadMaterial = (Material)Resources.Load("menuMat") as Material;
            cube.GetComponent<Renderer>().material = quadMaterial;
            switch (i)
            {
                case 0:
                    myTexture = Resources.Load("silla") as Texture2D;
                    cube.GetComponent<Renderer>().material.mainTexture = myTexture;
                    break;
                case 1:
                    myTexture = Resources.Load("vase") as Texture2D;
                    cube.GetComponent<Renderer>().material.mainTexture = myTexture;
                    break;
                case 2:
                    myTexture = Resources.Load("sofa1") as Texture2D;
                    cube.GetComponent<Renderer>().material.mainTexture = myTexture;
                    break;
                case 3:
                    myTexture = Resources.Load("mesa") as Texture2D;
                    cube.GetComponent<Renderer>().material.mainTexture = myTexture;
                    break;
                case 4:
                    myTexture = Resources.Load("television") as Texture2D;
                    cube.GetComponent<Renderer>().material.mainTexture = myTexture;
                    break;
                case 5:
                    myTexture = Resources.Load("bed") as Texture2D;
                    cube.GetComponent<Renderer>().material.mainTexture = myTexture;
                    break;
                case 6:
                    myTexture = Resources.Load("bookcase") as Texture2D;
                    cube.GetComponent<Renderer>().material.mainTexture = myTexture;
                    break;
            }
            cube.transform.parent = menu.transform;
            cube.transform.localEulerAngles = new Vector3(0,0,0);
            cube.transform.localPosition = new Vector3(x, y, distZ);
            cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            angCubo += anguloSep;
        }
        opts = new GameObject[menu.transform.childCount];
        for (int i = 0; i < menu.transform.childCount; ++i)
        {
            opts[i] = menu.transform.GetChild(i).gameObject;
        }
    }

    void InputControl()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            RightOption(ref activeOpt);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            LeftOption(ref activeOpt);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        { 
            SelectFurniture();
        }
    }

    void SelectFurniture()
    {
        int nameFurniture;
        if(menu.transform.childCount > 0)
        {
            int.TryParse(GameObject.FindGameObjectWithTag("active").name, out nameFurniture);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject furniture = null;
                switch (nameFurniture)
                {
                    case 0:
                        furniture = Resources.Load<GameObject>("Chair") as GameObject;
                        break;
                    case 1:
                        furniture = Resources.Load<GameObject>("Vase_1") as GameObject;
                        break;
                    case 2:
                        furniture = Resources.Load<GameObject>("Sofa") as GameObject;
                        break;
                    case 3:
                        furniture = Resources.Load<GameObject>("Table") as GameObject;
                        break;
                    case 4:
                        furniture = Resources.Load<GameObject>("TV") as GameObject;
                        break;
                    case 5:
                        furniture = Resources.Load<GameObject>("Bed") as GameObject;
                        break;
                    case 6:
                        furniture = Resources.Load<GameObject>("BookCase_1") as GameObject;
                        break;
                }
                Instantiate(furniture, new Vector3(5, 2, 4), transform.rotation * Quaternion.Euler(270f, 0f, 0f));
                menu.transform.parent.GetComponent<MoveCamera>().enabled = true;
                Destroy(GameObject.FindGameObjectWithTag("selection"));
                DestroyMenu();
            }
        }
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
                float dif = Mathf.Abs(bufferTracker.ElementAt(tamBuffer - 1).z - bufferTracker.ElementAt(tamBuffer - i - 1).z);
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
                float dif = Mathf.Abs(bufferTracker.ElementAt(tamBuffer - 1).y - bufferTracker.ElementAt(tamBuffer - i - 1).y);
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
                    SelectFurniture();
                }
                bufferTracker.Clear();
            }
            //Debug.Log("¿hay movimiento? "+ moveArmY);
        }
    }
}
