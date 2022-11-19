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
    // �̱���
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

    // DataBase��
    public string testDBid = "A45FE526BA86DD94"; // teamDataBase TEST
    public string testDBid2 = "2F2D067A082E0E55"; // LeageDataBase TEST
    public string testDBid3 = "8B9D85404288CD65"; // Formation TEST
    public string testDBid4 = "1F7F85444A2EE882"; // MapCustomDataBase TEST

    // �÷��̾� ����
    public PlayerLeaderboardEntry MyPlayFabInfo;
    public List<PlayerLeaderboardEntry> PlayFabUserList = new List<PlayerLeaderboardEntry>();

    // MapCustom ����Ʈ
    public ArrayJson arrayJson = new ArrayJson();

    // MapCustom�� ���� Object ����Ʈ
    public List<GameObject> createdObj = new List<GameObject>();
    public List<GameObject> createdPrefab = new List<GameObject>();

    // MapDataLoad
    public ArrayJson mapData;
    public bool isEnter = false;
    bool isParsing = true; // parsing ��
    bool isGetData = false; // �� �ε�� ���� �� �� �ѹ��� ���ָ� �Ǳ� ������, �� �ķδ� ������ �ȵǰ� üũ

    // �α��� �� ����
    public bool isLogin = false;

    // UserData ����
    public UserData myData;

    // LeagueData ����
    public LeagueArray leagues;
    public LeagueData leagueInfo;
    public bool isParsed = false;

    // FormationData ����
    public FormationArrayJson formationDatas;

    // Start is called before the first frame update
    void Start()
    {
        // MapCustom ����Ʈ
        arrayJson.datas = new List<SaveJsonInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        // �� Ŀ���� ����
        if(SceneManager.GetActiveScene().name == "YS_MapCustomScene" && isEnter == false)
        {
            if(isLogin == true)
            {
                LoadCustomMap();
            }
        }
        else if(SceneManager.GetActiveScene().name != "YS_MapCustomScene") // Ŀ���� ���� ������ �� �ε带 �����ϰԲ�
        {
            isGetData = false;
        }
    }

    /*public void SetData()
    {
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "A", "1" }, { "B", "2" } }, Permission = UserDataPermission.Public};
        PlayFabClientAPI.UpdateUserData(request, (result) => print("�� ������ ���� �����ߴµ�?"), (error) => print("������ ���� �����ߴ٤�����"));
    }*/

    public void GetData(string id, string key) // id�� �������� ���� Data�� DBid, key�� �ش� DBid�� Ű��
    {
        var request = new GetUserDataRequest() { PlayFabId = id };
        PlayFabClientAPI.GetUserData(request, (result) => Parsing(result, key), (error) => print("�� ������ �ҷ����� �����߾�"));
    }

    void Parsing(GetUserDataResult result, string key)
    {
        if(key == "MyData")
        {
            if(result.Data.ContainsKey(key) == false)
            {
                // ������ �ϸ�, userdata�� �ҷ��ͼ� �ű�User���� �Ǵ�
                // ������, AvatarScene����
                if (SceneManager.GetActiveScene().name == "LoginScene")
                {
                    PhotonNetwork.LoadLevel("AvatarCreateScene");
                }
            }
            else
            {
                myData = JsonUtility.FromJson<UserData>(result.Data[key].Value.ToString());
                
                // ������, SelectScene����
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

    public void SaveJsonTeamData(TeamData teamData, string key) // �� ������ ��, TeamData�� ������� TeamListDB�� �־��ִ� �κ�
    {
        Dictionary<string, string> dataDic = new Dictionary<string, string>();
        dataDic.Add(key, JsonUtility.ToJson(teamData));

        SetTeamData(dataDic);
    }

    public void SaveJsonLeagueData(LeagueArray leagueArray, string key) // ���� ������ ��, LeagueData�� ������� LeagueListDB�� �־��ִ� �κ�
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

    public void SaveJsonUser(UserData userData, string key) // ���� ������ ��, �߰����� UserData�� �־��ִ� �κ�
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

    public void UpdateTeamData(TeamData teamData, string key) // �� ������ ����
    {
        // ������
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid, Data = new Dictionary<string, string>() { { key, JsonUtility.ToJson(teamData) } }, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("�� ������ ���� �����ߴµ�?"), (error) => print("������ ���� �����ߴ٤�����"));
    }

    public void UpdateLeagueData(LeagueData leagueData, string key) // ���� ������ ����
    {
        // ������
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid2, Data = new Dictionary<string, string>() { { key, JsonUtility.ToJson(leagueData) } }, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("�� ������ ���� �����ߴµ�?"), (error) => print("������ ���� �����ߴ٤�����"));
    }

    public void UpdateUserData(UserData userData, string key) // ���� ������ ����
    {

    }

    public void SetUserData(Dictionary<string, string> userData)
    {
        // ������
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = MyPlayFabInfo.PlayFabId, Data = userData, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("�� ������ ���� �����ߴµ�?"), (error) => print("������ ���� �����ߴ٤�����"));
    }

    public void SetTeamData(Dictionary<string, string> jsonData)
    {
        // Ŭ���̾�Ʈ��(�ڱ��ڽŸ� ���� ����, ���� ������ ���� �Ұ�)
        /*var request = new UpdateUserDataRequest() { Data = jsonData, Permission = UserDataPermission.Public };
        PlayFabClientAPI.UpdateUserData(request, (result) => print("�� ������ ���� �����ߴµ�?"), (error) => print("������ ���� �����ߴ٤�����"));*/
        // ������
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid, Data = jsonData, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("�� ������ ���� �����ߴµ�?"), (error) => print("������ ���� �����ߴ٤�����"));
    }

    public void SetMapData(Dictionary<string, string> jsonData)
    {
        // ������
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid4, Data = jsonData, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("�� ������ ���� �����ߴµ�?"), (error) => print("������ ���� �����ߴ٤�����"));
    }

    public void SetLeagueData(Dictionary<string, string> leagueData)
    {
        // ������
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid2, Data = leagueData, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("�� ������ ���� �����ߴµ�?"), (error) => print("������ ���� �����ߴ٤�����"));
    }

    public void SetFormation(Dictionary<string, string> formationData)
    {
        // ������
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid3, Data = formationData, Permission = PlayFab.AdminModels.UserDataPermission.Public };
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

    // Custom Map �ҷ�����
    public void LoadCustomMap()
    {
        // Json���¸� ����� �� �ִ� ������ ���·� ��ȯ
        GetData(testDBid4, "MapData");

        if(isParsing == false)
        {
            for(int i = 0; i < mapData.datas.Count; i++)
            {
                SaveJsonInfo info = mapData.datas[i];

                LoadObject(info);
            }

            // ������Ʈ�� �� �����ϸ� ��
            isEnter = true;
            // parsing�� �����ϰԲ�
            isParsing = true;
        }
    }

    public void LoadObject(SaveJsonInfo info)
    {
        // ������Ʈ ����
        GameObject obj = Instantiate(Resources.Load<GameObject>("YS/" + info.name));

        obj.transform.position = info.position;
        obj.transform.eulerAngles = info.eulerAngle;
        obj.transform.localScale = info.localScale;
    }
}
