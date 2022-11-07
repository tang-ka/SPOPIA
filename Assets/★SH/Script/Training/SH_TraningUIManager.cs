using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SH_TraningUIManager : MonoBehaviour
{
    public Button btnTacticalBoard;
    public Button btnTool;
    public TMP_Dropdown ddFormation;

    public List<string> optionList = new List<string>();
        

    bool isTBOpen = true;
    bool isToolOpen = true;
    
    

    void Start()
    {
        // 드랍다운 옵션을 초기화 하고 싶다.
        ddFormation.ClearOptions();

        optionList = DataManager.instance.GetFormaitonNames();
        ddFormation.AddOptions(optionList);

        ddFormation.onValueChanged.AddListener(onValueChanged);

        btnTacticalBoard.onClick.AddListener(OnClickTBOpen);
        btnTool.onClick.AddListener(OnClickToolOpen);
    }

    void Update()
    {
        SlideMove(btnTacticalBoard, new Vector3(0, -1040, 0), isTBOpen);
        SlideMove(btnTool, new Vector3(150, 0, 0), isToolOpen);
    }

    private void onValueChanged(int arg)
    {
        print(optionList[arg]);
    }

    public void OnClickTBOpen()
    {
        isTBOpen = !isTBOpen;
        isToolOpen = isTBOpen;
    }

    public void OnClickToolOpen()
    {
        isToolOpen = !isToolOpen;
        isTBOpen = isToolOpen;
    }

    public void SlideMove(Button btn, Vector3 onPos, bool isOn)
    {
        Vector3 myPos = btn.GetComponent<RectTransform>().anchoredPosition;
        Vector3 offPos = Vector3.zero;

        // 켜고싶다.
        if (isOn)
        {
            myPos = Vector3.Lerp(myPos, onPos, Time.deltaTime * 10);
            if ((myPos - onPos).magnitude < 0.05f)
                myPos = onPos;
        }
        // 끄고 싶다.
        else
        {
            myPos = Vector3.Lerp(myPos, offPos, Time.deltaTime * 10);

            if ((myPos - offPos).magnitude < 0.05f)
                myPos = offPos;
        }

        btn.GetComponent<RectTransform>().anchoredPosition = myPos;
    }
}
