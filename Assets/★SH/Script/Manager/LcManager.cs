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

        // ���� ���� Ŀ�����ؼ� �ְ� �ʹ�.
        // 1. Ŀ���� ���� ����
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["teamNum"] = teamNum;

        leagueOption.CustomRoomProperties = hash;

        // 2. Ŀ���� ������ �����ϰ� �ʹ�.
        leagueOption.CustomRoomPropertiesForLobby = new string[] { "teamNum" };

        // �ش� �ɼ����� ����(��)�� �����ϰ� �ʹ�.
        PhotonNetwork.CreateRoom(inputLeagueName.text, leagueOption, TypedLobby.Default);
    }

    public void JoinLeague()
    {
        PhotonNetwork.JoinRoom(inputLeagueName.text);
    }

    #region �ݹ��Լ� (����� ����, ����)
    // �� ���� �Ϸ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("�ش� ���׸� �����Ͽ����ϴ�.");
    }
    // �� ���� ����
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("���� ���� ����, " + returnCode + ", " + message);
    }
    #endregion

    #region �ݹ��Լ� (������ ����, ����)
    // �� ������ �������� �� ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("LeagueAreaScene");
        print("���� ���Կ� �����߽��ϴ�.");
    }

    // �� ���� ���н� ȣ��Ǵ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("���� ���� ����" + returnCode + ", " + message);
    }
    #endregion
}
