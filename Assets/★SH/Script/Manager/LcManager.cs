using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine.EventSystems;

public class LcManager : MonoBehaviourPunCallbacks
{
    public InputField inputLeagueName;
    public InputField inputTeamNum;
    public InputField inputStartDate, inputEndDate;
    public string btnMapType, btnLeagueName;

    public Button btnCreateLeague;
    public Button btnJoinLeague;

    public GameObject leagueInfoPage;
    public Transform contentTr;

    private void Awake()
    {
        // 리그DB에서 리그들 불러오기
        DBManager.instance.GetData(DBManager.instance.testDBid2, "LeagueData");
    }

    void Start()
    {
        StartCoroutine(StartLeagueList());
    }

    IEnumerator StartLeagueList()
    {
        // DB에서 Parsing이 끝날 때까지 다음으로 안넘어가게끔!!!!!
        DBManager.instance.isParsed = false; // Parsing이 실행되게끔 false로 만들어주고 밑에서 함수를 실행시킨다.
        yield return new WaitUntil(() => DBManager.instance.isParsed == true);

        // 리그 목록 연동
        for (int i = 0; i < DBManager.instance.leagues.leagueDatas.Count; i++)
        {
            StartCoroutine(CreateLeagueList(i));
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inputLeagueName.text = "SPOPIA TEST";
            inputTeamNum.text = "4";
        }
    }

    public void OnClickCreateLeague()
    {
        RoomOptions leagueOption = new RoomOptions();
        leagueOption.MaxPlayers = 0;
        leagueOption.IsVisible = true;

        int teamNum = int.Parse(inputTeamNum.text);

        // 리그 정보 커스텀해서 넣고 싶다.
        // 1. 커스텀 정보 세팅
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["teamNum"] = teamNum;

        leagueOption.CustomRoomProperties = hash;

        // 2. 커스텀 정보를 공개하고 싶다.
        leagueOption.CustomRoomPropertiesForLobby = new string[] { "teamNum" };

        // 해당 옵선으로 리그(방)를 생성하고 싶다.
        PhotonNetwork.CreateRoom(inputLeagueName.text, leagueOption, TypedLobby.Default);

        // DB에 리그정보 셋팅
        LeagueData leagueData = new LeagueData();
        leagueData.leagueName = inputLeagueName.text;
        leagueData.teamNum = int.Parse(inputTeamNum.text);
        leagueData.startDate = inputStartDate.text;
        leagueData.endDate = inputEndDate.text;
        leagueData.mapType = btnMapType;

        // DB에 리그데이터 저장
        DBManager.instance.leagues.leagueDatas.Add(leagueData);
        DBManager.instance.SaveJsonLeagueData(DBManager.instance.leagues, "LeagueData");
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

        // 2. 커스텀 정보를 공개하고 싶다.
        leagueOption.CustomRoomPropertiesForLobby = new string[] { "teamNum" };

        // 해당 옵선으로 리그(방)를 생성하고 싶다.
        PhotonNetwork.CreateRoom(DBManager.instance.leagueInfo.leagueName, leagueOption, TypedLobby.Default);
    }

    public void JoinLeague()
    {
        // 해당 리그에 user가 1명이라도 존재한다면,
        // 그냥 Join
        if(btnLeagueName != "")
        {
            PhotonNetwork.JoinRoom(btnLeagueName);
        }
        // 1명도 존재하지 않는다면, = Fail이 된다면(콜백함수 OnJoinRoomFailed)
        // 방을 새로 생성!

        /*// 리그 데이터 받아오기
        DBManager.instance.GetData(DBManager.instance.testDBid2, "LeagueData");
        for(int i = 0; i < DBManager.instance.leagues.leagueDatas.Count; i++)
        {
            // 누른 리그 데이터를 리그 리스트에서 찾아서 leagueInfo에 넣어주기
            if(DBManager.instance.leagues.leagueDatas[i].leagueName == btnLeagueName)
            {
                DBManager.instance.leagueInfo = DBManager.instance.leagues.leagueDatas[i];

                break;
            }
        }*/
    }

    #region 콜백함수 (방생성 성공, 실패)
    // 방 생성 완료
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("해당 리그를 생성하였습니다.");
    }
    // 방 생성 실패
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("리그 생성 실패, " + returnCode + ", " + message);
    }
    #endregion

    #region 콜백함수 (방입장 성공, 실패)
    // 방 입장이 성공했을 때 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        // 리그 생성할 때 (생성할 때는 btnMapType을 받아옴)
        if(btnMapType != "")
        {
            PhotonNetwork.LoadLevel(btnMapType);
            print("리그 진입에 성공했습니다.");
        }
        /*else
        {
            // 리그 참가할 때 (참가할 때는 LeagueDB에 있는 MapType을 받아옴)
            PhotonNetwork.LoadLevel(DBManager.instance.leagueInfo.mapType);
            print("리그 진입에 성공했습니다.");
        }*/
    }

    // 방 입장 실패시 호출되는 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("리그 진입 실패" + returnCode + ", " + message);

        // 1명도 존재하지 않는다면, = Fail이 된다면(콜백함수 OnJoinRoomFailed)
        // 방을 새로 생성!
        CreateLeague();
    }
    #endregion

    public void LeagueInfoPage()
    {
        if (leagueInfoPage.activeSelf == false)
        {
            leagueInfoPage.SetActive(true);
        }
        else
        {
            leagueInfoPage.SetActive(false);
        }
    }

    public void SelectMapType()
    {
        btnMapType = EventSystem.current.currentSelectedGameObject.name;
    }

    // 리그 목록 연동(리그DB에 있는 데이터 받아와서 목록에 띄우기)
    public GameObject leagueItemFactory;
    IEnumerator CreateLeagueList(int idx)
    {
        // 리그아이템 만든다.
        /*Button leagueItem = Resources.Load<Button>("YS/LeagueItem");
        leagueItem.transform.SetParent(contentTr, false);

        Text t = leagueItem.GetComponentInChildren<Text>();
        t.text = DBManager.instance.leagueInfo.leagueName;*/

        // DB에서 Parsing이 끝날 때까지 다음으로 안넘어가게끔!!!!!
        //DBManager.instance.isParsed = false; // Parsing이 실행되게끔 false로 만들어주고 밑에서 함수를 실행시킨다.
        //yield return new WaitUntil(() => DBManager.instance.isParsed == true);

        GameObject leagueItem = Instantiate(leagueItemFactory, contentTr);

        Text t = leagueItem.GetComponentInChildren<Text>();
        t.text = DBManager.instance.leagues.leagueDatas[idx].leagueName;

        yield return null;
    }
}
