using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    public float speed = 0.01f;
    private GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.FindGameObjectWithTag("menu");
    }

    // Update is called once per frame
    void Update()
    {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        //transform.Rotate(new Vector3(0.0f,yaw, pitch));
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        }
    }
}