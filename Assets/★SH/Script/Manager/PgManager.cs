using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Timeline.AnimationPlayableAsset;

public class PgManager : MonoBehaviourPunCallbacks
{
    public static PgManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;

        // 닉네임 설정 (탕카)
        PhotonNetwork.NickName = DBManager.instance.myData.nickName;
        print(PhotonNetwork.NickName);
    }

    public Vector3 spawnPos;
    [SerializeField]
    public List<GameObject> coachList = new List<GameObject>();
    [SerializeField]
    public List<GameObject> playerList = new List<GameObject>();

    public Transform UserListParent;
    public GameObject coachIconFactory;
    public GameObject playerIconFactory;

    void Start()
    {
        // 게임씬에서 다음씬으로 넘어갈때 동기화해주기 ( 게임씬 등에서 한번 )
        PhotonNetwork.AutomaticallySyncScene = true;
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

        PostUser2Maseter(go.GetPhotonView().ViewID, SH_TrainingUIManager.instance.isCoach);

        //// 플레이어의 authority가 player면
        //if (isCoach == false)
        //{
        //    // 플레이어에게 플레이어 권한을 부여하고 싶다.
        //    icon = playerIconFactory; 
        //    playerList.Add(go);
        //}
        //else
        //{
        //    // 플레이어에게 플레이어 권한을 부여하고 싶다.
        //    icon = coachIconFactory;
        //    coachList.Add(go);
        //}
    }

    void PostUser2Maseter(int viewID, bool isCoach)
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
            CreateUserIcon();
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

    void CreateUserIcon()
    {
        photonView.RPC(nameof(RPC_CreateUserIcon), RpcTarget.All);
    }
    [PunRPC]
    void RPC_CreateUserIcon()
    {
        foreach (Transform tr in UserListParent)
        {
            Destroy(tr.gameObject);
        }
        // coachList 정보를 뿌려주자.
        for (int i = 0; i < coachList.Count; i++)
        {
            GameObject icon = Instantiate(coachIconFactory, UserListParent);
        }
        // playerList 정보를 뿌려주자.
        for (int i = 0; i < playerList.Count; i++)
        {
            GameObject icon = Instantiate(playerIconFactory, UserListParent);
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
}
