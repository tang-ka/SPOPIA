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
        // ���Ӿ����� ���������� �Ѿ�� ����ȭ���ֱ� ( ���Ӿ� ��� �ѹ� )
        PhotonNetwork.AutomaticallySyncScene = true;

        // OnPhotonSerializeView ȣ�� ��
        PhotonNetwork.SerializationRate = 60;
        // RPC ȣ�� ��
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

        // �÷��̾ �����Ѵ�.
        //PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);

        // �ƹ�Ÿ �ٸ��� ����(����)
        // �÷��̾ �����Ѵ�.
        GameObject go = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
        // �ڽĿ�����Ʈ Body-Character-Geometry�� ����
        GameObject goBaby = go.transform.Find("Body").gameObject.transform.Find("Character").gameObject.transform.Find("Geometry").gameObject;
        // �ڽĿ�����Ʈ�� idx�� �����Ͽ� �ش� �ƹ�Ÿ�� ���ش�.
        goBaby.transform.GetChild(DBManager.instance.myData.avatarIdx).gameObject.SetActive(true);
    }
}
