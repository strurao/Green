using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine.EventSystems;

public class cshChoiceBtn : MonoBehaviourPunCallbacks
{
    public Button[] Buttons;
    private GameObject[] spawnTrees;
    public int ButtonId;
    public bool AnswerCheck = false;
    public string ManagerName;
    private string[] Split_ButtonName;


    public PhotonView PV;


    cshWarningManager cwm;
    cshMissionManager cmm;
    cshBoilerManager cbm;
    cshPanelManager cpm;
    cshARInteraction cai;



    // Start is called before the first frame update
    void Start()
    {
        cmm = GameObject.Find("MissionManager").GetComponent<cshMissionManager>();
        cbm = GameObject.Find("BoilerManager").GetComponent<cshBoilerManager>();
        cpm = GameObject.Find("PanelManager").GetComponent<cshPanelManager>();
        cai = GameObject.Find("ARUser(Clone)").GetComponent<cshARInteraction>();
        cwm = GameObject.Find("WarningManager").GetComponent<cshWarningManager>();
        if (gameObject.transform.name.Equals("BoilerBtnManager"))
        {
            ManagerName = gameObject.transform.name;
            GameObject.Find("TextCamera").GetComponent<cshInceptionv3ImageClassifierApply>().ManagerName = ManagerName;
        }
        else
            ManagerName = null;


    }

    // Update is called once per frame
    void Update()
    {
        if (AnswerCheck)
        {
            if (gameObject.transform.name.Equals("BoilerBtnManager"))
            {
                Clear(ManagerName);
            }
            else
            {
                Clear(Split_ButtonName[0]);
            }
            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i].interactable = true;
                Buttons[i].colors = setColor(Buttons[i], new Color(200 / 255f, 200 / 255f, 200 / 255f, 128 / 255f));

            }
            AnswerCheck = false;
        }

    }

    public void SelectButton(int id)
    {
        ButtonId = id;
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = true;
            Buttons[i].colors = setColor(Buttons[i], new Color(200 / 255f, 200 / 255f, 200 / 255f, 128 / 255f));

        }

        Buttons[id - 1].interactable = false;
        Buttons[id - 1].colors = setColor(Buttons[id - 1], Color.red);
        ManagerName = gameObject.transform.name;
        GameObject.Find("TextCamera").GetComponent<cshInceptionv3ImageClassifierApply>().ManagerName = ManagerName;
        Split_ButtonName = Buttons[id - 1].ToString().Split(' ');
    }

    private ColorBlock setColor(Button b, Color c)
    {
        ColorBlock cb = b.colors;
        cb.disabledColor = c;
        return cb;
    }
    public void Warning(string ButtonName)
    {
        if (ButtonName.Equals("LampHalogenBtn"))
        {
            cwm.PanelWarningName = "LampWarningPanel";
        }
        else if (ButtonName.Equals("BoilerBtnManager"))
        {
            cwm.PanelWarningName = "BoilerWarningPanel";
        }
        else if (ButtonName.Equals("PlasticCookBtn"))
        {
            cwm.PanelWarningName = "CookWarningPanel";
        }
        else if (ButtonName.Equals("PaperCupBtn") || ButtonName.Equals("PlasticCupBtn"))
        {
            cwm.PanelWarningName = "CupWarningPanel";
        }
    }


    public void Clear(string ButtonName)
    {

        if (ButtonName.Equals("LampLEDBtn") || (ButtonName.Equals("BoilerBtnManager") && cbm.tmp == 24) || (ButtonName.Equals("WoodCookBtn")) || ButtonName.Equals("TumblerBtn"))
        {
            cpm.ARActive = false;
            PV.RPC("InteractionCountAdd", RpcTarget.All);


        }
        else
        {
            Warning(ButtonName);
        }


    }
    [PunRPC]
    public void InteractionCountAdd()
    {
        cmm = GameObject.Find("MissionManager").GetComponent<cshMissionManager>();
        cmm.interactionCount++;
        GameObject.Find("VRUser(Clone)").transform.Find("OVRCameraRig").Find("TrackingSpace").Find("CenterEyeAnchor").Find("VRCanvas").Find("ARWarningImage").gameObject.SetActive(false);

    }


}
