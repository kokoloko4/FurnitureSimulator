using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsLimits : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	private void OnCollisionStay(Collision collision)
	{
		GameObject obj = collision.gameObject;
		if (obj.name.Equals("left wall"))
		{
			transform.Translate(new Vector3(0.1f, 0, 0));
		}
		if (obj.name.Equals("floor"))
		{
			transform.Translate(new Vector3(0, 0.1f, 0));
		}
	}
}
