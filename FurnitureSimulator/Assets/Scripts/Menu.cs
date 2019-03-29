using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public int cantOpt;
    public int r;
    private GameObject menu;
    public Material m1;
    public Material m2;
    public float distZ;
    // Start is called before the first frame update
    void Start()
    {
        NewMenu();
    }

    // Update is called once per frame
    void Update()
    {
        NextOption();
    }

    void NewMenu()
    {
        menu = GameObject.FindGameObjectWithTag("menu");
        int anguloSep = 360 / cantOpt;
        float angCubo = 90;
        for (int i = 0; i < cantOpt; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.tag = "opt" + i;
            float x = r * Mathf.Cos(angCubo * Mathf.Deg2Rad);
            float y = r * Mathf.Sin(angCubo * Mathf.Deg2Rad);
            if (i == 0)
            {
                cube.transform.position = new Vector3(x, y, distZ);
            }
            else
            {
                cube.transform.position = new Vector3(x, y, 0);
            }
            cube.transform.localScale = new Vector3(2.0f, 1.0f, 0.01f);
            cube.AddComponent<Renderer>();
            if (i % 2 == 0)
            {
                cube.GetComponent<Renderer>().material = m1;
            }
            else
            {
                cube.GetComponent<Renderer>().material = m2;
            }

            cube.transform.parent = menu.transform;
            angCubo += anguloSep;
        }
    }

    void NextOption()
    {

    }
}
