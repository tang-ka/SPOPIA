using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SH_Portal : MonoBehaviourPun
{
    public bool isReadyEnter = false;
    public GameObject triggerPlayer;

    public Button btnYes;
    public Button btnNo;

    public GameObject enterBG;

    Vector3 dir = Vector3.zero;
        
    void Start()
    {
        btnYes.onClick.AddListener(OnClickYes);
        btnNo.onClick.AddListener(OnClickNo);

        enterBG.SetActive(false);
    }

    private void OnClickYes()
    {
        print("예쓰~~~!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
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

    private void OnClickNo()
    {
        print("노노노노노노노노!!!!!!!!!!!!!!!!!!!!!!");

        enterBG.SetActive(false);
        isReadyEnter = false;

        triggerPlayer.GetComponent<SH_PlayerFSM>().ChangeState(SH_PlayerFSM.State.NORMAL);
    }

    void Update()
    {
        if (triggerPlayer == null) return;

        if (enterBG.activeSelf)
        {
            dir = transform.position - triggerPlayer.transform.position;
            dir.y = 0;
            dir.Normalize();

            enterBG.transform.forward = dir;

            //float angle = Vector3.Angle(dir, enterBG.transform.right);
            //enterBG.transform.eulerAngles = new Vector3(90, 0, angle);
        }

        triggerPlayer.GetComponent<SH_PlayerCrossHair>().guideMessage.SetActive(enterBG.activeSelf);
    }

    // 트리거에 걸리면 캔버스를 켜고싶다.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                triggerPlayer = other.gameObject;
                enterBG.SetActive(true);
                isReadyEnter = true;
            }
        }
    }

    // 트리거에서 나가면 캔버스를 끄고 싶다.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == triggerPlayer)
        {
            enterBG.SetActive(false);
            isReadyEnter = false;
        }
    }
}
