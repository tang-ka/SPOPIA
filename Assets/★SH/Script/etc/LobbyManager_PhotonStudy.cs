using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class LobbyManager_PhotonStudy : MonoBehaviourPunCallbacks
{
    // ���̸� InputField
    public InputField inputRoomName;
    // ��й�ȣ InputField
    public InputField inputPassword;
    // ���ο� InputField
    public InputField inputMaxPlayer;
    // ������ Button
    public Button btnJoin;
    // ����� Button
    public Button btnCreate;

    // ���� ������
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();

    // map Thumbnail
    public GameObject[] mapThumbs;

    // ����Ʈ Content
    public Transform trListContent;

    void Start()
    {
        // ���̸�(InputField)�� ����ɶ� ȣ��Ǵ� �Լ� ���
        inputRoomName.onValueChanged.AddListener(OnRoomNameValueChanged);
        // ���ο�(InputField)�� ����ɶ� ȣ��Ǵ� �Լ� ���
        inputMaxPlayer.onValueChanged.AddListener(OnMaxPlayerValueChanged);
    }

    public void OnRoomNameValueChanged(string s)
    {
        // ����
        btnJoin.interactable = s.Length > 0;
        // ����
        btnCreate.interactable = s.Length > 0 && inputMaxPlayer.text.Length > 0; ;
    }

    public void OnMaxPlayerValueChanged(string s)
    {
        // ����
        btnCreate.interactable = s.Length > 0 && inputRoomName.text.Length > 0; ;
    }

    // �����
    public void CreateRoom()
    {
        // ������ ����
        RoomOptions roomOption = new RoomOptions();

        // �ִ��ο� (0���̸� �ִ��ο�)
        roomOption.MaxPlayers = byte.Parse(inputMaxPlayer.text);

        // �� ��Ͽ� ���̳�? ������ �ʴ���?
        roomOption.IsVisible = true;

        // custom ������ ����
        Hashtable hash = new Hashtable();
        hash["desc"] = "���� �ʺ����̴�.!" + Random.Range(1, 1000);
        hash["map_id"] = Random.Range(0, mapThumbs.Length);
        hash["room_name"] = inputRoomName.text;
        //hash["password"] = inputPassword.text;
        roomOption.CustomRoomProperties = hash;

        // custom ������ �����ϴ� ����
        roomOption.CustomRoomPropertiesForLobby = new string[] { "desc", "map_id", "room_name", "password" };

        // �� ���� ��û(�ش� �ɼ��� �̿��ؼ�)
        PhotonNetwork.CreateRoom(inputRoomName.text + inputPassword.text, roomOption, TypedLobby.Default);
    }

    // �� ���� �Ϸ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    // �� ���� ����
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed, " + returnCode + ", " + message);
    }

    // ������ ��û
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(inputRoomName.text + inputPassword.text);
        // PhotonNetwork.JoinRoom               : ������ �濡 ����
        // PhotonNetwork.JoinOrCreateRoom       : ���̸��� �����ؼ� ������ �Ҷ�, �ش� �̸��� ����
        //                                        ���ٸ� �� �̸����� ���� ���� �� ����
        // PhotonNetwork.JoinRandomOrCreateRoom : �������� ������ �Ҷ�, ���ǿ� �´� ���� ���ٸ�
        //                                        ���� ���� ���� �� ����
        // PhotonNetwork.JoinRandomRoom         : ������ �� ���ڴ�
    }

    // �� ������ �������� �� ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("LeagueAreaScene");
        print("OnJoinedRoom");
    }

    // �� ���� ���н� ȣ��Ǵ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed" + returnCode + ", " + message);
    }

    // �濡 ���� ������ ����Ǹ� ȣ�� �Ǵ� �Լ� (�߰�/����/����)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        // �븮��Ʈ UI ��ü ����
        DeleteRoomListUI();
        // �븮��Ʈ ������ ������Ʈ
        UpdateRoomCache(roomList);
        // �븮��Ʈ UI ��ü ����
        CreateRoomListUI();
    }

    void DeleteRoomListUI()
    {
        foreach (Transform tr in trListContent)
        {
            Destroy(tr.gameObject);
        }
    }

    void UpdateRoomCache(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            // ����, ����
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                // ���࿡ �ش� ���� �����Ȱ��̶��
                if (roomList[i].RemovedFromList)
                {
                    // roomCache���� �ش� ������ ����
                    roomCache.Remove(roomList[i].Name);
                }
                // �׷��� �ʴٸ�
                else
                {
                    // ���� ����
                    roomCache[roomList[i].Name] = roomList[i];
                }
            }
            // �߰�
            else
            {
                roomCache[roomList[i].Name] = roomList[i];
            }
        }
    }

    public GameObject roomItemfactory;

    void CreateRoomListUI()
    {
        foreach (RoomInfo info in roomCache.Values)
        {
            // ������� �����.
            GameObject go = Instantiate(roomItemfactory, trListContent);
            // ������� ������ ���� (������ (0/0))
            RoomItem item = go.GetComponent<RoomItem>();
            //item.SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
            item.SetInfo(info);

            // roomItem ��ư�� Ŭ���Ǹ� ȣ��Ǵ� �Լ� ���
            item.onClickAction = SetRoomName;
            //item.onClickAction = (room) =>
            //{
            //    inputRoomName.text = room;
            //};

            string desc = (string)info.CustomProperties["desc"];
            int map_id = (int)info.CustomProperties["map_id"];
        }
    }

    // ���� Thumbnail id
    int prevMap_id = -1;

    void SetRoomName(string room, int map_id)
    {
        // ���̸� ����
        inputRoomName.text = room;

        // ���࿡ ���� �� Tumbnail�� Ȱ��ȭ�� �Ǿ��ִٸ�
        if (prevMap_id > -1)
        {
            // ���� �� Thumbnail�� ��Ȱ��ȭ
            mapThumbs[prevMap_id].SetActive(false);
        }

        // �� Thumbnail ����
        mapThumbs[map_id].SetActive(true);

        // ���� �� id ����
        prevMap_id = map_id;
    }
}
