using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PgManager : MonoBehaviourPunCallbacks
{
    public static PgManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public Vector3 spawnPos;

    void Start()
    {
        // 게임씬에서 다음씬으로 넘어갈때 동기화해주기 ( 게임씬 등에서 한번 )
        PhotonNetwork.AutomaticallySyncScene = true;
        // OnPhotonSerializeView 호출 빈도
        PhotonNetwork.SerializationRate = 60;
        // RPC 호출 빈도
        PhotonNetwork.SendRate = 60;

        // 입력을 받을 떄까지 플레이어가 생성 되지 않게 하고 싶다.


        SettingSpawnOption();

        CreateUser();
    }

    public void SettingSpawnOption()
    {
        spawnPos = Vector3.zero + Vector3.up * 3;
    }

    public void CreateUser()
    {
        // 플레이어를 생성한다.
        //PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);

        // 아바타 다르게 생성(영수)
        // 플레이어를 생성한다.
        GameObject go = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
        // 자식오브젝트 Body-Character-Geometry에 접근
        GameObject goBaby = go.transform.Find("Body").Find("Character").Find("Geometry").gameObject;
        // 자식오브젝트에 idx로 접근하여 해당 아바타를 켜준다.
        goBaby.transform.GetChild(DBManager.instance.myData.avatarIdx).gameObject.SetActive(true);

        // 네트워크(RPC - 내 아바타가 다른 사람들한테도 보이게끔)
        print(go.GetPhotonView().ViewID);
        photonView.RPC(nameof(RpcCreateUser2), RpcTarget.OthersBuffered, go.GetPhotonView().ViewID, DBManager.instance.myData.avatarIdx);
    }

    [PunRPC]
    void RpcCreateUser2(int ID, int idx)
    {
        PhotonView.Find(ID).transform.Find("Body").Find("Character").Find("Geometry").GetChild(idx).gameObject.SetActive(true);
        print(ID + ",  " + idx);
    }

    void SetAuthority()
    {
        // 플레이어의 authority가 coach면 
    }
}
