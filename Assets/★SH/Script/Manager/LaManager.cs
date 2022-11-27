using Photon.Pun;
using Photon.Realtime;
using PlayFab.MultiplayerModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        //leagueAreaCanvas.SetActive(true);
        //playgroundCanvas.SetActive(false);
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
        PhotonView.Find(ID).transform.Find("Body").transform.Find("Character").transform.Find("Geometry").transform.GetChild(idx).gameObject.SetActive(true);
    }

    #region Change scene from LeagueArea to Playground
    public string preRoomName;
    public string nextRoomName;
    public string preLobbyName;

    public void EnterPlaygroundScene(string preRoom, string nextRoom, string preLobby)
    {
        preRoomName = preRoom;
        nextRoomName = nextRoom;
        preLobbyName = preLobby;

        LeaveRoom();
        Debug.Log("���̽��ϳ�!!!!!!!!");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        PhotonNetwork.ConnectUsingSettings();

        Debug.Log("�·���Ʈ��");
    }

    public override void OnConnected()
    {
        base.OnConnected();
        print("��Ŀ��Ƽ��");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        print("��Ŀ��Ƽ����������");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("�����ε�κ�");
        print(SceneManager.GetActiveScene().name + "�����ε�κ�");
        //CreatePlayground();
        JoinPlayground();
    }

    public void CreatePlayground()
    {
        RoomOptions leagueOption = new RoomOptions();
        leagueOption.MaxPlayers = 11;
        leagueOption.IsVisible = false;

        //int teamNum = DBManager.instance.leagueInfo.teamNum;

        //// ���� ���� Ŀ�����ؼ� �ְ� �ʹ�.
        //// 1. Ŀ���� ���� ����
        //ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        //hash["teamNum"] = teamNum;

        //leagueOption.CustomRoomProperties = hash;

        // 2. Ŀ���� ������ �����ϰ� �ʹ�.
        //leagueOption.CustomRoomPropertiesForLobby = new string[] { "teamNum" };

        // �ش� �ɼ��� �濡 �����ϰų� ���� �����ϰ� �ʹ�.
        //PhotonNetwork.JoinOrCreateRoom(nextRoomName, leagueOption, TypedLobby.Default);
        PhotonNetwork.CreateRoom(nextRoomName, leagueOption, TypedLobby.Default);
        
        print("�ù� ���׿���");
    }

    // �� ���� �Ϸ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("�Ʒ��� ���� �Ϸ�");
    }
    // �� ���� ����
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("�Ʒ��� ���� ����, " + returnCode + ", " + message);
    }

    // �� ����
    public void JoinPlayground()
    {
        PhotonNetwork.JoinRoom(nextRoomName);
    }

    // �� ������ �������� �� ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(SceneManager.GetActiveScene().name + "�����ε��");

        //if (SceneManager.GetActiveScene().name == "LeagueAreaScene")
        //    SceneManager.LoadScene("PlayGroundScene");
        //else if (SceneManager.GetActiveScene().name == "PlayGroundScene")
        //    SceneManager.LoadScene("LeagueAreaScene");

        SceneManager.LoadScene("PlayGroundScene");

        print("�Ʒ��� ���Կ� �����߽��ϴ�.");
    }

    // �� ���� ���н� ȣ��Ǵ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("�Ʒ��� ���� ����" + returnCode + ", " + message);
        CreatePlayground();
    }
    #endregion

    //public void CanvasSwitch()
    //{
    //    if (leagueAreaCanvas.activeSelf != playgroundCanvas.activeSelf)
    //    {
    //        leagueAreaCanvas.SetActive(!leagueAreaCanvas.activeSelf);
    //        playgroundCanvas.SetActive(!playgroundCanvas.activeSelf);
    //    }
    //    else
    //    {
    //        print("ĵ���� ���°� �����ϴ�. Ȯ���� �ʿ��մϴ�.");
    //    }
    //}

    //int trainNum = 0;

    //public void PlusTrainNum()
    //{
    //    photonView.RPC("RPC_PlusTrainNum", RpcTarget.All);
    //}

    //[PunRPC]
    //public void RPC_PlusTrainNum()
    //{
    //    trainNum++;
    //}

    //public void MinusTrainNum()
    //{
    //    photonView.RPC("RPC_MinusTrainNum", RpcTarget.All);
    //}

    //[PunRPC]
    //public void RPC_MinusTrainNum()
    //{
    //    trainNum--;

    //    if (trainNum < 0)
    //        trainNum = 0;
    //}
    //public int GetTrainNum()
    //{
    //    return trainNum;
    //}
}
