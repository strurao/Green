using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using TMPro;
public class cshBoilerManager : MonoBehaviour
{ 
    public TextMeshProUGUI txt; // 보일러 온도 텍스트 메시 프로 유아이 
    public int tmp; // 보일러온도 값 관리
    // Start is called before the first frame update
    void Start()
    {
        tmp = int.Parse(txt.text);

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void BoilerUpBtnClick()
    {
        tmp++;
        txt.text = tmp.ToString();
    }
    public void BoilerDownBtnClick()
    {
        if (tmp == 0)
            return;
        else
        {
            tmp--;
            txt.text = tmp.ToString();
        }

    }
}
