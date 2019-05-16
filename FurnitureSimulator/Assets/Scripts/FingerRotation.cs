using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void RotateFinger(Vector3 Rotations)
    {
        transform.localEulerAngles = new Vector3(0, 0, Rotations.x);
        transform.GetChild(0).localEulerAngles = new Vector3(0, 0, Rotations.y);
        transform.GetChild(0).GetChild(0).localEulerAngles = new Vector3(0, 0, Rotations.z);
    }
}
