using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class cshVRTeleport : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    private Transform vrPos;
    public bool isM = false;

    public GameObject kitchenImage;
    public GameObject livingImage;

    private int cnt = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("MissionManager").GetComponent<cshMissionManager>().teleportAccess == true && GameObject.Find("MissionManager").GetComponent<cshMissionManager>().interactionCount == 5 && cnt == 0)
        {
            StartCoroutine("livingImageOn");
        }
        else if (GameObject.Find("MissionManager").GetComponent<cshMissionManager>().teleportAccess == true && GameObject.Find("MissionManager").GetComponent<cshMissionManager>().interactionCount == 10 && cnt == 1)
        {
            StartCoroutine("kitchenImageOn");
        }

        if (isM)
        {
            if (GameObject.Find("MissionManager").GetComponent<cshMissionManager>().teleportAccess == true)
            {
                if (GameObject.Find("MissionManager").GetComponent<cshMissionManager>().interactionCount == 5)
                {
                    vrPos = GameObject.Find("VR_Pos_Kitchen").transform;
                    Teleport();
                    GameObject.Find("MissionManager").GetComponent<cshMissionManager>().teleportAccess = false;
                }

                else if (GameObject.Find("MissionManager").GetComponent<cshMissionManager>().interactionCount == 10)
                {
                    vrPos = GameObject.Find("VR_Pos_FrontDoor").transform;

                    Teleport();
                    GameObject.Find("MissionManager").GetComponent<cshMissionManager>().teleportAccess = false;
                }
            }
            isM = false;
        }
    }

    IEnumerator livingImageOn()
    {
        livingImage.SetActive(true);
        cnt++;
        Invoke("livingImageOff", 2.5f);

        yield return new WaitForSeconds(.5f);
    }

    void livingImageOff()
    {
        livingImage.SetActive(false);
    }
    IEnumerator kitchenImageOn()
    {
        kitchenImage.SetActive(true);
        cnt++;
        Invoke("kitchenImageOff", 2.5f);

        yield return new WaitForSeconds(.5f);
    }

    void kitchenImageOff()
    {
        kitchenImage.SetActive(false);
    }


    private void Teleport()
    {
        gameObject.GetComponent<OVRPlayerController>().enabled = false;
        gameObject.transform.position = vrPos.transform.position;
        gameObject.GetComponent<OVRPlayerController>().enabled = true;
    }
}