using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cshNBButton : MonoBehaviour
{
    public GameObject[] text = new GameObject[11];
    int textSize = 11;
    public static int num;
    public GameObject Button;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < textSize; i++)
        {
            if (i == 0)
                text[i].SetActive(true);
            else
                text[i].SetActive(false);
        }
        num = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void ButtonClick()
    {
        if (Button.name == "NextButton")
        {
            text[num].SetActive(false);
            num++;
            num = num % textSize;
            text[num].SetActive(true);
        }
        if (Button.name == "BeforeButton")
        {
            text[num].SetActive(false);
            if (num == 0)
                num = num + 4;
            else
                num--;
            num = num % textSize;
            text[num].SetActive(true);
        }


    }


}
