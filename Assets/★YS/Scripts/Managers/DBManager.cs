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
    // 縮越宕
    public static DBManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    string testDBid = "A45FE526BA86DD94"; // teamDataBase TEST
    string testDBid2 = "2F2D067A082E0E55"; // LeageDataBase TEST
    string testDBid3 = "8B9D85404288CD65"; // UserDataBase TEST -> 琶推廃亜?! 走榎坦軍 軒希左球稽 閤焼臣 呪 赤澗汽

    // 巴傾戚嬢 淫軒
    public PlayerLeaderboardEntry MyPlayFabInfo;
    public List<PlayerLeaderboardEntry> PlayFabUserList = new List<PlayerLeaderboardEntry>();

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
        PlayFabClientAPI.UpdateUserData(request, (result) => print("臣 汽戚斗 煽舌 失因梅澗汽?"), (error) => print("汽戚斗 煽舌 叔鳶梅陥せせせ"));
    }*/

    public void GetData()
    {
        var request = new GetUserDataRequest() { PlayFabId = testDBid };
        //PlayFabClientAPI.GetUserData(request, (result) => print(result.Data["UserData"].Value), (error) => print("格 汽戚斗 災君神奄 叔鳶梅嬢"));
        PlayFabClientAPI.GetUserData(request, Parsing, (error) => print("格 汽戚斗 災君神奄 叔鳶梅嬢"));
        PlayFabClientAPI.GetUserData(request, (result) => print(result.Data["TeamData"].Value), (error) => print("格 汽戚斗 災君神奄 叔鳶梅嬢"));
    }

    void Parsing(GetUserDataResult result)
    {
        UserData myData = JsonUtility.FromJson<UserData>(result.Data["UserData"].Value.ToString());
        print(myData.nickName);
    }

    public void SaveJson(TeamData teamData, string key) // 得 持失拝 凶, TeamData研 奄鋼生稽 TeamListDB拭 隔嬢爽澗 採歳
    {
        // To Playfab
        //Dictionary<string, string> dataDic = new Dictionary<string, string>();
        Dictionary<string, string> dataDic2 = new Dictionary<string, string>();
        //dataDic.Add("UserData", JsonUtility.ToJson(userData));
        dataDic2.Add(key, JsonUtility.ToJson(teamData));

        //SetUserData(dataDic);
        SetUserData(dataDic2);
    }

    public void SaveJsonLeagueData(LeagueData leagueData, string key) // 軒益 持失拝 凶, LeagueData研 奄鋼生稽 LeagueListDB拭 隔嬢爽澗 採歳
    {
        Dictionary<string, string> dataDic = new Dictionary<string, string>();
        dataDic.Add("LeagueData", JsonUtility.ToJson(leagueData));
        SetUserData(dataDic);
    }

    public void SaveTeamData(TeamData teamData, string key) // 得 汽戚斗 呪舛
    {
        // 辞獄遂
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid, Data = new Dictionary<string, string>() { { key, JsonUtility.ToJson(teamData) } }, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("臣 汽戚斗 煽舌 失因梅澗汽?"), (error) => print("汽戚斗 煽舌 叔鳶梅陥せせせ"));
    }

    public void SetUserData(Dictionary<string, string> jsonData)
    {
        // 適虞戚情闘遂(切奄切重幻 呪舛 亜管, 害税 汽戚斗 呪舛 災亜)
        /*var request = new UpdateUserDataRequest() { Data = jsonData, Permission = UserDataPermission.Public };
        PlayFabClientAPI.UpdateUserData(request, (result) => print("臣 汽戚斗 煽舌 失因梅澗汽?"), (error) => print("汽戚斗 煽舌 叔鳶梅陥せせせ"));*/
        // 辞獄遂
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid, Data = jsonData, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("臣 汽戚斗 煽舌 失因梅澗汽?"), (error) => print("汽戚斗 煽舌 叔鳶梅陥せせせ"));
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

    public void SetStat() // 搭域稽 噺据ID 煽舌
    {
        var request = new UpdatePlayerStatisticsRequest { Statistics = new List<StatisticUpdate> { new StatisticUpdate { StatisticName = "IDInfo", Value = 0 } } };
        PlayFabClientAPI.UpdatePlayerStatistics(request, (result) => { }, (error) => print("葵 煽舌叔鳶"));
    }
}
