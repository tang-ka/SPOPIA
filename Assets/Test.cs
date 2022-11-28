using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviourPun
{
    public GameObject cam;
    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine)
        {
            cam.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            string curRoom = PhotonNetwork.CurrentRoom.Name;
            string nextRoom = "PlayGround"; // DBManager.instance.myData.teamName;
            string preLobby = "1"; //PhotonNetwork.CurrentLobby.Name;
            LaManager.instance.EnterPlaygroundScene(curRoom, nextRoom, preLobby);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            string curRoom = PhotonNetwork.CurrentRoom.Name;
            string nextRoom = DBManager.instance.leagueInfo.leagueName;
            PgManager.instance.EnterScene(curRoom, nextRoom);
        }
    }
}
