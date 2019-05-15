using UnityEngine;

public class FingersMove : MonoBehaviour
{
    //Time
    private float nextActionTime = 0.0f;
    private float period = 0.1f;
    //Gloves
    private Gloves5DT Gloves;
    //Animations
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        Gloves = new Gloves5DT("TEST");
        anim = GetComponent<Animator>();
        //anim.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            //Gloves.GetFingersData("Glove14Left@10.3.136.131", "Glove14Right@10.3.136.131");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.Play("ClosePinkyRight");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.Play("OpenPinkyRight");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.Play("CloseIndexRight");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.Play("OpenindexRight");
        }
    }
}
