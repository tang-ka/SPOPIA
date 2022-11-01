using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public class SaveJsonInfo
{
    public GameObject go;
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
    // 싱글톤
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
    string testDBid3 = "8B9D85404288CD65"; // UserDataBase TEST -> 필요한가?! 지금처럼 리더보드로 받아올 수 있는데
    string testDBid4 = "1F7F85444A2EE882"; // MapCustomDataBase TEST

    // 플레이어 관리
    public PlayerLeaderboardEntry MyPlayFabInfo;
    public List<PlayerLeaderboardEntry> PlayFabUserList = new List<PlayerLeaderboardEntry>();

    // MapCustom 리스트
    public ArrayJson arrayJson = new ArrayJson();

    // MapCustom을 위한 Object 리스트
    public List<GameObject> createdObj = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // MapCustom 리스트
        arrayJson.datas = new List<SaveJsonInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void SetData()
    {
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "A", "1" }, { "B", "2" } }, Permission = UserDataPermission.Public};
        PlayFabClientAPI.UpdateUserData(request, (result) => print("올 데이터 저장 성공했는데?"), (error) => print("데이터 저장 실패했다ㅋㅋㅋ"));
    }*/

    public void GetData()
    {
        var request = new GetUserDataRequest() { PlayFabId = testDBid };
        //PlayFabClientAPI.GetUserData(request, (result) => print(result.Data["UserData"].Value), (error) => print("너 데이터 불러오기 실패했어"));
        PlayFabClientAPI.GetUserData(request, Parsing, (error) => print("너 데이터 불러오기 실패했어"));
        PlayFabClientAPI.GetUserData(request, (result) => print(result.Data["TeamData"].Value), (error) => print("너 데이터 불러오기 실패했어"));
    }

    void Parsing(GetUserDataResult result)
    {
        UserData myData = JsonUtility.FromJson<UserData>(result.Data["UserData"].Value.ToString());
        print(myData.nickName);
    }

    public void SaveJson(TeamData teamData, string key) // 팀 생성할 때, TeamData를 기반으로 TeamListDB에 넣어주는 부분
    {
        // To Playfab
        //Dictionary<string, string> dataDic = new Dictionary<string, string>();
        Dictionary<string, string> dataDic2 = new Dictionary<string, string>();
        //dataDic.Add("UserData", JsonUtility.ToJson(userData));
        dataDic2.Add(key, JsonUtility.ToJson(teamData));

        //SetUserData(dataDic);
        SetUserData(dataDic2);
    }

    public void SaveJsonLeagueData(LeagueData leagueData, string key) // 리그 생성할 때, LeagueData를 기반으로 LeagueListDB에 넣어주는 부분
    {
        Dictionary<string, string> dataDic = new Dictionary<string, string>();
        dataDic.Add("LeagueData", JsonUtility.ToJson(leagueData));
        SetUserData(dataDic);
    }

    public void SaveTeamData(TeamData teamData, string key) // 팀 데이터 수정
    {
        // 서버용
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid, Data = new Dictionary<string, string>() { { key, JsonUtility.ToJson(teamData) } }, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("올 데이터 저장 성공했는데?"), (error) => print("데이터 저장 실패했다ㅋㅋㅋ"));
    }

    public void SaveJsonMapCustom(ArrayJson array, string key)
    {
        Dictionary<string, string> dataDic = new Dictionary<string, string>();
        dataDic.Add(key, JsonUtility.ToJson(array));
        SetMapData(dataDic);
    }

    public void SetUserData(Dictionary<string, string> jsonData)
    {
        // 클라이언트용(자기자신만 수정 가능, 남의 데이터 수정 불가)
        /*var request = new UpdateUserDataRequest() { Data = jsonData, Permission = UserDataPermission.Public };
        PlayFabClientAPI.UpdateUserData(request, (result) => print("올 데이터 저장 성공했는데?"), (error) => print("데이터 저장 실패했다ㅋㅋㅋ"));*/
        // 서버용
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid, Data = jsonData, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("올 데이터 저장 성공했는데?"), (error) => print("데이터 저장 실패했다ㅋㅋㅋ"));
    }

    public void SetMapData(Dictionary<string, string> jsonData)
    {
        // 서버용
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid4, Data = jsonData, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("올 데이터 저장 성공했는데?"), (error) => print("데이터 저장 실패했다ㅋㅋㅋ"));
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

    public void SetStat() // 통계로 회원ID 저장
    {
        var request = new UpdatePlayerStatisticsRequest { Statistics = new List<StatisticUpdate> { new StatisticUpdate { StatisticName = "IDInfo", Value = 0 } } };
        PlayFabClientAPI.UpdatePlayerStatistics(request, (result) => { }, (error) => print("값 저장실패"));
    }
}
