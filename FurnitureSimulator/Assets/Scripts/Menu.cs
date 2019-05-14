using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    // Menu
    private int cantOpt = 7;
    private float r = 0.8f;
    public float distZ;
    private GameObject menu;
    private GameObject[] opts;
    private bool created = false;
    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.FindGameObjectWithTag("menu");
    }

    // Update is called once per frame
    void Update()
    {
        MainMenu();
        SelectFurniture();
    }

    void MainMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && created == false)
        {
            CreateMenu();
            created = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && created == true)
        {
            DestroyMenu();
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
        Material quadMaterial;
        Texture2D myTexture;
        GameObject selection = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quadMaterial = (Material)Resources.Load("menuMatSel") as Material;
        selection.name = "selection";
        selection.transform.parent = menu.transform;
        selection.transform.position = new Vector3(5,4, 5.5f);
        selection.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        selection.GetComponent<Renderer>().material = quadMaterial;
        for (int i = 0; i < cantOpt; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Quad);
            cube.name = i.ToString();
            cube.transform.parent = menu.transform;
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
            cube.transform.position = new Vector3(x + 5, y + 3, distZ);
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
                    myTexture = Resources.Load("sofa") as Texture2D;
                    cube.GetComponent<Renderer>().material.mainTexture = myTexture;
                    break;
                case 3:
                    myTexture = Resources.Load("mesa") as Texture2D;
                    cube.GetComponent<Renderer>().material.mainTexture = myTexture;
                    break;
                case 4:
                    myTexture = Resources.Load("tv") as Texture2D;
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
            cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            angCubo += anguloSep;
        }
        opts = new GameObject[menu.transform.childCount];
        for (int i = 0; i < menu.transform.childCount; ++i)
        {
            opts[i] = menu.transform.GetChild(i).gameObject;
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
                Instantiate(furniture, new Vector3(5, 2, 3f), Quaternion.identity);
                DestroyMenu();
            }
        }
    }
}
