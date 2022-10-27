using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class LobbyManager_PhotonStudy : MonoBehaviourPunCallbacks
{
    // 방이름 InputField
    public InputField inputRoomName;
    // 비밀번호 InputField
    public InputField inputPassword;
    // 총인원 InputField
    public InputField inputMaxPlayer;
    // 방참가 Button
    public Button btnJoin;
    // 방생성 Button
    public Button btnCreate;

    // 방의 정보들
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();

    // map Thumbnail
    public GameObject[] mapThumbs;

    // 리스트 Content
    public Transform trListContent;

    void Start()
    {
        // 방이름(InputField)이 변경될때 호출되는 함수 등록
        inputRoomName.onValueChanged.AddListener(OnRoomNameValueChanged);
        // 총인원(InputField)이 변경될때 호출되는 함수 등록
        inputMaxPlayer.onValueChanged.AddListener(OnMaxPlayerValueChanged);
    }

    public void OnRoomNameValueChanged(string s)
    {
        // 참가
        btnJoin.interactable = s.Length > 0;
        // 생성
        btnCreate.interactable = s.Length > 0 && inputMaxPlayer.text.Length > 0; ;
    }

    public void OnMaxPlayerValueChanged(string s)
    {
        // 생성
        btnCreate.interactable = s.Length > 0 && inputRoomName.text.Length > 0; ;
    }

    // 방생성
    public void CreateRoom()
    {
        // 방정보 세팅
        RoomOptions roomOption = new RoomOptions();

        // 최대인원 (0명이면 최대인원)
        roomOption.MaxPlayers = byte.Parse(inputMaxPlayer.text);

        // 룸 목록에 보이냐? 보이지 않느냐?
        roomOption.IsVisible = true;

        // custom 정보를 셋팅
        Hashtable hash = new Hashtable();
        hash["desc"] = "여긴 초보방이다.!" + Random.Range(1, 1000);
        hash["map_id"] = Random.Range(0, mapThumbs.Length);
        hash["room_name"] = inputRoomName.text;
        //hash["password"] = inputPassword.text;
        roomOption.CustomRoomProperties = hash;

        // custom 정보를 공개하는 설정
        roomOption.CustomRoomPropertiesForLobby = new string[] { "desc", "map_id", "room_name", "password" };

        // 방 생성 요청(해당 옵션을 이용해서)
        PhotonNetwork.CreateRoom(inputRoomName.text + inputPassword.text, roomOption, TypedLobby.Default);
    }

    // 방 생성 완료
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    // 방 생성 실패
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed, " + returnCode + ", " + message);
    }

    // 방입장 요청
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(inputRoomName.text + inputPassword.text);
        // PhotonNetwork.JoinRoom               : 선택한 방에 들어갈때
        // PhotonNetwork.JoinOrCreateRoom       : 방이름을 설정해서 들어가려고 할때, 해당 이름의 방이
        //                                        없다면 그 이름으로 방을 생성 후 입장
        // PhotonNetwork.JoinRandomOrCreateRoom : 랜덤방을 들어가려고 할때, 조건에 맞는 방이 없다면
        //                                        내가 방을 생성 후 입장
        // PhotonNetwork.JoinRandomRoom         : 랜덤한 방 들어가겠다
    }

    // 방 입장이 성공했을 때 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("LeagueAreaScene");
        print("OnJoinedRoom");
    }

    // 방 입장 실패시 호출되는 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed" + returnCode + ", " + message);
    }

    // 방에 대한 정보가 변경되면 호출 되는 함수 (추가/삭제/수정)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        // 룸리스트 UI 전체 삭제
        DeleteRoomListUI();
        // 룸리스트 정보를 업데이트
        UpdateRoomCache(roomList);
        // 룸리스트 UI 전체 생성
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
            // 수정, 삭제
            if (roomCache.ContainsKey(roomList[i].Name))
            {
                // 만약에 해당 룸이 삭제된것이라면
                if (roomList[i].RemovedFromList)
                {
                    // roomCache에서 해당 정보를 삭제
                    roomCache.Remove(roomList[i].Name);
                }
                // 그렇지 않다면
                else
                {
                    // 정보 수정
                    roomCache[roomList[i].Name] = roomList[i];
                }
            }
            // 추가
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
            // 룸아이템 만든다.
            GameObject go = Instantiate(roomItemfactory, trListContent);
            // 룸아이템 정보를 셋팅 (방제목 (0/0))
            RoomItem item = go.GetComponent<RoomItem>();
            //item.SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
            item.SetInfo(info);

            // roomItem 버튼이 클릭되면 호출되는 함수 등록
            item.onClickAction = SetRoomName;
            //item.onClickAction = (room) =>
            //{
            //    inputRoomName.text = room;
            //};

            string desc = (string)info.CustomProperties["desc"];
            int map_id = (int)info.CustomProperties["map_id"];
        }
    }

    // 이전 Thumbnail id
    int prevMap_id = -1;

    void SetRoomName(string room, int map_id)
    {
        // 룸이름 설정
        inputRoomName.text = room;

        // 만약에 이전 맵 Tumbnail이 활성화가 되어있다면
        if (prevMap_id > -1)
        {
            // 이전 맵 Thumbnail을 비활성화
            mapThumbs[prevMap_id].SetActive(false);
        }

        // 맵 Thumbnail 설정
        mapThumbs[map_id].SetActive(true);

        // 이전 맵 id 저장
        prevMap_id = map_id;
    }
}
