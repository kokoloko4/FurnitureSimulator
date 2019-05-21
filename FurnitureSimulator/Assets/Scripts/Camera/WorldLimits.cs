using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldLimits : MonoBehaviour
{
    //Virtual limits
    public Vector3 MinPositionsV;
    public Vector3 MaxPositionsV;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        MoveObject();
    }

    void MoveObject()
    {
        float posX = Mathf.Clamp(transform.position.x, MinPositionsV.x, MaxPositionsV.x);
        float posY = Mathf.Clamp(transform.position.y, MinPositionsV.y, MaxPositionsV.y);
        float posZ = Mathf.Clamp(transform.position.z, MinPositionsV.z, MaxPositionsV.z);
        transform.position = new Vector3(posX, posY, posZ);
    }
}
