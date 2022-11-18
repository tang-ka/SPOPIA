using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using JetBrains.Annotations;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SH_PlayerCrossHair : MonoBehaviourPunCallbacks
{
    public Transform cam;

    public GameObject dataInputTable;

    public GameObject screenViewCanvas;

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
        }

        //screenViewCanvas.SetActive(true);
        if (dataInputTable != null)
            dataInputTable.SetActive(false);


        fsm = GetComponent<SH_PlayerFSM>();

        m_ped = new PointerEventData(null);
    }

    MeshRenderer sphere;

    // Photon ����ȭ �ʿ� ����
    void Update()
    {
        if (photonView.IsMine == false) return;
        // ���콺 Ż�� (����)
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

        // ġƮŰ
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GetComponent<CharacterController>().enabled = false;
            transform.position = new Vector3(40, 2.1f, 350);
            GetComponent<CharacterController>().enabled = true;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20))
        {
            print(hit.transform.gameObject.name);
            if (hit.transform.gameObject.name == "MatchDataSphere")
            {
                hit.transform.GetComponent<MeshRenderer>().enabled = true;
                sphere = hit.transform.GetComponent<MeshRenderer>();

                if (Input.GetMouseButtonDown(0))
                {
                    dataInputTable.SetActive(true);
                    fsm.ChangeState(SH_PlayerFSM.State.UIPLAYING);
                }
            }
            else if (hit.transform.gameObject.name == "EnterBG")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Canvas c = hit.transform.parent.transform.Find("Canvas").GetComponent<Canvas>();
                    m_gr = m_canvas.GetComponent<GraphicRaycaster>();
                    UIRay(c, ray.origin);
                    //ShiftPosition();
                    //EnterPlaygroundScene();
                }
            }
            //else if (hit.transform.gameObject.name == "MovePracticeCube")
            //{
            //    if (Input.GetMouseButtonDown(0))
            //    {
            //        //ShiftPosition();
            //        EnterPlaygroundScene();
            //    }
            //}
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

    //public void ShiftPosition()
    //{
    //    // �Ʒ������� �̵��ϰ� �ʹ�.
    //    // 1. �̵��� ��ġ
    //    GetComponent<CharacterController>().enabled = false;
    //    if (fsm.state == SH_PlayerFSM.State.NORMAL)
    //    {
    //        // �����̸� TEACH�� ���¸� ��ȯ�ϰ� �ʹ�.
    //        if (LaManager.instance.GetTrainNum() == 0)
    //            fsm.ChangeState(SH_PlayerFSM.State.TEACH);
    //        // �Ϲݼ����� LEARN���� ���¸� ��ȯ�ϰ� �ʹ�.
    //        else
    //            fsm.ChangeState(SH_PlayerFSM.State.LEARN);

    //        transform.position += new Vector3(10000, 3, 10000);
    //        LaManager.instance.CanvasSwitch();
    //        LaManager.instance.PlusTrainNum();

    //        print("�Ʒ��� �̵� �Ϸ�!!!!!");
    //    }
    //    else if (fsm.state == SH_PlayerFSM.State.TEACH || fsm.state == SH_PlayerFSM.State.LEARN)
    //    {
    //        fsm.ChangeState(SH_PlayerFSM.State.NORMAL);
    //        transform.position += new Vector3(-10000, 3, -10000);
    //        LaManager.instance.CanvasSwitch();
    //        LaManager.instance.MinusTrainNum();

    //        print("���װ��� �̵� �Ϸ�!!!!!");
    //    }
    //    else
    //    {
    //        print("�Ʒ������� �� �� ���� �����Դϴ�.");
    //    }
    //    GetComponent<CharacterController>().enabled = true;
    //}

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

    public static string roomName;

    public void EnterPlaygroundScene()
    {
        LeaveRoom();
        roomName = PhotonNetwork.CurrentRoom.Name;
        Debug.Log("���̽��ϳ�!!!!!!!!");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        PhotonNetwork.ConnectUsingSettings();

        Debug.Log("���̽��ϰ�");
    }

    public override void OnConnected()
    {
        base.OnConnected();
        print("��Ŀ��Ƽ��");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("��Ŀ��Ƽ����������");
        //PhotonNetwork.LoadLevel("WorldChoiceScene");
        //PhotonNetwork.LoadLevel("YS_MapCustomScene");
    }

}
