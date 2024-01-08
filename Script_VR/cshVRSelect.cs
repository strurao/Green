using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

public class cshVRSelect : MonoBehaviourPunCallbacks
{
    public Image ProgressBar;
    public LineRenderer rayLine;
    public Transform RHandRayPos;
    public PhotonView PV;

    public GameObject arWarningImage;
    public GameObject trashWarningImage;
    public GameObject detergentImage;
    public GameObject cupImage;
    public GameObject endImage;

    private bool isSelectMode;
    public string trashName = null;
    public bool isSelect;
    public bool isS = false;

    private float barTime = 0.0f;
    private const float selectTime = 3.0f;

    void Start()
    {
        rayLine.enabled = false;
        isSelectMode = false;
        isSelect = false;
    }

    // Update is called once per frame
    void Update()
    {

        // 키보드 입력으로 일단 만들어 보기
        // 선택(S) 물건 선택
        if (isS)
        {
            if (!isSelectMode)
                isSelectMode = true;
            else
            {
                isSelectMode = false;
                rayLine.enabled = false;
            }
            isS = false;
        }

        if (isSelectMode)
        {
            Debug.Log("S(선택) : VR Select for Hand RayCast");

            RaycastHit hit;
            int layerMask = 1;  //<< LayerMask.NameToLayer("SceneLivingBed");  // 거실에 있는 물건들만 충돌

            if (GameObject.Find("MissionManager").GetComponent<cshMissionManager>().interactionCount < 5)
            {
                layerMask = 1 << LayerMask.NameToLayer("SceneLivingBed");
            }
            else if (isSelect)
            {
                layerMask = 1 << LayerMask.NameToLayer("TrashCan");
            }
            else if (GameObject.Find("MissionManager").GetComponent<cshMissionManager>().interactionCount >= 5 && GameObject.Find("MissionManager").GetComponent<cshMissionManager>().interactionCount < 10)
            {
                layerMask = 1 << LayerMask.NameToLayer("SceneKitchen");
            }
            else
            {
                layerMask = 1 << LayerMask.NameToLayer("Bicycle");
            }

            rayLine.SetPosition(0, RHandRayPos.position);                           //손에서 나가는 레이를 시각화
            rayLine.SetPosition(1, RHandRayPos.position + (-RHandRayPos.right));
            rayLine.enabled = true;
            rayLine.SetColors(Color.white, Color.white);

            if (Physics.Raycast(RHandRayPos.position, -RHandRayPos.right, out hit, Mathf.Infinity, layerMask))
            {
                //거실 중 실제 선택해야할 물건들일 가르킬 때만 시각화 했던 레이의 색을 파란색으로 변경

                // 상호작용할 물건을 제대로 가르킬 때
                //(hit.transform.tag == "TV" || hit.transform.tag == "Spray" || hit.transform.tag == "Lamp" || hit.transform.tag == "Boiler" || hit.transform.tag == "RightCup")



                if (barTime <= selectTime && hit.transform.tag != "Untagged")
                {
                    barTime += Time.deltaTime;
                    rayLine.SetColors(Color.blue, Color.blue);

                    Debug.DrawRay(RHandRayPos.position, -RHandRayPos.right * hit.distance, Color.blue);

                    Debug.Log(hit.transform.tag);
                }
                else // 상호작용할 물건이 아닌 것을 가르킬 때
                {
                    barTime = 0.0f;
                    ProgressBar.fillAmount = 0;

                    Debug.DrawRay(RHandRayPos.position, -RHandRayPos.right * 1000, Color.red);
                    rayLine.SetColors(Color.white, Color.white);
                }

                ProgressBar.fillAmount = barTime / selectTime;

                if (ProgressBar.fillAmount >= 1.0)  // 선택했을 때
                {
                    PV.RPC("Interaction", RpcTarget.All, hit.transform.tag);
                }
            }
        }

    }

    IEnumerator trashImageOn()
    {
        trashWarningImage.SetActive(true);
        Invoke("trashImageOff", 2.5f);

        yield return new WaitForSeconds(.5f);
    }

    void trashImageOff()
    {
        trashWarningImage.SetActive(false);
    }
    IEnumerator detergentImageOn()
    {
        detergentImage.SetActive(true);
        Invoke("detergentImageOff", 2.5f);

        yield return new WaitForSeconds(.5f);
    }

    void detergentImageOff()
    {
        detergentImage.SetActive(false);
    }
    IEnumerator cupImageOn()
    {
        cupImage.SetActive(true);
        Invoke("cupImageOff", 2.5f);

        yield return new WaitForSeconds(.5f);
    }

    void cupImageOff()
    {
        cupImage.SetActive(false);
    }
    [PunRPC]
    private void Interaction(string tag)
    {
        cshPanelManager play = GameObject.Find("PanelManager").GetComponent<cshPanelManager>(); //ar유저에게 신호를 주기 위해 스크립트 접근
        cshMissionManager cmm = GameObject.Find("MissionManager").GetComponent<cshMissionManager>();

        isSelectMode = false;       // 선택모드 종료
        rayLine.enabled = false;    // 레이시각화 종료
        barTime = 0.0f;
        ProgressBar.fillAmount = 0;

        GameObject SelObject = GameObject.Find(tag);

        Debug.Log(tag + " is selected");

        if (tag == "TV")            // TV 선택 : TV화면 종료
        {
            Destroy(SelObject);
            cmm.interactionCount++;
            //상호작용을 마칠때 마다 하나씩 셈, 모든 수행을 끝내면 다음 방으로 넘어갈 준비
        }
        else if (tag == "Spray")    // 스프레이 선택 : 
        {
            SelObject.tag = "Untagged"; // 태그 제거(선택 불가능한 상태로 만들기 위해)

            SelObject.GetComponent<Animator>().Play("up");
            SelObject.transform.Find("SprayWater").GetComponent<ParticleSystem>().Play();
            cmm.interactionCount++;
        }
        else if (tag == "Lamp")     // 램프 선택 : 
        {
            play.ARActive = true;
            play.ChoiceObject = "LampPanel";
            arWarningImage.SetActive(true);
            SelObject.tag = "Untagged";
        }
        else if (tag == "Boiler")   // 보일러 선택 : 
        {
            play.ARActive = true;
            play.ChoiceObject = "BoilerPanel";           // AR유저에게 신호
            arWarningImage.SetActive(true);
            SelObject.tag = "Untagged";
        }
        else if (tag == "WrongCup")
        {
            StartCoroutine("cupImageOn");
        }
        else if (tag == "RightCup")
        {
            cmm.interactionCount = 5;
        }
        else if (!isSelect && (tag == "PizzaBox" || tag == "PetBottle" || tag == "Lighter"))
        {
            trashName = tag;
            SelObject.GetComponent<cshSelectedTrash>().isSelected = true;
            GameObject.Find("VRUser(Clone)").GetComponent<cshVRCancel>().selectedName = tag;
            GameObject.Find("WasteTrashCan").tag = "WasteTrashCan";
            GameObject.Find("PaperTrashCan").tag = "PaperTrashCan";
            GameObject.Find("PlasticTrashCan").tag = "PlasticTrashCan";

            isSelect = true;
        }
        else if (tag == "KitchenTap")
        {
            SelObject.tag = "Untagged";
            GameObject.Find("KitchenTapWater").GetComponent<ParticleSystem>().Stop();
            cmm.interactionCount++;
        }
        else if (tag == "Detergent")
        {

            Destroy(GameObject.Find("washingScreen2"));

            SelObject.tag = "Untagged";
            SelObject.GetComponent<Animator>().Play("upDetergent");
            SelObject.transform.Find("Petals Prefab 2 (1)").GetComponent<ParticleSystem>().Play();
            SelObject.transform.Find("Petals Prefab 2 (2)").GetComponent<ParticleSystem>().Play();
            SelObject.transform.Find("Petals Prefab 2 (3)").GetComponent<ParticleSystem>().Play();
            cmm.interactionCount++;
        }
        else if (tag == "WrongDetergent")
        {
            StartCoroutine("detergentImageOn");
        }
        else if (tag == "KitchenUtensils")
        {
            play.ARActive = true;
            play.ChoiceObject = "CookPanel";           // AR유저에게 신호
            arWarningImage.SetActive(true);
            SelObject.tag = "Untagged";
        }
        else if (tag == "WasteTrashCan")
        {

            if (trashName == "Lighter")
            {
                isSelect = false;
                GameObject.Find(trashName).SetActive(false);
                trashName = null;
                cmm.trashCount++;
            }
            else
            {
                StartCoroutine("trashImageOn");
            }
        }
        else if (tag == "PaperTrashCan")
        {
            if (trashName == "PizzaBox")
            {
                isSelect = false;
                GameObject.Find(trashName).SetActive(false);
                trashName = null;
                cmm.trashCount++;
            }
            else
            {
                StartCoroutine("trashImageOn");
            }
        }
        else if (tag == "PlasticTrashCan")
        {
            if (trashName == "PetBottle")
            {
                isSelect = false;
                GameObject.Find(trashName).SetActive(false);
                trashName = null;
                cmm.trashCount++;
            }
            else
            {
                StartCoroutine("trashImageOn");
            }
        }
        else if (tag == "Bag")
        {
            cmm.interactionCount = 10;
        }
        else if (tag == "Bicycle")
        {
            endImage.SetActive(true);
        }
    }
}