using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class cshVRCommunication : MonoBehaviourPunCallbacks
{
    public PhotonView PV;
    public bool isC = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isC)
        {
            PV.RPC("HintRequest", RpcTarget.All);
            isC = false;
        }
    }
    
    [PunRPC]
    
    private void HintRequest()
    {
        cshARInteraction arInteraction = GameObject.Find("ARUser(Clone)").GetComponent<cshARInteraction>();
        arInteraction.HelpRequest = true;
    }
}