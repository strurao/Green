using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshVRCancel : MonoBehaviour
{
    public string selectedName = null;
    public bool isB = false;

    // Start is called before the first frame update
    void Start()
    {
        selectedName = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (isB && !(gameObject.GetComponent<cshVRInfo>().imageOn) && (selectedName != null))
        {
            //Debug.Log("isB " + isB);
            //Debug.Log("!imageOn " + !(gameObject.GetComponent<cshVRInfo>().imageOn));
            //Debug.Log("selectedNameNull " + (selectedName != null));
            //Debug.Log(isB && !(gameObject.GetComponent<cshVRInfo>().imageOn) && selectedName != null);
            GameObject.Find(selectedName).tag = selectedName;
            GameObject.Find(selectedName).GetComponent<cshSelectedTrash>().isSelected = false;
            gameObject.GetComponent<cshVRSelect>().isSelect = false;
            gameObject.GetComponent<cshVRSelect>().trashName = null;

            GameObject.Find("WasteTrashCan").tag = "Untagged";
            GameObject.Find("PaperTrashCan").tag = "Untagged";
            GameObject.Find("PlasticTrashCan").tag = "Untagged";

            selectedName = null;
            isB = false;
        } else if (isB)
        {
            isB = false;
            gameObject.GetComponent<cshVRInfo>().isB = true;
        }
    }
}
