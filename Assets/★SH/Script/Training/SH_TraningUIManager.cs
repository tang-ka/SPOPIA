using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SH_TraningUIManager : MonoBehaviour
{
    public GameObject tacticalBoard;
    public GameObject tool;

    public Button btnTacticalBoard;
    public Button btnTool;
    public TMP_Dropdown ddFormation;

    public List<string> optionList = new List<string>();

    public GameObject bluefactory;
    public GameObject redfactory;

    Transform blueParent;
    Transform redParent;

    bool isTBOpen = true;
    bool isToolOpen = true;

    void Start()
    {
        // 드랍다운 옵션을 초기화 하고 싶다.
        ddFormation.ClearOptions();

        optionList = FormDataManager.instance.GetFormNames();
        ddFormation.AddOptions(optionList);

        ddFormation.onValueChanged.AddListener(onValueChanged);
        btnTacticalBoard.onClick.AddListener(OnClickTBOpen);
        btnTool.onClick.AddListener(OnClickToolOpen);

        blueParent = tacticalBoard.transform.GetChild(0).Find("BlueTeam").transform;
        redParent = tacticalBoard.transform.GetChild(0).Find("RedTeam").transform;

        onValueChanged(0);
    }

    void Update()
    {
        SlideMove(tacticalBoard, new Vector3(0, -1040, 0), isTBOpen);
        SlideMove(tool, new Vector3(150, 0, 0), isToolOpen);
    }

    private void onValueChanged(int arg)
    {
        Formation selected = FormDataManager.instance.GetForm(optionList[arg]);

        // 삭제하고
        for (int i = 0; i < blueParent.childCount; i++)
        {
            Destroy(blueParent.GetChild(i).gameObject);
        }

        // 생성한다.
        for (int i = 0; i < selected.pos.Length; i++)
        {
            GameObject bluePiece = Instantiate(bluefactory, blueParent);
            bluePiece.transform.localPosition = selected.pos[i];
        }
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

    public void SlideMove(GameObject go, Vector3 onPos, bool isOn)
    {
        Vector3 myPos = go.GetComponent<RectTransform>().anchoredPosition;
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

        go.GetComponent<RectTransform>().anchoredPosition = myPos;
    }
}
