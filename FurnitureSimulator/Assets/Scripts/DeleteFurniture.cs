using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteFurniture : MonoBehaviour
{
    private GameObject RightHand = null;
    private GameObject LeftHand = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RightHand = GetComponent<CollisionHands>().RightHand;
        LeftHand = GetComponent<CollisionHands>().LeftHand;
        if(RightHand != null && LeftHand != null && Input.GetKey(KeyCode.Q))
        {
            Destroy(this.gameObject);
        }
    }
}
