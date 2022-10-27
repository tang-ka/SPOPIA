using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WcManager : MonoBehaviourPunCallbacks
{
    public Button btnSoccer;
    public Button btnBasketball;
    public Button btnBaseball;

    TypedLobby soccerWorld;
    TypedLobby baseballWorld;
    TypedLobby basketballWorld;
    TypedLobby selectWorld;

    private void Awake()
    {
        soccerWorld = new TypedLobby("SoccerWorld", LobbyType.Default);
        baseballWorld = new TypedLobby("BaseballWorld", LobbyType.Default);
        basketballWorld = new TypedLobby("BasketballWorld", LobbyType.Default);
    }

    void Start()
    {
        
    }

    public void JoinWorld(TypedLobby world, bool isInService)
    {
        selectWorld = world;
        print("���� ���� : " + selectWorld + "..");
        // ���� ����
        // PhotonNetwork.NickName = 
        if (isInService)
        {
            PhotonNetwork.JoinLobby(world);
        }
        else
        {
            print("Coming soon..");
        }

    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(selectWorld + " ���� ����");

        // RegionChoiceScene���� �̵�
        PhotonNetwork.LoadLevel("LeagueChoiceScene");
    }

    public void OnClickSoccer()
    {
        JoinWorld(soccerWorld, true);
    }

    public void OnClickBaseball()
    {
        JoinWorld(baseballWorld, false);
    }

    public void OnClickBasketball()
    {
        JoinWorld(basketballWorld, false);
    }
}