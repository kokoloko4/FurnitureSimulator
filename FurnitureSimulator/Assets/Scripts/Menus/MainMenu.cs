using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WiimoteApi;

public class MainMenu : MonoBehaviour
{
    public Wiimote controlWii = null;
    private bool activated = false;
    private Button actualButton;
    private GameObject botones;
    Vector3 wmpOffset;
    //States
    private int NunAnt = 0;
    private int NunAct = 0;
    //Opciones
    private int cantOpts = 4;
    private int index = 0;


    // Start is called before the first frame update
    void Start()
    {
        //WiimoteManager.FindWiimotes();
        //controlWii = WiimoteManager.Wiimotes[0];
        botones = GameObject.FindGameObjectWithTag("botones");
        actualButton = botones.transform.GetChild(index).GetComponent<Button>();
        actualButton.tag = "select";
        actualButton.GetComponent<Image>().color = Color.gray;
        actualButton.OnSelect(null);
    }

    // Update is called once per frame
    void Update()
    {
        //NunAnt = NunAct;
        //NunAct = GetBaseInput();
        //Debug.Log(NunchuckDirection());
        if (Input.GetKeyDown(KeyCode.DownArrow) && index < cantOpts)
        {
            index++;
            actualButton.tag = "Untagged";
            actualButton.GetComponent<Image>().color = Color.white;
            //===
            if (index == 4)
            {
                index = cantOpts - 1;
            }
            actualButton = botones.transform.GetChild(index).GetComponent<Button>();
            actualButton.tag = "select";
            actualButton.GetComponent<Image>().color = Color.gray;
            actualButton.OnSelect(null);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && index >= 0)
        {
            index--;
            actualButton.tag = "Untagged";
            actualButton.GetComponent<Image>().color = Color.white;
            //===
            if (index == -1)
            {
                index = 0;
            }
            actualButton = botones.transform.GetChild(index).GetComponent<Button>();
            actualButton.tag = "select";
            actualButton.GetComponent<Image>().color = Color.gray;
            actualButton.OnSelect(null);
        }
    }

    /*private int GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        int mov = 0;

        if (controlWii.current_ext == ExtensionController.NUNCHUCK)
        {
            NunchuckData data = controlWii.Nunchuck;
            Debug.Log("Stick: " + data.stick[0] + ", " + data.stick[1]);
            if (data.stick[1] - 130 > 10 )
            {
                mov = 1;
            }
            if (data.stick[1] - 130 < -10)
            {
                mov = -1;
            }
        }
        return mov;
    }

    //returns 1 for up, returns -1 for down
    private int NunchuckDirection()
    {
        if (NunAct == 1 && NunAnt == 0)
            return 1;
        if (NunAct == -1 && NunAnt == 0)
            return -1;
        return 0;
    }*/
}
