using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.Timeline.AnimationPlayableAsset;

public class PgManager : MonoBehaviourPunCallbacks
{
    public static PgManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public Vector3 spawnPos;
    [SerializeField]
    public List<GameObject> coachList = new List<GameObject>();
    [SerializeField]
    public List<GameObject> playerList = new List<GameObject>();

    public Transform UserListParent;
    public GameObject coachIconFactory;
    public GameObject playerIconFactory;

    // 파일 이름
    string filename;
    public RawImage rawImg;


    void Start()
    {
        // 게임씬에서 다음씬으로 넘어갈때 동기화해주기 ( 게임씬 등에서 한번 )
        PhotonNetwork.AutomaticallySyncScene = false;
        // OnPhotonSerializeView 호출 빈도
        PhotonNetwork.SerializationRate = 60;
        // RPC 호출 빈도
        PhotonNetwork.SendRate = 60;

        // 입력을 받을 떄까지 플레이어가 생성 되지 않게 하고 싶다.

        //SettingSpawnOption();

        //CreateUser();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SettingSpawnOption()
    {
        spawnPos = Vector3.zero + Vector3.up * 3;
    }

    GameObject go;
    GameObject icon;

    public void CreateUser()
    {
        // 플레이어를 생성한다.
        //PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);

        // 아바타 다르게 생성(영수)
        // 플레이어를 생성한다.
        go = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
        // 자식오브젝트 Body-Character-Geometry에 접근
        GameObject goBaby = go.transform.Find("Body").Find("Character").Find("Geometry").gameObject;
        // 자식오브젝트에 idx로 접근하여 해당 아바타를 켜준다.
        goBaby.transform.GetChild(DBManager.instance.myData.avatarIdx).gameObject.SetActive(true);

        go.name += PhotonNetwork.CurrentRoom.PlayerCount;

        // 네트워크(RPC - 내 아바타가 다른 사람들한테도 보이게끔)
        photonView.RPC(nameof(RpcCreateUser2), RpcTarget.OthersBuffered, go.GetPhotonView().ViewID, DBManager.instance.myData.avatarIdx);
    }

    [PunRPC]
    void RpcCreateUser2(int ID, int idx)
    {
        PhotonView.Find(ID).transform.Find("Body").Find("Character").Find("Geometry").GetChild(idx).gameObject.SetActive(true);
        print(ID + ",  " + idx);
    }

    public void MyStart()
    {
        SettingSpawnOption();
        CreateUser();

        PostUser2Master(go.GetPhotonView().ViewID, SH_TrainingUIManager.instance.isCoach);
    }

    void PostUser2Master(int viewID, bool isCoach)
    {
        photonView.RPC(nameof(RPC_PostUser2Master), RpcTarget.MasterClient, viewID, isCoach);
    }

    [PunRPC]
    void RPC_PostUser2Master(int viewID, bool isCoach)
    {
        // 1. MasterClient는 본인을 등록하고 싶다.
        if (go.GetPhotonView().ViewID == viewID)
        {
            SetAuthority(go, isCoach);
            if (isCoach)
                go.GetComponent<SH_PlayerFSM>().ChangeState(SH_PlayerFSM.State.TEACH);
            else
                go.GetComponent<SH_PlayerFSM>().ChangeState(SH_PlayerFSM.State.LEARN);
        }
        // 2. MasterClient가 아니면 해당 ID의 gameObject를 찾아서 등록하고 싶다.
        else
        {
            print("나 마스터 아니야!!!");
            SetAuthority(PhotonView.Find(viewID).gameObject, isCoach);
        }

        // 3. 내가 방장이라면(나는 방장이다. 오류 방지)
        if (PhotonNetwork.IsMasterClient)
        {
            ClearList();
            // coachList 정보를 뿌려주자.
            for (int i = 0; i < coachList.Count; i++)
            {
                UpdateList(coachList[i].GetPhotonView().ViewID, i, true);
            }
            // playerList 정보를 뿌려주자.
            for (int i = 0; i < playerList.Count; i++)
            {
                UpdateList(playerList[i].GetPhotonView().ViewID, i, false);
            }
            CreateUserIcon(viewID);
        }
    }

    void ClearList()
    {
        photonView.RPC(nameof(RPC_ClearList), RpcTarget.Others);
    }
    void UpdateList(int viewID, int index, bool isCoach)
    {
        photonView.RPC(nameof(RPC_UpdateList), RpcTarget.Others, viewID, index, isCoach);
    }
    [PunRPC]
    void RPC_ClearList()
    {
        coachList.Clear();
        playerList.Clear();
    }
    [PunRPC]
    void RPC_UpdateList(int viewID, int index, bool isCoach)
    {
        if (isCoach)
        {
            GameObject user = PhotonView.Find(viewID).gameObject;
            coachList.Insert(index, user);
            user.GetComponent<SH_PlayerFSM>().ChangeState(SH_PlayerFSM.State.TEACH);
        }
        else
        {
            GameObject user = PhotonView.Find(viewID).gameObject;
            playerList.Insert(index, user);
            user.GetComponent<SH_PlayerFSM>().ChangeState(SH_PlayerFSM.State.LEARN);
        }
    }

    void CreateUserIcon(int viewID)
    {
        photonView.RPC(nameof(RPC_CreateUserIcon), RpcTarget.All, viewID);
    }
    [PunRPC]
    void RPC_CreateUserIcon(int viewID)
    {
        foreach (Transform tr in UserListParent)
        {
            Destroy(tr.gameObject);
        }
        // coachList 정보를 뿌려주자.
        for (int i = 0; i < coachList.Count; i++)
        {
            GameObject icon = Instantiate(coachIconFactory, UserListParent);
            int _viewID = coachList[i].GetPhotonView().ViewID;
            icon.GetComponent<SH_UserIcon>().Init(_viewID);
        }
        // playerList 정보를 뿌려주자.
        for (int i = 0; i < playerList.Count; i++)
        {
            DownloadProfileImage();

            GameObject icon = Instantiate(playerIconFactory, UserListParent);
            int _viewID = playerList[i].GetPhotonView().ViewID;
            icon.GetComponent<SH_UserIcon>().Init(_viewID);

            icon.transform.Find("UserIcon").GetComponent<RawImage>().texture = rawImg.texture;
        }
    }

    void SetAuthority(GameObject user, bool isCoach)
    {
        // 유저가 일반 플레이어를 선택했다면
        if (isCoach == false)
        {
            playerList.Add(user);
        }
        // 유저가 코치를 선택했다면
        else
        {
            coachList.Add(user);
        }
    }

    public void DownloadProfileImage()
    {
        filename = "pf_" + DBManager.instance.myData.nickName + ".png";

        byte[] byteTexture = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/" + filename);

        if (byteTexture.Length > 0)
        {
            Texture2D t = new Texture2D(0, 0);
            t.LoadImage(byteTexture);

            rawImg.texture = t;
        }
    }

    #region Change scene from LeagueArea to Playground
    public string preRoomName;
    public string nextRoomName;

    public void EnterScene(string preRoom, string nextRoom)
    {
        preRoomName = preRoom;
        nextRoomName = nextRoom;

        LeaveRoom();
        Debug.Log("나이스하냐!!!!!!!!22");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        //PhotonNetwork.ConnectUsingSettings();

        Debug.Log("온레프트룸22");
    }

    public override void OnConnected()
    {
        base.OnConnected();
        print("온커넥티드22");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        print("온커넥티드투마스터22");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("온쪼인드로비22");
        print(SceneManager.GetActiveScene().name + "온조인드로비");
        //CreateLeague();
        JoinLeagueWorld();
    }

    public void CreateLeague()
    {
        RoomOptions leagueOption = new RoomOptions();
        leagueOption.MaxPlayers = 0;
        leagueOption.IsVisible = true;

        int teamNum = DBManager.instance.leagueInfo.teamNum;

        // 리그 정보 커스텀해서 넣고 싶다.
        // 1. 커스텀 정보 세팅
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["teamNum"] = teamNum;

        leagueOption.CustomRoomProperties = hash;

        // 2.커스텀 정보를 공개하고 싶다.
       leagueOption.CustomRoomPropertiesForLobby = new string[] { "teamNum" };

        // 해당 옵션의 방에 참가하거나 방을 생성하고 싶다.
        //PhotonNetwork.JoinOrCreateRoom(nextRoomName, leagueOption, TypedLobby.Default);
        PhotonNetwork.CreateRoom(nextRoomName, leagueOption, TypedLobby.Default);
        print("훈련장 거");
    }

    // 방 생성 완료
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("훈련장 생성 완료22");
    }
    // 방 생성 실패
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("훈련장 생성 실패22, " + returnCode + ", " + message);
        JoinLeagueWorld();
    }

    // 방 입장
    public void JoinLeagueWorld()
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

        //SceneManager.LoadScene("LeagueAreaScene");
        PhotonNetwork.LoadLevel("LeagueAreaScene");

        print("리그 월드로 돌아왔습니다22.");
    }

    // 방 입장 실패시 호출되는 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("훈련장 진입 실패22" + returnCode + ", " + message);
        CreateLeague();
    }
    #endregion
}
