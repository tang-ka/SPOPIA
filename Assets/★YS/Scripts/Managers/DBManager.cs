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

    // DataBase들
    public string testDBid = "A45FE526BA86DD94"; // teamDataBase TEST
    public string testDBid2 = "2F2D067A082E0E55"; // LeageDataBase TEST
    public string testDBid3 = "8B9D85404288CD65"; // Formation TEST
    public string testDBid4 = "1F7F85444A2EE882"; // MapCustomDataBase TEST

    // 플레이어 관리
    public PlayerLeaderboardEntry MyPlayFabInfo;
    public List<PlayerLeaderboardEntry> PlayFabUserList = new List<PlayerLeaderboardEntry>();

    // MapCustom 리스트
    public ArrayJson arrayJson = new ArrayJson();

    // MapCustom을 위한 Object 리스트
    public List<GameObject> createdObj = new List<GameObject>();
    public List<GameObject> createdPrefab = new List<GameObject>();

    // MapDataLoad
    public ArrayJson mapData;
    public bool isEnter = false;
    bool isParsing = true; // parsing 끝
    bool isGetData = false; // 맵 로드는 씬에 들어갈 때 한번만 해주면 되기 때문에, 그 후로는 실행이 안되게 체크

    // 로그인 된 상태
    public bool isLogin = false;

    // UserData 선언
    public UserData myData;

    // LeagueData 선언
    public LeagueArray leagues;
    public LeagueData leagueInfo;
    public bool isParsed = false;

    // FormationData 선언
    public FormationArrayJson formationDatas;

    // Start is called before the first frame update
    void Start()
    {
        // MapCustom 리스트
        arrayJson.datas = new List<SaveJsonInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        // 맵 커스텀 관련
        if(SceneManager.GetActiveScene().name == "YS_MapCustomScene" && isEnter == false)
        {
            if(isLogin == true)
            {
                LoadCustomMap();
            }
        }
        else if(SceneManager.GetActiveScene().name != "YS_MapCustomScene") // 커스텀 씬을 나가면 맵 로드를 가능하게끔
        {
            isGetData = false;
        }
    }

    /*public void SetData()
    {
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "A", "1" }, { "B", "2" } }, Permission = UserDataPermission.Public};
        PlayFabClientAPI.UpdateUserData(request, (result) => print("올 데이터 저장 성공했는데?"), (error) => print("데이터 저장 실패했다ㅋㅋㅋ"));
    }*/

    public void GetData(string id, string key) // id는 가져오고 싶은 Data의 DBid, key는 해당 DBid의 키값
    {
        var request = new GetUserDataRequest() { PlayFabId = id };
        PlayFabClientAPI.GetUserData(request, (result) => Parsing(result, key), (error) => print("너 데이터 불러오기 실패했어"));
    }

    void Parsing(GetUserDataResult result, string key)
    {
        if(key == "MyData")
        {
            if(result.Data.ContainsKey(key) == false)
            {
                // 접속을 하면, userdata를 불러와서 신규User인지 판단
                // 없으면, AvatarScene으로
                if (SceneManager.GetActiveScene().name == "LoginScene")
                {
                    PhotonNetwork.LoadLevel("AvatarCreateScene");
                }
            }
            else
            {
                myData = JsonUtility.FromJson<UserData>(result.Data[key].Value.ToString());
                
                // 있으면, SelectScene으로
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

    public void SaveJsonTeamData(TeamData teamData, string key) // 팀 생성할 때, TeamData를 기반으로 TeamListDB에 넣어주는 부분
    {
        Dictionary<string, string> dataDic = new Dictionary<string, string>();
        dataDic.Add(key, JsonUtility.ToJson(teamData));

        SetTeamData(dataDic);
    }

    public void SaveJsonLeagueData(LeagueArray leagueArray, string key) // 리그 생성할 때, LeagueData를 기반으로 LeagueListDB에 넣어주는 부분
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

    public void SaveJsonUser(UserData userData, string key) // 유저 생성할 때, 추가적인 UserData를 넣어주는 부분
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

    public void UpdateTeamData(TeamData teamData, string key) // 팀 데이터 수정
    {
        // 서버용
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid, Data = new Dictionary<string, string>() { { key, JsonUtility.ToJson(teamData) } }, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("올 데이터 저장 성공했는데?"), (error) => print("데이터 저장 실패했다ㅋㅋㅋ"));
    }

    public void UpdateLeagueData(LeagueData leagueData, string key) // 리그 데이터 수정
    {
        // 서버용
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid2, Data = new Dictionary<string, string>() { { key, JsonUtility.ToJson(leagueData) } }, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("올 데이터 저장 성공했는데?"), (error) => print("데이터 저장 실패했다ㅋㅋㅋ"));
    }

    public void UpdateUserData(UserData userData, string key) // 유저 데이터 수정
    {

    }

    public void SetUserData(Dictionary<string, string> userData)
    {
        // 서버용
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = MyPlayFabInfo.PlayFabId, Data = userData, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("올 데이터 저장 성공했는데?"), (error) => print("데이터 저장 실패했다ㅋㅋㅋ"));
    }

    public void SetTeamData(Dictionary<string, string> jsonData)
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

    public void SetLeagueData(Dictionary<string, string> leagueData)
    {
        // 서버용
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid2, Data = leagueData, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("올 데이터 저장 성공했는데?"), (error) => print("데이터 저장 실패했다ㅋㅋㅋ"));
    }

    public void SetFormation(Dictionary<string, string> formationData)
    {
        // 서버용
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid3, Data = formationData, Permission = PlayFab.AdminModels.UserDataPermission.Public };
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

    // Custom Map 불러오기
    public void LoadCustomMap()
    {
        // Json형태를 사용할 수 있는 데이터 형태로 변환
        GetData(testDBid4, "MapData");

        if(isParsing == false)
        {
            for(int i = 0; i < mapData.datas.Count; i++)
            {
                SaveJsonInfo info = mapData.datas[i];

                LoadObject(info);
            }

            // 오브젝트를 다 생성하면 끝
            isEnter = true;
            // parsing이 가능하게끔
            isParsing = true;
        }
    }

    public void LoadObject(SaveJsonInfo info)
    {
        // 오브젝트 생성
        GameObject obj = Instantiate(Resources.Load<GameObject>("YS/" + info.name));

        obj.transform.position = info.position;
        obj.transform.eulerAngles = info.eulerAngle;
        obj.transform.localScale = info.localScale;
    }
}
