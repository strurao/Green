using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshTreeController : MonoBehaviour
{
    cshMissionManager cmm;
    // Start is called before the first frame update
    void Start()
    {
        cmm = GameObject.Find("MissionManager").GetComponent<cshMissionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cmm.interactionCount == 2)
        {
            for (int i = 0; i < 4; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else if (cmm.interactionCount == 4)
        {
            for (int i = 4; i < 9; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else if (cmm.interactionCount == 7)
        {
            for (int i = 9; i < 14; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else if (cmm.interactionCount == 9)
        {
            for (int i = 14; i < 21; i++)
            {
               transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
