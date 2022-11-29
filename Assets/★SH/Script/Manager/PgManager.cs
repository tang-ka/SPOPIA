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

    // ���� �̸�
    string filename;
    public RawImage rawImg;


    void Start()
    {
        // ���Ӿ����� ���������� �Ѿ�� ����ȭ���ֱ� ( ���Ӿ� ��� �ѹ� )
        PhotonNetwork.AutomaticallySyncScene = false;
        // OnPhotonSerializeView ȣ�� ��
        PhotonNetwork.SerializationRate = 60;
        // RPC ȣ�� ��
        PhotonNetwork.SendRate = 60;

        // �Է��� ���� ������ �÷��̾ ���� ���� �ʰ� �ϰ� �ʹ�.

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
        // �÷��̾ �����Ѵ�.
        //PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);

        // �ƹ�Ÿ �ٸ��� ����(����)
        // �÷��̾ �����Ѵ�.
        go = PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
        // �ڽĿ�����Ʈ Body-Character-Geometry�� ����
        GameObject goBaby = go.transform.Find("Body").Find("Character").Find("Geometry").gameObject;
        // �ڽĿ�����Ʈ�� idx�� �����Ͽ� �ش� �ƹ�Ÿ�� ���ش�.
        goBaby.transform.GetChild(DBManager.instance.myData.avatarIdx).gameObject.SetActive(true);

        go.name += PhotonNetwork.CurrentRoom.PlayerCount;

        // ��Ʈ��ũ(RPC - �� �ƹ�Ÿ�� �ٸ� ��������׵� ���̰Բ�)
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
        // 1. MasterClient�� ������ ����ϰ� �ʹ�.
        if (go.GetPhotonView().ViewID == viewID)
        {
            SetAuthority(go, isCoach);
            if (isCoach)
                go.GetComponent<SH_PlayerFSM>().ChangeState(SH_PlayerFSM.State.TEACH);
            else
                go.GetComponent<SH_PlayerFSM>().ChangeState(SH_PlayerFSM.State.LEARN);
        }
        // 2. MasterClient�� �ƴϸ� �ش� ID�� gameObject�� ã�Ƽ� ����ϰ� �ʹ�.
        else
        {
            print("�� ������ �ƴϾ�!!!");
            SetAuthority(PhotonView.Find(viewID).gameObject, isCoach);
        }

        // 3. ���� �����̶��(���� �����̴�. ���� ����)
        if (PhotonNetwork.IsMasterClient)
        {
            ClearList();
            // coachList ������ �ѷ�����.
            for (int i = 0; i < coachList.Count; i++)
            {
                UpdateList(coachList[i].GetPhotonView().ViewID, i, true);
            }
            // playerList ������ �ѷ�����.
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
        // coachList ������ �ѷ�����.
        for (int i = 0; i < coachList.Count; i++)
        {
            GameObject icon = Instantiate(coachIconFactory, UserListParent);
            int _viewID = coachList[i].GetPhotonView().ViewID;
            icon.GetComponent<SH_UserIcon>().Init(_viewID);
        }
        // playerList ������ �ѷ�����.
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
        // ������ �Ϲ� �÷��̾ �����ߴٸ�
        if (isCoach == false)
        {
            playerList.Add(user);
        }
        // ������ ��ġ�� �����ߴٸ�
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
        Debug.Log("���̽��ϳ�!!!!!!!!22");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        //PhotonNetwork.ConnectUsingSettings();

        Debug.Log("�·���Ʈ��22");
    }

    public override void OnConnected()
    {
        base.OnConnected();
        print("��Ŀ��Ƽ��22");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        print("��Ŀ��Ƽ����������22");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("�����ε�κ�22");
        print(SceneManager.GetActiveScene().name + "�����ε�κ�");
        //CreateLeague();
        JoinLeagueWorld();
    }

    public void CreateLeague()
    {
        RoomOptions leagueOption = new RoomOptions();
        leagueOption.MaxPlayers = 0;
        leagueOption.IsVisible = true;

        int teamNum = DBManager.instance.leagueInfo.teamNum;

        // ���� ���� Ŀ�����ؼ� �ְ� �ʹ�.
        // 1. Ŀ���� ���� ����
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["teamNum"] = teamNum;

        leagueOption.CustomRoomProperties = hash;

        // 2.Ŀ���� ������ �����ϰ� �ʹ�.
       leagueOption.CustomRoomPropertiesForLobby = new string[] { "teamNum" };

        // �ش� �ɼ��� �濡 �����ϰų� ���� �����ϰ� �ʹ�.
        //PhotonNetwork.JoinOrCreateRoom(nextRoomName, leagueOption, TypedLobby.Default);
        PhotonNetwork.CreateRoom(nextRoomName, leagueOption, TypedLobby.Default);
        print("�Ʒ��� ��");
    }

    // �� ���� �Ϸ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("�Ʒ��� ���� �Ϸ�22");
    }
    // �� ���� ����
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("�Ʒ��� ���� ����22, " + returnCode + ", " + message);
        JoinLeagueWorld();
    }

    // �� ����
    public void JoinLeagueWorld()
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

        //SceneManager.LoadScene("LeagueAreaScene");
        PhotonNetwork.LoadLevel("LeagueAreaScene");

        print("���� ����� ���ƿԽ��ϴ�22.");
    }

    // �� ���� ���н� ȣ��Ǵ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("�Ʒ��� ���� ����22" + returnCode + ", " + message);
        CreateLeague();
    }
    #endregion
}
