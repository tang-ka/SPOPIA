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
        // 게임씬에서 다음씬으로 넘어갈때 동기화해주기 ( 게임씬 등에서 한번 )
        PhotonNetwork.AutomaticallySyncScene = true;
        // OnPhotonSerializeView 호출 빈도
        PhotonNetwork.SerializationRate = 60;
        // RPC 호출 빈도
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

        // 플레이어를 생성한다.
        //PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);

        // 아바타 다르게 생성(영수)
        // 플레이어를 생성한다.
        GameObject go = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
        // 자식오브젝트 Body-Character-Geometry에 접근
        GameObject goBaby = go.transform.Find("Body").gameObject.transform.Find("Character").gameObject.transform.Find("Geometry").gameObject;
        // 자식오브젝트에 idx로 접근하여 해당 아바타를 켜준다.
        goBaby.transform.GetChild(DBManager.instance.myData.avatarIdx).gameObject.SetActive(true);

        // 네트워크(RPC - 내 아바타가 다른 사람들한테도 보이게끔)
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
        Debug.Log("나이스하냐!!!!!!!!");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        PhotonNetwork.ConnectUsingSettings();

        Debug.Log("온레프트룸");
    }

    public override void OnConnected()
    {
        base.OnConnected();
        print("온커넥티드");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        print("온커넥티드투마스터");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("온쪼인드로비");
        print(SceneManager.GetActiveScene().name + "온조인드로비");
        //CreatePlayground();
        JoinPlayground();
    }

    public void CreatePlayground()
    {
        RoomOptions leagueOption = new RoomOptions();
        leagueOption.MaxPlayers = 11;
        leagueOption.IsVisible = false;

        //int teamNum = DBManager.instance.leagueInfo.teamNum;

        //// 리그 정보 커스텀해서 넣고 싶다.
        //// 1. 커스텀 정보 세팅
        //ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        //hash["teamNum"] = teamNum;

        //leagueOption.CustomRoomProperties = hash;

        // 2. 커스텀 정보를 공개하고 싶다.
        //leagueOption.CustomRoomPropertiesForLobby = new string[] { "teamNum" };

        // 해당 옵션의 방에 참가하거나 방을 생성하고 싶다.
        //PhotonNetwork.JoinOrCreateRoom(nextRoomName, leagueOption, TypedLobby.Default);
        PhotonNetwork.CreateRoom(nextRoomName, leagueOption, TypedLobby.Default);
        
        print("시발 리그월드");
    }

    // 방 생성 완료
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("훈련장 생성 완료");
    }
    // 방 생성 실패
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("훈련장 생성 실패, " + returnCode + ", " + message);
    }

    // 방 입장
    public void JoinPlayground()
    {
        PhotonNetwork.JoinRoom(nextRoomName);
    }

    // 방 입장이 성공했을 때 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(SceneManager.GetActiveScene().name + "온조인드룸");

        //if (SceneManager.GetActiveScene().name == "LeagueAreaScene")
        //    SceneManager.LoadScene("PlayGroundScene");
        //else if (SceneManager.GetActiveScene().name == "PlayGroundScene")
        //    SceneManager.LoadScene("LeagueAreaScene");

        SceneManager.LoadScene("PlayGroundScene");

        print("훈련장 진입에 성공했습니다.");
    }

    // 방 입장 실패시 호출되는 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("훈련장 진입 실패" + returnCode + ", " + message);
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
    //        print("캔버스 상태가 같습니다. 확인이 필요합니다.");
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
