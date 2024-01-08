using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class cshARPlayer : MonoBehaviourPun
{
    private GameObject target;
    private Camera ARCamera;
    private int state = 0; //0:거실 시작, 1:거실과 부엌 함께, 2: 부엌, 3:전체신
    cshMissionManager cmm;

    public void ClearText(bool capture)
    {
        /*
        if (capture)
            GetComponent<cshCaptureCamera>().CaptureText();
            */
        GameObject[] delete = GameObject.FindGameObjectsWithTag("LineDraw");
        int deleteCount = delete.Length;
        for (int i = deleteCount - 1; i >= 0; i--)
            Destroy(delete[i]);
        //PhotonNetwork.Destroy(delete[i]);

    }

    // Start is called before the first frame update
    void Start()
    {
        // 네트워크 환경에서 현재 제어중인 사용자(AR/VR)인지 유무 판단
        if (!photonView.IsMine)
        {

            Camera[] cameras;
            cameras = transform.gameObject.GetComponentsInChildren<Camera>();
            foreach (Camera c in cameras)
            {
                c.enabled = false;
            }
        }
        else
        {

            target = GameObject.Find("TargetCube");
            ARCamera = GameObject.Find("ARCamera").GetComponent<Camera>();

            //btnGen.interactable = true;
        }
        cmm = GameObject.Find("MissionManager").GetComponent<cshMissionManager>();
    }



    // Update is called once per frame
    void Update()
    {

        //if (cmm.interactionCount == 5 && cmm.teleportAccess == true)
        //{
        //    state = 1;
        //}
        //else if (cmm.interactionCount == 5 && cmm.teleportAccess == false)
        //{
        //    state = 2;
        //}
        //else if (cmm.interactionCount == 10)
        //{
        //    state = 3;
        //}

        if (cmm.interactionCount < 5)
        {
            state = 0;
        }
        else if (cmm.interactionCount >= 5 && GameObject.Find("VRUser(Clone)").transform.position == GameObject.Find("VR_Pos_Kitchen").transform.position)
        {
            state = 2;
        }
        else if (cmm.interactionCount == 5)
        {
            state = 1;
        }
        else if (cmm.interactionCount == 10 && GameObject.Find("VRUser(Clone)").transform.position == GameObject.Find("VR_Pos_FrontDoor").transform.position)
        {
            state = 3;
        }

        if (!photonView.IsMine)
        {
            transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            return;
        }
        else
        {
            // 이미지 타깃이 인식이 되면, 배경 객체를 화면에 그리고
            // 그렇지 않다면 그리지 않게 만드는 과정
            // ARObject/ControlTile/Player 레이어를 AR카메라의 Culling Mask와 연동하여 설정
            // cullingMask에 state에 맞는 Layer 추가
            if (target.GetComponent<Renderer>().enabled)
            {
                switch (state)
                {
                    case 0:
                        ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("SceneLivingBed");
                        ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("VRUser");
                        //        ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("Ground");



                        ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("SceneKitchen"));
                        ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("SceneBackground"));
                        break;
                    case 1:
                        ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("SceneLivingBed");
                        ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("SceneKitchen");
                        ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("TrashCan");
                        ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("VRUser");
                        //       ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("Ground");

                        ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("SceneBackground"));
                        break;
                    case 2:
                        ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("SceneKitchen");
                        ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("VRUser");
                        ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("TrashCan");
                        //        ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("Ground");

                        ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("SceneLivingBed"));
                        ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("SceneBackground"));
                        break;
                    case 3:
                        ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("SceneLivingBed");
                        ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("SceneKitchen");
                        ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("SceneBackground");
                        ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("VRUser");
                        ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("TrashCan");
                        ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("Bicycle");

                        break;
                    default:
                        break;
                }
            }
            else
            {
                // cullingMask에 Layer 제거
                ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("SceneLivingBed"));
                ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("SceneKitchen"));
                ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("SceneBackground"));
                ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("VRUser"));
                ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("TrashCan"));
                ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Bicycle"));
            }

            /*
            // cullingMask에 Layer 추가
            if (target.GetComponent<Renderer>().enabled)
            {
                ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("ARObject");
                ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("ControlTile");
                ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("Player");
                ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("HMD_Player");
                ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("Portal");
                ARCamera.cullingMask |= 1 << LayerMask.NameToLayer("Monster");
            }
            // cullingMask에 Layer 제거
            else
            {
                ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("ARObject"));
                ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("ControlTile"));
                ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));
                ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("HMD_Player"));
                ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Portal"));
                ARCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Monster"));
            }*/
        }
    }
}
