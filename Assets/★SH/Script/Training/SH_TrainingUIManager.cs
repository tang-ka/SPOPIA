using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SH_TrainingUIManager : MonoBehaviour
{
    public GameObject startUI;
    public GameObject tacticalBoard;
    public GameObject tool;
    public GameObject playerList;

    public Button btnTacticalBoard;
    public Button btnTool;
    public Button btnPracticeStart;
    public Button btnCoach;
    public Button btnPlayer;

    public TMP_Dropdown ddFormation;

    public List<string> optionList = new List<string>();

    public GameObject bluefactory;
    public GameObject redfactory;

    Transform blueParent;
    Transform redParent;

    bool isTBOpen = true;
    bool isToolOpen = true;

    SH_TrainingFSM trFSM;

    void Start()
    {
        // 드랍다운 옵션을 초기화 하고 싶다.
        ddFormation.ClearOptions();

        optionList = FormDataManager.instance.GetFormNames();
        ddFormation.AddOptions(optionList);

        ddFormation.onValueChanged.AddListener(onValueChanged);
        btnTacticalBoard.onClick.AddListener(OnClickTBOpen);
        btnTool.onClick.AddListener(OnClickToolOpen);
        btnPracticeStart.onClick.AddListener(OnClickPracticeStart);
        btnCoach.onClick.AddListener(OnClickCoach);
        btnPlayer.onClick.AddListener(OnClickPlayer);

        blueParent = tacticalBoard.transform.GetChild(0).Find("BlueTeam").transform;
        redParent = tacticalBoard.transform.GetChild(0).Find("RedTeam").transform;

        trFSM = GetComponent<SH_TrainingFSM>();

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

            FormationManager.instance.pieces[i] = bluePiece;
        }
    }

    public void OnClickTBOpen()
    {
        isTBOpen = !isTBOpen;
        isToolOpen = isTBOpen;

        if (isTBOpen)
            trFSM.instance.ChangeTime(SH_TrainingFSM.Time.EXPLANATION);
        else
            trFSM.instance.ChangeTime(SH_TrainingFSM.Time.PRACTICE);
    }

    public void OnClickToolOpen()
    {
        isToolOpen = !isToolOpen;
        isTBOpen = isToolOpen;
    }

    public void OnClickPracticeStart()
    {
        trFSM.instance.ChangeTime(SH_TrainingFSM.Time.PRACTICE);
        OnClickTBOpen();
    }

    public void OnClickCoach()
    {
        // 플레이어에게 코치권한을 부여하고 싶다.


        // 훈련을 시작하고 싶다.
        StartTraining();
    }

    public void OnClickPlayer()
    {
        // 플레이어에게 플레이어 권한을 부여하고 싶다.


        // 훈련을 시작하고 싶다.
        StartTraining();
    }

    public void StartTraining()
    {
        startUI.SetActive(false);
        tacticalBoard.SetActive(true);
        tool.SetActive(true);
        playerList.SetActive(true);
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
