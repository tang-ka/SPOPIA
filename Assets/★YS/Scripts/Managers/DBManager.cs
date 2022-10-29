using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class DBManager : MonoBehaviour
{
    //public InputField dbTest;
    public string testDBid = "2F2D067A082E0E55";

    // �÷��̾� ����
    public PlayerLeaderboardEntry MyPlayFabInfo;
    public List<PlayerLeaderboardEntry> PlayFabUserList = new List<PlayerLeaderboardEntry>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void SetData()
    {
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "A", "1" }, { "B", "2" } }, Permission = UserDataPermission.Public};
        PlayFabClientAPI.UpdateUserData(request, (result) => print("�� ������ ���� �����ߴµ�?"), (error) => print("������ ���� �����ߴ٤�����"));
    }*/

    public void GetData()
    {
        var request = new GetUserDataRequest() { PlayFabId = testDBid };
        PlayFabClientAPI.GetUserData(request, (result) => print(result.Data["UserData"].Value), (error) => print("�� ������ �ҷ����� �����߾�"));
        PlayFabClientAPI.GetUserData(request, (result) => print(result.Data["TeamData"].Value), (error) => print("�� ������ �ҷ����� �����߾�"));
    }

    public void SaveJson()
    {
        UserData userData = new UserData();
        TeamData teamData = new TeamData();

        // testData
        userData.nickName = "tangka";
        userData.age = 27;
        userData.playerLevel = 4;
        userData.position = "CM";
        userData.teamName = "FC MTVS";
        userData.height = 175;
        userData.weight = 75;

        userData.goal = 10;
        userData.assist = 10;
        userData.matchCount = 5;

        teamData.memberNum = 13;
        teamData.region = "Pangyo";
        //teamData.levelerCount;
        teamData.formation = "4-4-2";

        teamData.goal = 23;
        teamData.lossGoal = 8;
        teamData.matchCount = 5;
        teamData.win = 4;
        teamData.lose = 0;
        teamData.draw = 1;

        // To Playfab
        Dictionary<string, string> dataDic = new Dictionary<string, string>();
        Dictionary<string, string> dataDic2 = new Dictionary<string, string>();
        dataDic.Add("UserData", JsonUtility.ToJson(userData));
        dataDic2.Add("TeamData", JsonUtility.ToJson(teamData));

        SetUserData(dataDic);
        SetUserData(dataDic2);
    }

    public void SetUserData(Dictionary<string, string> jsonData)
    {
        // Ŭ���̾�Ʈ��(�ڱ��ڽŸ� ���� ����, ���� ������ ���� �Ұ�)
        /*var request = new UpdateUserDataRequest() { Data = jsonData, Permission = UserDataPermission.Public };
        PlayFabClientAPI.UpdateUserData(request, (result) => print("�� ������ ���� �����ߴµ�?"), (error) => print("������ ���� �����ߴ٤�����"));*/
        // ������
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = "2F2D067A082E0E55", Data = jsonData, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("�� ������ ���� �����ߴµ�?"), (error) => print("������ ���� �����ߴ٤�����"));
    }

    public void GetLeaderboard(string myID)
    {
        PlayFabUserList.Clear();

        for (int i = 0; i < 10; i++)
        {
            var request = new GetLeaderboardRequest
            {
                StartPosition = i * 100,
                StatisticName = "IDInfo",
                MaxResultsCount = 100,
                ProfileConstraints = new PlayerProfileViewConstraints() { ShowDisplayName = true }
            };
            PlayFabClientAPI.GetLeaderboard(request, (result) =>
            {
                if (result.Leaderboard.Count == 0) return;
                for (int j = 0; j < result.Leaderboard.Count; j++)
                {
                    PlayFabUserList.Add(result.Leaderboard[j]);
                    if (result.Leaderboard[j].PlayFabId == myID) MyPlayFabInfo = result.Leaderboard[j];
                }
            },
            (error) => { });
        }
    }

    public void SetStat() // ���� ȸ��ID ����
    {
        var request = new UpdatePlayerStatisticsRequest { Statistics = new List<StatisticUpdate> { new StatisticUpdate { StatisticName = "IDInfo", Value = 0 } } };
        PlayFabClientAPI.UpdatePlayerStatistics(request, (result) => { }, (error) => print("�� �������"));
    }
}
