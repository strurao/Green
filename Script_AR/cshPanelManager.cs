using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
public class cshPanelManager : MonoBehaviourPunCallbacks
{
    public GameObject[] panel;
    public string ChoiceObject; // VR에서 스크립트 참조로 가져올값
    public bool ARActive; //VR에서 스크립트 참조로 가져올 값

    void Start()
    {
        for (int i = 0; i < panel.Length; i++)
        {
            panel[i].SetActive(false);
            panel[i].gameObject.SetActive(false);

        }
        ARActive = false;
        //ChoiceObject = "BoilerPanel";
        ChoiceObject = null;

    }

    // Update is called once per frame
    void Update()
    {
        if (ChoiceObject == null)
            return;
       // Debug.Log(ChoiceObject);

        if (ARActive)
           {
                for (int i = 0; i < panel.Length; i++)
                {
                    if (ChoiceObject.Equals(panel[i].name))
                    {
                        panel[i].SetActive(true);
                    }
                }
            }
            else
            {
                for (int i = 0; i < panel.Length; i++)
                {
                    if (ChoiceObject.Equals(panel[i].name))
                    {
                        panel[i].SetActive(false);
                    }
                }
            ChoiceObject = null;
            }
     }
}



