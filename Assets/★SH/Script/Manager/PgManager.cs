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

        // �г��� ���� (��ī)
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
        // ���Ӿ����� ���������� �Ѿ�� ����ȭ���ֱ� ( ���Ӿ� ��� �ѹ� )
        PhotonNetwork.AutomaticallySyncScene = true;
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

        PostUser2Maseter(go.GetPhotonView().ViewID, SH_TrainingUIManager.instance.isCoach);

        //// �÷��̾��� authority�� player��
        //if (isCoach == false)
        //{
        //    // �÷��̾�� �÷��̾� ������ �ο��ϰ� �ʹ�.
        //    icon = playerIconFactory; 
        //    playerList.Add(go);
        //}
        //else
        //{
        //    // �÷��̾�� �÷��̾� ������ �ο��ϰ� �ʹ�.
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
        // coachList ������ �ѷ�����.
        for (int i = 0; i < coachList.Count; i++)
        {
            GameObject icon = Instantiate(coachIconFactory, UserListParent);
        }
        // playerList ������ �ѷ�����.
        for (int i = 0; i < playerList.Count; i++)
        {
            GameObject icon = Instantiate(playerIconFactory, UserListParent);
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
}
