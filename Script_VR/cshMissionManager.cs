using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class cshMissionManager : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public GameObject cups;
    public GameObject bag;
    public bool teleportAccess;
    public int interactionCount = 0;
    public int trashCount = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (interactionCount == 4 && cups.activeSelf == false)
        {
            PV.RPC("CupActive", RpcTarget.All, true);
        }
        else if (interactionCount != 4 && cups.activeSelf == true)
        {
            PV.RPC("CupActive", RpcTarget.All, false);
        }

        if (interactionCount == 9 && bag.activeSelf == false)
        {
            PV.RPC("BagActive", RpcTarget.All, true);
        }
        else if (interactionCount != 9 && bag.activeSelf == true)
        {
            PV.RPC("BagActive", RpcTarget.All, false);
        }

        if (trashCount == 3)
        {
            PV.RPC("interAdd", RpcTarget.All);
        }
    }

    [PunRPC]

    private void CupActive(bool value)
    {
        cups.gameObject.SetActive(value);
    }

    [PunRPC]

    private void BagActive(bool value)
    {
        bag.gameObject.SetActive(value);
    }

    [PunRPC]

    private void interAdd()
    {
        interactionCount++;
        trashCount = 0;
    }
}
