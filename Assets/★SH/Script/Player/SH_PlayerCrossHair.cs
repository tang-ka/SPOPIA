using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using JetBrains.Annotations;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SH_PlayerCrossHair : MonoBehaviourPunCallbacks
{
    public Transform cam;

    public GameObject dataInputTable;

    public GameObject screenViewCanvas;

    public GameObject guideMessage;
    public GameObject dataInputCenterMsg;

    public Text nickName;

    SH_PlayerFSM fsm;

    Canvas m_canvas;
    GraphicRaycaster m_gr;
    PointerEventData m_ped;
    List<RaycastResult> results;

    void Start()
    {
        if (photonView.IsMine)
        {
            screenViewCanvas.SetActive(true);
            dataInputTable = GameObject.Find("InputMatchData");
            nickName.text = PhotonNetwork.NickName;
        }

        //screenViewCanvas.SetActive(true);
        if (dataInputTable != null)
            dataInputTable.SetActive(false);


        fsm = GetComponent<SH_PlayerFSM>();

        m_ped = new PointerEventData(null);
    }

    MeshRenderer sphere;

    // Photon 동기화 필요 없음
    void Update()
    {
        if (photonView.IsMine == false) return;
        // 마우스 탈출 (영수)
        if (guideMessage.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (fsm.state == SH_PlayerFSM.State.NORMAL)
                {
                    fsm.ChangeState(SH_PlayerFSM.State.UIPLAYING);
                }
                else
                {
                    fsm.ChangeState(SH_PlayerFSM.State.NORMAL);
                }
            }
        }

        // 치트키
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GetComponent<CharacterController>().enabled = false;
            transform.position = new Vector3(130, 2, 215);
            GetComponent<CharacterController>().enabled = true;
        }

        int layerMask = (-1) - (1 << LayerMask.NameToLayer("IgnoreRay"));

        // 크로스 헤어에 레이 쏘기
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction.normalized * 20, Color.red);

        if (Physics.Raycast(ray, out hit, 20, layerMask))
        {
            if (hit.transform.gameObject.name == "MatchDataSphere")
            {
                hit.transform.GetComponent<MeshRenderer>().enabled = true;
                sphere = hit.transform.GetComponent<MeshRenderer>();

                if (Input.GetMouseButtonDown(0))
                {
                    dataInputTable.SetActive(true);
                    dataInputCenterMsg.SetActive(false);
                    fsm.ChangeState(SH_PlayerFSM.State.UIPLAYING);
                }
            }
            else if (hit.transform.gameObject.name == "EnterBG")
            {
                if (Input.GetMouseButtonDown(1))
                {
                    if (SceneManager.GetActiveScene().name == "LeagueAreaScene")
                    {
                        string curRoom = PhotonNetwork.CurrentRoom.Name;
                        string nextRoom = "PlayGround"; // DBManager.instance.myData.teamName;
                        string preLobby = "1"; //PhotonNetwork.CurrentLobby.Name;
                        LaManager.instance.EnterPlaygroundScene(curRoom, nextRoom, preLobby);
                    }
                    else if (SceneManager.GetActiveScene().name == "PlayGroundScene")
                    {
                        string curRoom = PhotonNetwork.CurrentRoom.Name;
                        string nextRoom = DBManager.instance.leagueInfo.leagueName;
                        PgManager.instance.EnterScene(curRoom, nextRoom);
                    }
                }
            }
            else
            {
                if (sphere == null) return;
                sphere.enabled = false;
            }
        }
        else
        {
            if (sphere == null) return;
            sphere.enabled = false;
        }
    }

    public void UIRay(Canvas c, Vector3 origin)
    {
        m_ped.position = origin;
        //m_ped.position = Input.mousePosition;
        results = new List<RaycastResult>();
        m_gr.Raycast(m_ped, results);

        if (results.Count > 0)
        {
            print(results[0].gameObject.name);
        }
    }

    //public void ShiftPosition()
    //{
    //    // 훈련장으로 이동하고 싶다.
    //    // 1. 이동할 위치
    //    GetComponent<CharacterController>().enabled = false;
    //    if (fsm.state == SH_PlayerFSM.State.NORMAL)
    //    {
    //        // 감독이면 TEACH로 상태를 전환하고 싶다.
    //        if (LaManager.instance.GetTrainNum() == 0)
    //            fsm.ChangeState(SH_PlayerFSM.State.TEACH);
    //        // 일반선수면 LEARN으로 상태를 전환하고 싶다.
    //        else
    //            fsm.ChangeState(SH_PlayerFSM.State.LEARN);

    //        transform.position += new Vector3(10000, 3, 10000);
    //        LaManager.instance.CanvasSwitch();
    //        LaManager.instance.PlusTrainNum();

    //        print("훈련장 이동 완료!!!!!");
    //    }
    //    else if (fsm.state == SH_PlayerFSM.State.TEACH || fsm.state == SH_PlayerFSM.State.LEARN)
    //    {
    //        fsm.ChangeState(SH_PlayerFSM.State.NORMAL);
    //        transform.position += new Vector3(-10000, 3, -10000);
    //        LaManager.instance.CanvasSwitch();
    //        LaManager.instance.MinusTrainNum();

    //        print("리그공간 이동 완료!!!!!");
    //    }
    //    else
    //    {
    //        print("훈련장으로 들어갈 수 없는 상태입니다.");
    //    }
    //    GetComponent<CharacterController>().enabled = true;
    //}
}
