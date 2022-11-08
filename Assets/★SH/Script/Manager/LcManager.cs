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
        // ����DB���� ���׵� �ҷ�����
        DBManager.instance.GetData(DBManager.instance.testDBid2, "LeagueData");
    }

    void Start()
    {
        // ���� ��� ����
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

        // ���� ���� Ŀ�����ؼ� �ְ� �ʹ�.
        // 1. Ŀ���� ���� ����
        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["teamNum"] = teamNum;

        leagueOption.CustomRoomProperties = hash;

        // 2. Ŀ���� ������ �����ϰ� �ʹ�.
        leagueOption.CustomRoomPropertiesForLobby = new string[] { "teamNum" };

        // �ش� �ɼ����� ����(��)�� �����ϰ� �ʹ�.
        PhotonNetwork.CreateRoom(inputLeagueName.text, leagueOption, TypedLobby.Default);

        // DB�� �������� ����
        LeagueData leagueData = new LeagueData();
        leagueData.leagueName = inputLeagueName.text;
        leagueData.teamNum = int.Parse(inputTeamNum.text);
        leagueData.startDate = inputStartDate.text;
        leagueData.endDate = inputEndDate.text;
        leagueData.mapType = btnMapType;

        // DB�� ���׵����� ����
        DBManager.instance.SaveJsonLeagueData(leagueData, "LeagueData");
    }

    public void JoinLeague()
    {
        PhotonNetwork.JoinRoom(inputLeagueName.text);

        // ���� ������ �޾ƿ���
        DBManager.instance.GetData(DBManager.instance.testDBid2, "LeagueData");
    }

    #region �ݹ��Լ� (����� ����, ����)
    // �� ���� �Ϸ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("�ش� ���׸� �����Ͽ����ϴ�.");
    }
    // �� ���� ����
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("���� ���� ����, " + returnCode + ", " + message);
    }
    #endregion

    #region �ݹ��Լ� (������ ����, ����)
    // �� ������ �������� �� ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(btnMapType);
        print("���� ���Կ� �����߽��ϴ�.");
    }

    // �� ���� ���н� ȣ��Ǵ� �Լ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("���� ���� ����" + returnCode + ", " + message);
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

    // ���� ��� ����(����DB�� �ִ� ������ �޾ƿͼ� ��Ͽ� ����)
    public GameObject leagueItemFactory;
    IEnumerator CreateLeagueList()
    {
        // ���׾����� �����.
        /*Button leagueItem = Resources.Load<Button>("YS/LeagueItem");
        leagueItem.transform.SetParent(contentTr, false);

        Text t = leagueItem.GetComponentInChildren<Text>();
        t.text = DBManager.instance.leagueInfo.leagueName;*/

        // DB���� Parsing�� ���� ������ �������� �ȳѾ�Բ�!!!!!
        yield return new WaitUntil(() => DBManager.instance.isParsed == true);

        GameObject leagueItem = Instantiate(leagueItemFactory, contentTr);

        Text t = leagueItem.GetComponentInChildren<Text>();
        t.text = DBManager.instance.leagueInfo.leagueName;

        DBManager.instance.isParsed = false;
    }
}
