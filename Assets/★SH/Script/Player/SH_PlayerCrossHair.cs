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
        // �Ʒ������� �̵��ϰ� �ʹ�.
        // 1. �̵��� ��ġ
        GetComponent<CharacterController>().enabled = false;
        if (fsm.state == SH_PlayerFSM.State.NORMAL)
        {
            // �����̸� TEACH�� ���¸� ��ȯ�ϰ� �ʹ�.
            if (LaManager.instance.GetTrainNum() == 0)
                fsm.ChangeState(SH_PlayerFSM.State.TEACH);
            // �Ϲݼ����� LEARN���� ���¸� ��ȯ�ϰ� �ʹ�.
            else
                fsm.ChangeState(SH_PlayerFSM.State.LEARN);

            transform.position += new Vector3(10000, 3, 10000);
            LaManager.instance.CanvasSwitch();
            LaManager.instance.PlusTrainNum();

            print("�Ʒ��� �̵� �Ϸ�!!!!!");
        }
        else if (fsm.state == SH_PlayerFSM.State.TEACH || fsm.state == SH_PlayerFSM.State.LEARN)
        {
            fsm.ChangeState(SH_PlayerFSM.State.NORMAL);
            transform.position += new Vector3(-10000, 3, -10000);
            LaManager.instance.CanvasSwitch();
            LaManager.instance.MinusTrainNum();

            print("���װ��� �̵� �Ϸ�!!!!!");
        }
        else
        {
            print("�Ʒ������� �� �� ���� �����Դϴ�.");
        }
        GetComponent<CharacterController>().enabled = true;
    }


}
