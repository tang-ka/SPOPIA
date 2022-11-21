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
    }

    public Vector3 spawnPos;
    [SerializeField]
    List<GameObject> coachList = new List<GameObject>();
    [SerializeField]
    List<GameObject> playerList = new List<GameObject>();

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
        if (photonView.ViewID == viewID)
        {
            SetAuthority(go, isCoach);
        }
        // 2. MasterClient�� �ƴϸ� �ش� ID�� gameObject�� ã�Ƽ� ����ϰ� �ʹ�.
        else
        {
            print("�� ������ �ƴϾ�!!!");
            SetAuthority(PhotonView.Find(viewID).gameObject, isCoach);
        }

        // 3. ���� �����̶��(���� �����̴�. ���� ����)
        if (photonView.IsMine)
        {
            CreateUserIcon();
            // 4. ��ϵǾ��ִ� List�� �ѷ��ְ� �ʹ�.
            // coachList ������ �ѷ�����.
            //for (int i = 0; i < coachList.Count; i++)
            //{
            //    Instantiate(coachIconFactory, UserListParent);
            //}
            //// playerList ������ �ѷ�����.
            //for (int i = 0; i < playerList.Count; i++)
            //{
            //    Instantiate(playerIconFactory, UserListParent);
            //}
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
        for (int i = 0; i < coachList.Count; i++)
        {
            Instantiate(coachIconFactory, UserListParent);
        }
        // playerList ������ �ѷ�����.
        for (int i = 0; i < playerList.Count; i++)
        {
            Instantiate(playerIconFactory, UserListParent);
        }
        //switch(order)
        //{
        //    case 1:
        //        Instantiate(coachIconFactory, UserListParent);
        //        break;

        //    case 2:
        //        Instantiate(playerIconFactory, UserListParent);
        //        break;
        //}
    }

    void SetAuthority(GameObject user, bool isCoach)
    {
        // ������ �Ϲ� �÷��̾ �����ߴٸ�
        if (isCoach == false)
            playerList.Add(user);
        // ������ ��ġ�� �����ߴٸ�
        else
            coachList.Add(user);
    }
}
