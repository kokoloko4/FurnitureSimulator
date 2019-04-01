using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public int cantOpt;
    public float r;
    private GameObject menu;
    public Material m1;
    public Material m2;
    public float distZ;
    private GameObject[] opts;
    private GameObject activeOpt;
    public float targetDistance;
    public LayerMask layerMask;
    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.FindGameObjectWithTag("menu");
        CreateMenu();
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
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RightOption(ref activeOpt);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            LeftOption(ref activeOpt);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && menu.activeSelf == false)
        {
            menu.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && menu.activeSelf == true)
        {
            menu.SetActive(false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit optHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out optHit, targetDistance, layerMask))
            {
                if (optHit.collider.tag.Equals("active"))
                {
                    Debug.Log(optHit.collider.name);
                }
            }
        }
    }

    void CreateMenu()
    {
        int anguloSep = 360 / cantOpt;
        float angCubo = 90;
        for (int i = 0; i < cantOpt; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = i.ToString();
            float x = r * Mathf.Cos(angCubo * Mathf.Deg2Rad);
            float y = r * Mathf.Sin(angCubo * Mathf.Deg2Rad);
            if (i == 0)
            {
                cube.tag = "active";
                cube.transform.position = new Vector3(x, y, distZ-(0.2f));
            }
            else
            {
                cube.tag = "not active";
                cube.transform.position = new Vector3(x, y, distZ);
            }
            cube.transform.localScale = new Vector3(0.1f, 0.05f, 0.01f);
            cube.GetComponent<Renderer>().material.color = Random.ColorHSV();
            cube.transform.parent = menu.transform;
            angCubo += anguloSep;
        }
        opts = new GameObject[menu.transform.childCount];
        for (int i = 0; i < menu.transform.childCount; ++i)
        {
            opts[i] = menu.transform.GetChild(i).gameObject;
        }
    }

    void RightOption(ref GameObject activeOpt)
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
        opts[0].transform.position = new Vector3(x,y,z);
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
