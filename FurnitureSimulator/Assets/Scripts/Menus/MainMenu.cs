using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WiimoteApi;

public class MainMenu : MonoBehaviour
{
    public Wiimote controlWii = null;
    private bool activated = false;
    public GameObject rooms;
    private Button actualButton;
    private GameObject botones;
    Vector3 wmpOffset;
    //States
    private int NunDir = 0;
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
        WiimoteManager.FindWiimotes();
        controlWii = WiimoteManager.Wiimotes[0];
    }

    // Update is called once per frame
    void Update()
    {
        int ret;
        do
        {
            ret = controlWii.ReadWiimoteData();


        } while (ret > 0);

        controlWii.SetupIRCamera(IRDataType.BASIC);

        if (controlWii.current_ext == ExtensionController.NUNCHUCK)
        {

            NunchuckData data = controlWii.Nunchuck;
            if (NunDir != 0)
            {
                if (data.stick[1] - 130 < -90 && !data.c && !data.z)
                {
                    NunAct = -1;
                }
                else if (data.stick[1] - 130 > 90 && !data.c && !data.z)
                {
                    NunAct = 1;
                }
                else
                {
                    NunAct = 0;
                }
            }
            NunDir = 0;

            if (data.stick[1] - 130 < -90 && !data.c && !data.z)
            {
                NunDir = -1;
            }
            else if (data.stick[1] - 130 > 90 && !data.c && !data.z)
            {
                NunDir = 1;
            }

            if(NunAct == 0)
            {
                if (NunDir == -1 && !data.c && !data.z && index < cantOpts)
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
                else if (NunDir == 1 && !data.c && !data.z && index >= 0)
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


            if (data.c)
            {
                switch (index)
                {
                    case 0:
                        rooms.GetComponent<RoomsMgr>().LoadRoom("Habitacion3");
                        break;
                    case 1:
                        rooms.GetComponent<RoomsMgr>().LoadRoom("Habitacion2");
                        break;
                    case 2:
                        rooms.GetComponent<RoomsMgr>().LoadRoom("Habitacion1");
                        break;
                    case 3:
                        rooms.GetComponent<RoomsMgr>().ExitSimulator();
                        break;
                }
            }
        }
    }
}
