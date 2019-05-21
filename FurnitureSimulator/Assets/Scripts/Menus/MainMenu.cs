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
    Vector3 wmpOffset;
    //States
    private int NunAnt = 0;
    private int NunAct = 0;

    // Start is called before the first frame update
    void Start()
    {
        WiimoteManager.FindWiimotes();
        controlWii = WiimoteManager.Wiimotes[0];
        /*actualButton = GameObject.FindGameObjectWithTag("select").GetComponent<Button>();
        actualButton.GetComponent<Image>().color = Color.gray;
        actualButton.OnSelect(null);*/
    }

    // Update is called once per frame
    void Update()
    {
        NunAnt = NunAct;
        NunAct = GetBaseInput();
        //Debug.Log(NunchuckDirection());
    }

    private int GetBaseInput()
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
    }
}
