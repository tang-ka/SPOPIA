using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SH_PlayerCrossHair : MonoBehaviourPun
{
    public Transform cam;

    public GameObject dataInputTable;

    public GameObject screenViewCanvas;

    SH_PlayerFSM fsm;

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
    }

    MeshRenderer sphere;

    // Photon 동기화 필요 없음
    void Update()
    {
        if (photonView.IsMine == false) return;
        // 마우스 탈출 (영수)
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

        // 치트키
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
            else if (hit.transform.gameObject.name == "MovePracticeCube")
            {
                if (Input.GetMouseButtonDown(0))
                    ShiftPosition();
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

    public void ShiftPosition()
    {
        // 훈련장으로 이동하고 싶다.
        // 1. 이동할 위치
        GetComponent<CharacterController>().enabled = false;
        if (fsm.state == SH_PlayerFSM.State.NORMAL)
        {
            // 감독이면 TEACH로 상태를 전환하고 싶다.
            if (LaManager.instance.GetTrainNum() == 0)
                fsm.ChangeState(SH_PlayerFSM.State.TEACH);
            // 일반선수면 LEARN으로 상태를 전환하고 싶다.
            else
                fsm.ChangeState(SH_PlayerFSM.State.LEARN);

            transform.position += new Vector3(10000, 3, 10000);
            LaManager.instance.CanvasSwitch();
            LaManager.instance.PlusTrainNum();

            print("훈련장 이동 완료!!!!!");
        }
        else if (fsm.state == SH_PlayerFSM.State.TEACH || fsm.state == SH_PlayerFSM.State.LEARN)
        {
            fsm.ChangeState(SH_PlayerFSM.State.NORMAL);
            transform.position += new Vector3(-10000, 3, -10000);
            LaManager.instance.CanvasSwitch();
            LaManager.instance.MinusTrainNum();

            print("리그공간 이동 완료!!!!!");
        }
        else
        {
            print("훈련장으로 들어갈 수 없는 상태입니다.");
        }
        GetComponent<CharacterController>().enabled = true;
    }


}
