using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WiimoteApi;


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
    //Hands
    public GameObject RightHand;
    //Wii
    public Wiimote controlWii;
    //Furniture
    public Vector3 actualScale = Vector3.zero;
    //States
    private int NunDir = 0;
    private int NunAct = 0;


    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.FindGameObjectWithTag("menu");
        menu.transform.localPosition = new Vector3(0, 0, 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        MainMenu();
        InputControl();
    }

    void MainMenu()
    {
        WiimoteManager.FindWiimotes();
        controlWii = WiimoteManager.Wiimotes[0];
        //controlWii = transform.parent.GetComponent<MoveCamera>().controlWii;
        controlWii.SetupIRCamera(IRDataType.BASIC);
        if (controlWii != null)
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

            if (data.c && data.z && created == false)
            {
                menu.transform.parent.GetComponent<MoveCamera>().enabled = false;
                CreateMenu();
                created = true;
            }
            else if (data.z && !data.c && created == true)
            {
                DestroyMenu();
                Destroy(GameObject.FindGameObjectWithTag("selection"));
                menu.transform.parent.GetComponent<MoveCamera>().enabled = true;
                created = false;
                controlWii = null;
            }
            if (created == true)
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
            }

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
        selection.transform.localEulerAngles = new Vector3(0, 0, 0);
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
            cube.transform.localEulerAngles = new Vector3(0, 0, 0);
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
        if (controlWii != null)
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
            //Debug.Log("Stick: " + data.stick[0] + ", " + data.stick[1]);

            if (NunDir != 0)
            {
                if (data.stick[0] - 125 < -90 && !data.c && !data.z)
                {
                    NunAct = -1;
                }
                else if (data.stick[0] - 125 > 90 && !data.c && !data.z)
                {
                    NunAct = 1;
                }
                else
                {
                    NunAct = 0;
                }
            }
            NunDir = 0;

            if (data.stick[0] - 125 < -90 && !data.c && !data.z)
            {
                NunDir = -1;
            }
            else if (data.stick[0] - 125 > 90 && !data.c && !data.z)
            {
                NunDir = 1;
            }
            if(NunAct == 0)
            {
                if (NunDir == -1 && !data.c && !data.z)
                {
                    RightOption(ref activeOpt);
                }
                else if (NunDir == 1 && !data.c && !data.z)
                {
                    LeftOption(ref activeOpt);
                }
                if (data.c)
                {
                    SelectFurniture();
                }
            }
        }

    }

    void SelectFurniture()
    {
        NunchuckData data = controlWii.Nunchuck;
        int nameFurniture;
        if (menu.transform.childCount > 0)
        {
            int.TryParse(GameObject.FindGameObjectWithTag("active").name, out nameFurniture);
            if (data.c && !data.z)
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
                GameObject vista = GameObject.FindGameObjectWithTag("vista");
                Instantiate(furniture, new Vector3(vista.transform.position.x, vista.transform.position.y - 0.5f, vista.transform.position.z + 0.5f), transform.rotation * Quaternion.Euler(270f, 0f, 0f));
                actualScale = furniture.transform.localScale;
                menu.transform.parent.GetComponent<MoveCamera>().enabled = true;
                Destroy(GameObject.FindGameObjectWithTag("selection"));
                DestroyMenu();
                controlWii = null;
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
}
