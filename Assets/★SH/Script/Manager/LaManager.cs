using Photon.Pun;
using PlayFab.MultiplayerModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaManager : MonoBehaviourPunCallbacks
{
    public static LaManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Spawn Position
    public Vector3 spawnPos;

    void Start()
    {
        // 게임씬에서 다음씬으로 넘어갈때 동기화해주기 ( 게임씬 등에서 한번 )
        PhotonNetwork.AutomaticallySyncScene = true;

        // OnPhotonSerializeView 호출 빈도
        PhotonNetwork.SerializationRate = 60;
        // RPC 호출 빈도
        PhotonNetwork.SendRate = 60;

        SettingSpawnOption();

        CreateUser();
    }

    void Update()
    {
        
    }

    public void SettingSpawnOption()
    {
        spawnPos = Vector3.zero + Vector3.up * 3;
    }

    public void CreateUser()
    {

        // 플레이어를 생성한다.
        PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
    }
}
