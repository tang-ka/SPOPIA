using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SH_PlayerCrossHair : MonoBehaviourPun
{
    public Transform cam;

    public GameObject DataInputTable;

    public GameObject screenViewCanvas;

    SH_PlayerFSM fsm;

    void Start()
    {
        if (photonView.IsMine)
        {
            screenViewCanvas.SetActive(true);
        }

        //screenViewCanvas.SetActive(true);

        fsm = GetComponent<SH_PlayerFSM>();
    }

    MeshRenderer sphere;

    // Photon 동기화 필요 없음
    void Update()
    {
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
                    DataInputTable.SetActive(true);
                    fsm.ChangeState(SH_PlayerFSM.State.UIPLAYING);
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
}
