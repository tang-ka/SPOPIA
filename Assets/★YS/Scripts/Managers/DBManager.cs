using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveJsonInfo
{
    public string name;
    public Vector3 position;
    public Vector3 eulerAngle;
    public Vector3 localScale;
}

[System.Serializable]
public class ArrayJson
{
    public List<SaveJsonInfo> datas;
}

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

    // DataBase級
    public string testDBid = "A45FE526BA86DD94"; // teamDataBase TEST
    public string testDBid2 = "2F2D067A082E0E55"; // LeageDataBase TEST
    public string testDBid3 = "8B9D85404288CD65"; // Formation TEST
    public string testDBid4 = "1F7F85444A2EE882"; // MapCustomDataBase TEST

    // 巴傾戚嬢 淫軒
    public PlayerLeaderboardEntry MyPlayFabInfo;
    public List<PlayerLeaderboardEntry> PlayFabUserList = new List<PlayerLeaderboardEntry>();

    // MapCustom 軒什闘
    public ArrayJson arrayJson = new ArrayJson();

    // MapCustom聖 是廃 Object 軒什闘
    public List<GameObject> createdObj = new List<GameObject>();
    public List<GameObject> createdPrefab = new List<GameObject>();

    // MapDataLoad
    public ArrayJson mapData;
    public bool isEnter = false;
    bool isParsing = true; // parsing 魁
    bool isGetData = false; // 己 稽球澗 樟拭 級嬢哀 凶 廃腰幻 背爽檎 鞠奄 凶庚拭, 益 板稽澗 叔楳戚 照鞠惟 端滴

    // 稽益昔 吉 雌殿
    public bool isLogin = false;

    // UserData 識情
    public UserData myData;

    // LeagueData 識情
    public LeagueArray leagues;
    public LeagueData leagueInfo;
    public bool isParsed = false;

    // FormationData 識情
    public FormationArrayJson formationDatas;

    // ImageDB 淫恵
    // Entitytype
    public string entityType;

    // Start is called before the first frame update
    void Start()
    {
        // MapCustom 軒什闘
        arrayJson.datas = new List<SaveJsonInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        // 己 朕什賭 淫恵
        if(SceneManager.GetActiveScene().name == "YS_MapCustomScene" && isEnter == false)
        {
            if(isLogin == true)
            {
                LoadCustomMap();
            }
        }
        else if(SceneManager.GetActiveScene().name != "YS_MapCustomScene") // 朕什賭 樟聖 蟹亜檎 己 稽球研 亜管馬惟懐
        {
            isGetData = false;
        }
    }

    /*public void SetData()
    {
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "A", "1" }, { "B", "2" } }, Permission = UserDataPermission.Public};
        PlayFabClientAPI.UpdateUserData(request, (result) => print("臣 汽戚斗 煽舌 失因梅澗汽?"), (error) => print("汽戚斗 煽舌 叔鳶梅陥せせせ"));
    }*/

    public void GetData(string id, string key) // id澗 亜閃神壱 粛精 Data税 DBid, key澗 背雁 DBid税 徹葵
    {
        var request = new GetUserDataRequest() { PlayFabId = id };
        PlayFabClientAPI.GetUserData(request, (result) => Parsing(result, key), (error) => print("格 汽戚斗 災君神奄 叔鳶梅嬢"));
    }

    void Parsing(GetUserDataResult result, string key)
    {
        if(key == "MyData")
        {
            if(result.Data.ContainsKey(key) == false)
            {
                // 羨紗聖 馬檎, userdata研 災君人辞 重鋭User昔走 毒舘
                // 蒸生檎, AvatarScene生稽
                if (SceneManager.GetActiveScene().name == "LoginScene")
                {
                    PhotonNetwork.LoadLevel("AvatarCreateScene");
                }
            }
            else
            {
                myData = JsonUtility.FromJson<UserData>(result.Data[key].Value.ToString());
                
                // 赤生檎, SelectScene生稽
                if (SceneManager.GetActiveScene().name == "LoginScene")
                {
                    PhotonNetwork.LoadLevel("YS_SelectScene");
                }
            }
        }
        else if (key == "MapData")
        {
            if(isGetData == false)
            {
                mapData = JsonUtility.FromJson<ArrayJson>(result.Data[key].Value.ToString());
                isParsing = false;
                isGetData = true;
            }
        }
        else if (key == "LeagueData")
        {
            leagues = JsonUtility.FromJson<LeagueArray>(result.Data[key].Value.ToString());
        }
        else if (key == myData.teamName)
        {
            formationDatas = JsonUtility.FromJson<FormationArrayJson>(result.Data[key].Value.ToString());
        }

        isParsed = true;
    }

    public void SaveJsonTeamData(TeamData teamData, string key) // 得 持失拝 凶, TeamData研 奄鋼生稽 TeamListDB拭 隔嬢爽澗 採歳
    {
        Dictionary<string, string> dataDic = new Dictionary<string, string>();
        dataDic.Add(key, JsonUtility.ToJson(teamData));

        SetTeamData(dataDic);
    }

    public void SaveJsonLeagueData(LeagueArray leagueArray, string key) // 軒益 持失拝 凶, LeagueData研 奄鋼生稽 LeagueListDB拭 隔嬢爽澗 採歳
    {
        Dictionary<string, string> dataDic = new Dictionary<string, string>();
        dataDic.Add(key, JsonUtility.ToJson(leagueArray));
        SetLeagueData(dataDic);
    }

    public void SaveJsonMapCustom(ArrayJson array, string key)
    {
        Dictionary<string, string> dataDic = new Dictionary<string, string>();
        dataDic.Add(key, JsonUtility.ToJson(array));
        SetMapData(dataDic);
    }

    public void SaveJsonUser(UserData userData, string key) // 政煽 持失拝 凶, 蓄亜旋昔 UserData研 隔嬢爽澗 採歳
    {
        Dictionary<string, string> dataDic = new Dictionary<string, string>();
        dataDic.Add(key, JsonUtility.ToJson(userData));
        SetUserData(dataDic);
    }

    public void SaveJsonFormation(FormationArrayJson formationData, string key)
    {
        Dictionary<string, string> dataDic = new Dictionary<string, string>();
        dataDic.Add(key, JsonUtility.ToJson(formationData));
        SetFormation(dataDic);
    }

    public void UpdateTeamData(TeamData teamData, string key) // 得 汽戚斗 呪舛
    {
        // 辞獄遂
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid, Data = new Dictionary<string, string>() { { key, JsonUtility.ToJson(teamData) } }, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("臣 汽戚斗 煽舌 失因梅澗汽?"), (error) => print("汽戚斗 煽舌 叔鳶梅陥せせせ"));
    }

    public void UpdateLeagueData(LeagueData leagueData, string key) // 軒益 汽戚斗 呪舛
    {
        // 辞獄遂
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid2, Data = new Dictionary<string, string>() { { key, JsonUtility.ToJson(leagueData) } }, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("臣 汽戚斗 煽舌 失因梅澗汽?"), (error) => print("汽戚斗 煽舌 叔鳶梅陥せせせ"));
    }

    public void UpdateUserData(UserData userData, string key) // 政煽 汽戚斗 呪舛
    {
        // 辞獄遂
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = MyPlayFabInfo.PlayFabId, Data = new Dictionary<string, string>() { { key, JsonUtility.ToJson(userData) } }, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("臣 汽戚斗 煽舌 失因梅澗汽?"), (error) => print("汽戚斗 煽舌 叔鳶梅陥せせせ"));
    }

    public void SetUserData(Dictionary<string, string> userData)
    {
        // 辞獄遂
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = MyPlayFabInfo.PlayFabId, Data = userData, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("臣 汽戚斗 煽舌 失因梅澗汽?"), (error) => print("汽戚斗 煽舌 叔鳶梅陥せせせ"));
    }

    public void SetTeamData(Dictionary<string, string> jsonData)
    {
        // 適虞戚情闘遂(切奄切重幻 呪舛 亜管, 害税 汽戚斗 呪舛 災亜)
        /*var request = new UpdateUserDataRequest() { Data = jsonData, Permission = UserDataPermission.Public };
        PlayFabClientAPI.UpdateUserData(request, (result) => print("臣 汽戚斗 煽舌 失因梅澗汽?"), (error) => print("汽戚斗 煽舌 叔鳶梅陥せせせ"));*/
        // 辞獄遂
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid, Data = jsonData, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("臣 汽戚斗 煽舌 失因梅澗汽?"), (error) => print("汽戚斗 煽舌 叔鳶梅陥せせせ"));
    }

    public void SetMapData(Dictionary<string, string> jsonData)
    {
        // 辞獄遂
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid4, Data = jsonData, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("臣 汽戚斗 煽舌 失因梅澗汽?"), (error) => print("汽戚斗 煽舌 叔鳶梅陥せせせ"));
    }

    public void SetLeagueData(Dictionary<string, string> leagueData)
    {
        // 辞獄遂
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid2, Data = leagueData, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("臣 汽戚斗 煽舌 失因梅澗汽?"), (error) => print("汽戚斗 煽舌 叔鳶梅陥せせせ"));
    }

    public void SetFormation(Dictionary<string, string> formationData)
    {
        // 辞獄遂
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid3, Data = formationData, Permission = PlayFab.AdminModels.UserDataPermission.Public };
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

    // Custom Map 災君神奄
    public void LoadCustomMap()
    {
        // Json莫殿研 紫遂拝 呪 赤澗 汽戚斗 莫殿稽 痕発
        GetData(testDBid4, "MapData");

        if(isParsing == false)
        {
            for(int i = 0; i < mapData.datas.Count; i++)
            {
                SaveJsonInfo info = mapData.datas[i];

                LoadObject(info);
            }

            // 神崎詮闘研 陥 持失馬檎 魁
            isEnter = true;
            // parsing戚 亜管馬惟懐
            isParsing = true;
        }
    }

    public void LoadObject(SaveJsonInfo info)
    {
        // 神崎詮闘 持失
        GameObject obj = Instantiate(Resources.Load<GameObject>("YS/" + info.name));

        obj.transform.position = info.position;
        obj.transform.eulerAngles = info.eulerAngle;
        obj.transform.localScale = info.localScale;
    }
}
