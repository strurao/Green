using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using TMPro;

public class cshARInteraction : MonoBehaviourPunCallbacks
{
    cshMissionManager cmm;

    public GameObject effect;
    public PhotonView PV;
    public Image RequestImg;

    public Image LivingRoomMissionGaze; // 미션 클리어마다 올라갈 게이지
    public Image KitchenMissionGaze;
    public TextMeshProUGUI SavingPointTxt;
    public TextMeshProUGUI EcoPointTxt;
    public TextMeshProUGUI SelectedModeTxt;

    AudioSource audioSource;

    public bool ClickOn = false;
    public bool HelpRequest = false;
    private float MaxGaze;
    private float GazeDelta;
    //public float LivingRoomMissionClearCount;
    // public float KitchenMissionClearCount;
    public int MissionCount;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        cmm = GameObject.Find("MissionManager").GetComponent<cshMissionManager>();
        RequestImg.gameObject.SetActive(false);
        SelectedModeTxt.gameObject.SetActive(false);
        MaxGaze = 100;
        GazeDelta = 20;
        // LivingRoomMissionClearCount = 0;
        // KitchenMissionClearCount = 0;
        LivingRoomMissionGaze.fillAmount = 0;
        KitchenMissionGaze.fillAmount = 0;
        SavingPointTxt.text = "0%";
        EcoPointTxt.text = "0%";

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Clicked();
        }
        MissionCount = cmm.interactionCount;
  
        
        LivingRoomMissionGaze.fillAmount = (MissionCount * GazeDelta) / MaxGaze;

        if (MissionCount < 5)
        {
            SavingPointTxt.text = (20 * MissionCount).ToString() + "%";
        }
        else
        {
            SavingPointTxt.text = "100%";
        }
        KitchenMissionGaze.fillAmount = ((MissionCount-5) * GazeDelta) / MaxGaze;

        if (MissionCount < 5)
        {
            EcoPointTxt.text = "0%";
        }
        else
        {
            EcoPointTxt.text = (20 * (MissionCount - 5)).ToString() + "%";
        }
        

        if (HelpRequest)
        {
            StartCoroutine("HelpRequestOn");
            HelpRequest = false;
        }
    }
    private void Clicked()
    {
        RaycastHit hit;
        Vector3 pos = new Vector3();
        GameObject target = null;
        string str1 = null;
        int hitcount = 0;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (true == (Physics.Raycast(ray.origin, ray.direction * 1000, out hit)) && hitcount == 0)
        {
            target = hit.collider.gameObject;
            pos = target.transform.position;
            str1 = target.tag.ToString();
            Debug.Log(str1);
            hitcount++;
        }
        if (ClickOn == true)
        {
            PV.RPC("ChoiceInteraction", RpcTarget.All, str1, pos);
            ClickOn = false;
            SelectedModeTxt.gameObject.SetActive(false);
        }
    }
    [PunRPC]
    private void ChoiceInteraction(string str, Vector3 vec)
    {
        Debug.Log("choics 진입" + str);
        if (str.Equals("Lamp") || str.Equals("TV") || str.Equals("Boiler") || str.Equals("Spray") || str.Equals("Detergent") || str.Equals("PizzaBox") || str.Equals("KitchenTap") || str.Equals("Lighter" )|| str.Equals("PetBottle") || str.Equals("Bag") || str.Equals("Bicycle") )
        {
            Debug.Log(str);
            GameObject gb = Instantiate(effect, vec + new Vector3(0, 1, 0), Quaternion.identity);
            Destroy(gb, 3.0f);
        }
        Debug.Log(audioSource.name);
        audioSource.Play();
        SelectedModeTxt.gameObject.SetActive(false);
    }
    IEnumerator HelpRequestOn()
    {
        RequestImg.gameObject.SetActive(true);
        Invoke("Wait3Sec", 3f);

        yield return new WaitForSeconds(.5f);
    }

    void Wait3Sec()
    {
        RequestImg.gameObject.SetActive(false);
    }
}
