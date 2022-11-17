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

    public GameObject leagueAreaCanvas;
    public GameObject playgroundCanvas;

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

        leagueAreaCanvas.SetActive(true);
        playgroundCanvas.SetActive(false);
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

        // ��Ʈ��ũ(RPC - �� �ƹ�Ÿ�� �ٸ� ��������׵� ���̰Բ�)
        photonView.RPC("RpcCreateUser", RpcTarget.OthersBuffered, go.GetPhotonView().ViewID, DBManager.instance.myData.avatarIdx);
    }

    [PunRPC]
    void RpcCreateUser(int ID, int idx)
    {
        PhotonView.Find(ID).gameObject.transform.Find("Body").gameObject.transform.Find("Character").gameObject.transform.Find("Geometry").gameObject.transform.GetChild(idx).gameObject.SetActive(true);
    }

    public void CanvasSwitch()
    {
        if (leagueAreaCanvas.activeSelf != playgroundCanvas.activeSelf)
        {
            leagueAreaCanvas.SetActive(!leagueAreaCanvas.activeSelf);
            playgroundCanvas.SetActive(!playgroundCanvas.activeSelf);
        }
        else
        {
            print("ĵ���� ���°� �����ϴ�. Ȯ���� �ʿ��մϴ�.");
        }
    }

    int trainNum = 0;

    public void PlusTrainNum()
    {
        photonView.RPC("RPC_PlusTrainNum", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_PlusTrainNum()
    {
        trainNum++;
    }

    public void MinusTrainNum()
    {
        photonView.RPC("RPC_MinusTrainNum", RpcTarget.All);
    }

    [PunRPC]
    public void RPC_MinusTrainNum()
    {
        trainNum--;

        if (trainNum < 0)
            trainNum = 0;
    }
    public int GetTrainNum()
    {
        return trainNum;
    }

}
