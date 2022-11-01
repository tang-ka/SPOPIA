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

    string testDBid = "A45FE526BA86DD94"; // teamDataBase TEST
    string testDBid2 = "2F2D067A082E0E55"; // LeageDataBase TEST
    string testDBid3 = "8B9D85404288CD65"; // UserDataBase TEST -> �ʿ��Ѱ�?! ����ó�� ��������� �޾ƿ� �� �ִµ�
    string testDBid4 = "1F7F85444A2EE882"; // MapCustomDataBase TEST

    // �÷��̾� ����
    public PlayerLeaderboardEntry MyPlayFabInfo;
    public List<PlayerLeaderboardEntry> PlayFabUserList = new List<PlayerLeaderboardEntry>();

    // MapCustom ����Ʈ
    public ArrayJson arrayJson = new ArrayJson();

    // MapCustom�� ���� Object ����Ʈ
    public List<GameObject> createdObj = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // MapCustom ����Ʈ
        arrayJson.datas = new List<SaveJsonInfo>();
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
        //PlayFabClientAPI.GetUserData(request, (result) => print(result.Data["UserData"].Value), (error) => print("�� ������ �ҷ����� �����߾�"));
        PlayFabClientAPI.GetUserData(request, Parsing, (error) => print("�� ������ �ҷ����� �����߾�"));
        PlayFabClientAPI.GetUserData(request, (result) => print(result.Data["TeamData"].Value), (error) => print("�� ������ �ҷ����� �����߾�"));
    }

    void Parsing(GetUserDataResult result)
    {
        UserData myData = JsonUtility.FromJson<UserData>(result.Data["UserData"].Value.ToString());
        print(myData.nickName);
    }

    public void SaveJson(TeamData teamData, string key) // �� ������ ��, TeamData�� ������� TeamListDB�� �־��ִ� �κ�
    {
        // To Playfab
        //Dictionary<string, string> dataDic = new Dictionary<string, string>();
        Dictionary<string, string> dataDic2 = new Dictionary<string, string>();
        //dataDic.Add("UserData", JsonUtility.ToJson(userData));
        dataDic2.Add(key, JsonUtility.ToJson(teamData));

        //SetUserData(dataDic);
        SetUserData(dataDic2);
    }

    public void SaveJsonLeagueData(LeagueData leagueData, string key) // ���� ������ ��, LeagueData�� ������� LeagueListDB�� �־��ִ� �κ�
    {
        Dictionary<string, string> dataDic = new Dictionary<string, string>();
        dataDic.Add("LeagueData", JsonUtility.ToJson(leagueData));
        SetUserData(dataDic);
    }

    public void SaveTeamData(TeamData teamData, string key) // �� ������ ����
    {
        // ������
        var request = new PlayFab.AdminModels.UpdateUserDataRequest() { PlayFabId = testDBid, Data = new Dictionary<string, string>() { { key, JsonUtility.ToJson(teamData) } }, Permission = PlayFab.AdminModels.UserDataPermission.Public };
        PlayFabAdminAPI.UpdateUserData(request, (result) => print("�� ������ ���� �����ߴµ�?"), (error) => print("������ ���� �����ߴ٤�����"));
    }

    public void SaveJsonMapCustom(ArrayJson array, string key)
    {
        Dictionary<string, string> dataDic = new Dictionary<string, string>();
        dataDic.Add(key, JsonUtility.ToJson(array));
        SetMapData(dataDic);
    }

    public void SetUserData(Dictionary<string, string> jsonData)
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
