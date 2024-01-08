using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshWarningManager : MonoBehaviour
{
    public GameObject[] PanelWarning;
    public string PanelWarningName;
    int num;

    // Start is called before the first frame update
    void Start()
    {
        PanelWarningName = null;
        num = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PanelWarningName != null)

        {
            Debug.Log("PanelWarning 진입!!");
            for (int i = 0; i < PanelWarning.Length; i++)
            {
                if (PanelWarning[i].name.Equals(PanelWarningName))
                {
                    PanelWarning[i].SetActive(true);
                    num = i;
                }

            }
        }
        else
            PanelWarning[num].SetActive(false);
    }
    public void WarningUIOk()
    {
        PanelWarningName = null;
    }
}
