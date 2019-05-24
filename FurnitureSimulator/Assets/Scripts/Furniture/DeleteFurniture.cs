using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteFurniture : MonoBehaviour
{
    private CollisionHands collisionHands;
    // Start is called before the first frame update
    void Start()
    {
        collisionHands = GetComponent<CollisionHands>();
    }

    // Update is called once per frame
    void Update()
    {
        if(collisionHands.TouchingFurniture() && GetComponent<CollisionHands>().isGrabbed && Input.GetKey(KeyCode.Q))
        {
            Destroy(this.gameObject);
        }
    }
}
