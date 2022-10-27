using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;

public class LcManager : MonoBehaviourPunCallbacks
{
    public InputField inputLeagueName;
    public InputField inputTeamNum;

    public Button btnCreateLeague;
    public Button btnJoinLeague;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inputLeagueName.text = "SPOPIA TEST";
            inputTeamNum.text = "4";
        }
    }

    public void OnClickCreateLeague()
    {
        RoomOptions leagueOption = new RoomOptions();
        leagueOption.MaxPlayers = 0;
        leagueOption.IsVisible = true;

        int teamNum = int.Parse(inputTeamNum.text);

        // 리그 정보 커스텀해서 넣고 싶다.
        // 1. 커스텀 정보 세팅
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["teamNum"] = teamNum;

        leagueOption.CustomRoomProperties = hash;

        // 2. 커스텀 정보를 공개하고 싶다.
        leagueOption.CustomRoomPropertiesForLobby = new string[] { "teamNum" };

        // 해당 옵선으로 리그(방)를 생성하고 싶다.
        PhotonNetwork.CreateRoom(inputLeagueName.text, leagueOption, TypedLobby.Default);
    }

    public void JoinLeague()
    {
        PhotonNetwork.JoinRoom(inputLeagueName.text);
    }

    #region 콜백함수 (방생성 성공, 실패)
    // 방 생성 완료
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("해당 리그를 생성하였습니다.");
    }
    // 방 생성 실패
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("리그 생성 실패, " + returnCode + ", " + message);
    }
    #endregion

    #region 콜백함수 (방입장 성공, 실패)
    // 방 입장이 성공했을 때 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("LeagueAreaScene");
        print("리그 진입에 성공했습니다.");
    }

    // 방 입장 실패시 호출되는 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("리그 진입 실패" + returnCode + ", " + message);
    }
    #endregion
}
