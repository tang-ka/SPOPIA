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
        // ���Ӿ����� ���������� �Ѿ�� ����ȭ���ֱ� ( ���Ӿ� ��� �ѹ� )
        PhotonNetwork.AutomaticallySyncScene = true;
        // OnPhotonSerializeView ȣ�� ��
        PhotonNetwork.SerializationRate = 60;
        // RPC ȣ�� ��
        PhotonNetwork.SendRate = 60;

        // �Է��� ���� ������ �÷��̾ ���� ���� �ʰ� �ϰ� �ʹ�.


        SettingSpawnOption();

        CreateUser();
    }

    public void SettingSpawnOption()
    {
        spawnPos = Vector3.zero + Vector3.up * 3;
    }

    public void CreateUser()
    {
        // �÷��̾ �����Ѵ�.
        //PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);

        // �ƹ�Ÿ �ٸ��� ����(����)
        // �÷��̾ �����Ѵ�.
        GameObject go = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
        // �ڽĿ�����Ʈ Body-Character-Geometry�� ����
        GameObject goBaby = go.transform.Find("Body").Find("Character").Find("Geometry").gameObject;
        // �ڽĿ�����Ʈ�� idx�� �����Ͽ� �ش� �ƹ�Ÿ�� ���ش�.
        goBaby.transform.GetChild(DBManager.instance.myData.avatarIdx).gameObject.SetActive(true);

        // ��Ʈ��ũ(RPC - �� �ƹ�Ÿ�� �ٸ� ��������׵� ���̰Բ�)
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
        // �÷��̾��� authority�� coach�� 
    }
}
