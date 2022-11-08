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
    public string btnMapType;

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
        // 리그 목록 연동
        StartCoroutine(CreateLeagueList());
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
        DBManager.instance.SaveJsonLeagueData(leagueData, "LeagueData");
    }

    public void JoinLeague()
    {
        PhotonNetwork.JoinRoom(inputLeagueName.text);

        // 리그 데이터 받아오기
        DBManager.instance.GetData(DBManager.instance.testDBid2, "LeagueData");
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
        PhotonNetwork.LoadLevel(btnMapType);
        print("리그 진입에 성공했습니다.");
    }

    // 방 입장 실패시 호출되는 함수
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("리그 진입 실패" + returnCode + ", " + message);
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
    IEnumerator CreateLeagueList()
    {
        // 리그아이템 만든다.
        /*Button leagueItem = Resources.Load<Button>("YS/LeagueItem");
        leagueItem.transform.SetParent(contentTr, false);

        Text t = leagueItem.GetComponentInChildren<Text>();
        t.text = DBManager.instance.leagueInfo.leagueName;*/

        // DB에서 Parsing이 끝날 때까지 다음으로 안넘어가게끔!!!!!
        yield return new WaitUntil(() => DBManager.instance.isParsed == true);

        GameObject leagueItem = Instantiate(leagueItemFactory, contentTr);

        Text t = leagueItem.GetComponentInChildren<Text>();
        t.text = DBManager.instance.leagueInfo.leagueName;

        DBManager.instance.isParsed = false;
    }
}
