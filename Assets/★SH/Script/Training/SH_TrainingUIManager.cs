using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;

public class SH_TrainingUIManager : MonoBehaviourPun
{
    public static SH_TrainingUIManager instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject startUI;
    public GameObject tacticalBoard;
    public GameObject tool;
    public GameObject userList;

    public Button btnTacticalBoard;
    public Button btnPracticeStart;
    public Button btnTool;
    public Button btnUserList;
    public Button btnCoach;
    public Button btnPlayer;
    public Button btnStart;

    public TMP_Dropdown ddFormation;

    public List<string> optionList = new List<string>();

    public GameObject bluefactory;
    public GameObject redfactory;

    public Transform blueParent;
    public Transform redParent;

    bool isTBOpen = true;
    bool isToolOpen = true;
    bool isUserListOpen = true;

    SH_TrainingFSM trFSM;

    void Start()
    {
        // 드랍다운 옵션을 초기화 하고 싶다.
        ddFormation.ClearOptions();

        optionList = FormDataManager.instance.GetFormNames();
        ddFormation.AddOptions(optionList);

        ddFormation.onValueChanged.AddListener(OnValueChanged);
        btnTacticalBoard.onClick.AddListener(OnClickTBOpen);
        btnTool.onClick.AddListener(OnClickToolOpen);
        btnUserList.onClick.AddListener(OnClickUserList);
        btnPracticeStart.onClick.AddListener(OnClickPracticeStart);
        btnCoach.onClick.AddListener(OnClickCoach);
        btnPlayer.onClick.AddListener(OnClickPlayer);
        btnStart.onClick.AddListener(OnClickStart);

        blueParent = tacticalBoard.transform.GetChild(0).Find("BlueTeam").transform;
        redParent = tacticalBoard.transform.GetChild(0).Find("RedTeam").transform;

        trFSM = GetComponent<SH_TrainingFSM>();

        OnValueChanged(0);

        startUI.SetActive(true);
        tacticalBoard.SetActive(false);
        tool.SetActive(false);
        userList.SetActive(false);
    }

    void Update()
    {
        SlideMove(tacticalBoard, new Vector3(0, -1040, 0), isTBOpen);
        SlideMove(tool, new Vector3(150, 0, 0), isToolOpen);
        SlideMove(userList, new Vector3(-150, 0, 0), isUserListOpen);
    }

    Formation selected;
    public void OnValueChanged(int arg)
    {
        photonView.RPC(nameof(RPC_OnValueChanged), RpcTarget.All, arg);
    }
    [PunRPC]
    public void RPC_OnValueChanged(int arg)
    {
        Formation selected = FormDataManager.instance.GetForm(optionList[arg]);

        // 삭제하고
        for (int i = 0; i < blueParent.childCount; i++)
        {
            GameObject go = blueParent.GetChild(i).gameObject;

            go.GetComponent<SH_PieceWindow>().OnClickBtnDistDelete();
            go.GetComponent<SH_PieceWindow>().OnClickBtnArrowDelete();
            Destroy(go);
        }

        // 생성한다.
        for (int i = 0; i < selected.pos.Length; i++)
        {
            GameObject bluePiece = Instantiate(bluefactory, blueParent);
            bluePiece.name += i;
            bluePiece.transform.localPosition = selected.pos[i];

            FormationManager.instance.pieces[i] = bluePiece;
        }

        ddFormation.value = arg;
    }

    public void OnClickTBOpen()
    {
        photonView.RPC(nameof(RPC_OnClickTBOpen), RpcTarget.All);
    }
    [PunRPC]
    public void RPC_OnClickTBOpen()
    {
        isTBOpen = !isTBOpen;
        isToolOpen = isTBOpen;
        isUserListOpen = isTBOpen;

        if (isTBOpen)
            trFSM.instance.RPC_ChangeTime(SH_TrainingFSM.Time.EXPLANATION);
        else
            trFSM.instance.RPC_ChangeTime(SH_TrainingFSM.Time.PRACTICE);
    }

    public void OnClickToolOpen()
    {
        isToolOpen = !isToolOpen;
    }

    public void OnClickUserList()
    {
        isUserListOpen = !isUserListOpen;
    }

    public void OnClickPracticeStart()
    {
        trFSM.instance.ChangeTime(SH_TrainingFSM.Time.PRACTICE);
        OnClickTBOpen();
    }

    public bool isCoach;

    public void OnClickCoach()
    {
        isCoach = true;
    }

    public void OnClickPlayer()
    {
        isCoach = false;
    }

    public void OnClickStart()
    {
        startUI.SetActive(false);
        tacticalBoard.SetActive(true);
        tool.SetActive(true);
        userList.SetActive(true);

        //PgManager.instance.SettingSpawnOption();
        //PgManager.instance.CreateUser();
        PgManager.instance.MyStart();
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
