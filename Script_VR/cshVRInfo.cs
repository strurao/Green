using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cshVRInfo : MonoBehaviour
{
    public GameObject infoImage;

    public bool isB = false;
    public bool isD = false;
    public bool imageOn = true;

    private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        imageOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isB)
        {
            if (imageOn)
            {
                infoImage.SetActive(false);
                imageOn = false;
            }
            isB = false;
        }

        if(isD)
        {
            if(!imageOn)
            {
                infoImage.gameObject.SetActive(true);
                imageOn = true;
            }
            else
            {
                infoImage.transform.GetChild(index).gameObject.SetActive(false);
                index++;
                index %= infoImage.transform.GetChildCount();
                infoImage.transform.GetChild(index).gameObject.SetActive(true);
            }
            isD = false;
        }
    }
}