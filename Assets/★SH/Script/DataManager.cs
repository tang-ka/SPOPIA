using Newtonsoft.Json.Linq;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[System.Serializable]
public class UserData
{
    //public int serverID;
    // user info
    public int avatarIdx;
    public string nickName;
    public int age;
    //public int playerLevel; // 0: normal, 1: middle, 2: high, 3: university, 4: over
    public string position;
    public string teamName;
    public int height;
    public int weight;
    //string profileImage;

    //int 
    //int backNumber;
    //string favoritePlayer;
    //Avatar

    // user record
    public int goal;
    public int assist;
    public int matchCount;
    public int goalRank;
    public int assistRank;

    //int SPOPIAscore : ���� ������
    //recent5
}

[System.Serializable]
public class TeamData
{
    // team info
    public string teamName;
    public int memberNum;
    public List<UserData> users;
    public string region; // league
    //public int[] levelerCount = new int[5] { 0, 0, 9, 4, 0 };
    public string formation; // 4-4-2, 4-2-3-1

    // team record
    public int goal;
    public int lossGoal;
    public int matchCount;
    public int win;
    public int lose;
    public int draw;
    public int rank;
    public int points;

    //int SPOPIAscore : �� ������
    //recent5
}

[System.Serializable]
public class LeagueData
{
    public string leagueName;
    public int teamNum;
    public List<TeamData> teams;

    public string startDate;
    public string endDate;
    public bool isfinished;

    // Map����
    public int mapType;
}

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [SerializeField]
    private LeagueData fakeLeagueData;
    private LeagueData realLeagueData;

    private void Awake()
    {
        instance = this;

        // ��¥ LeagueData ���� -> �� �ȿ� �� ������(teams) ����
        fakeLeagueData = new LeagueData();
        fakeLeagueData.teams = new List<TeamData>();

        SetData();
    }

    void SetData()
    {
        SetFakeTeamData();
        SetFakeUserData();
    }

    void SetFakeTeamData()
    {
        TeamData temp;

        string[] teamsName = { "FC Robot", "FC Mech", "FC Archi", "FC Elec" };

        for (int i = 0; i < teamsName.Length; i++)
        {
            // ��¥ �� �����Ϳ� �̸��� �������ش�.
            temp = new TeamData();
            temp.teamName = teamsName[i];

            temp.goal = i * 2;
            temp.lossGoal = i;
            temp.matchCount = i;
            temp.win = i;
            //temp.lose = i;
            //temp.draw = i;

            // ��¥ �� ������ �ȿ� ���� ������(users) ����
            temp.users = new List<UserData>();

            // ��¥ ���� �������� teams�� ��¥ �������� �߰�
            fakeLeagueData.teams.Add(temp);

            // DB�� ����
            //string s = i.ToString();
            //DBManager.instance.SaveJsonTeamData(temp, s);
        }

        //for (int i = 0; i < fakeLeagueData.teams.Count; i++)
        //{
        //    fakeLeagueData.teams[i].users = new List<UserData>();
        //}
    }

    void SetRealTeamData()
    {

    }
    
    void SetFakeUserData()
    {
        UserData temp;

        string[] usersName = new string[1];
        for (int i = 0; i < usersName.Length; i++)
        {
            usersName[i] = "User_" + i;
        }

        for (int i = 0; i < fakeLeagueData.teams.Count; i++)
        {
            for (int j = 0; j < usersName.Length; j++)
            {
                temp = new UserData();
                temp.nickName = fakeLeagueData.teams[i].teamName + "_" + usersName[j];

                temp.goal = i;
                temp.assist = 4 - i;
                temp.matchCount = 1;

                fakeLeagueData.teams[i].users.Add(temp);
            }
        }
    }

    void SetRealUserData()
    {

    }

    void SetLeagueData()
    {
        DBManager.instance.GetData("2F2D067A082E0E55", "LeagueData");
    }

    /// <summary>
    /// ��¥ �� �������� �̸��� ��ȯ���ش�.
    /// </summary>
    /// <returns></returns>
    public List<string> GetTeamsNames()
    {
        List<string> teamNames = new List<string>();

        for (int i = 0; i < fakeLeagueData.teams.Count; i++)
        {
            teamNames.Add(fakeLeagueData.teams[i].teamName);
        }

        return teamNames;
    }

    /// <summary>
    /// ��¥ �� ������ ����Ʈ�� ��ȯ�� �ش�.
    /// </summary>
    /// <returns></returns>
    public List<TeamData> GetTeamDataList()
    {
        return fakeLeagueData.teams;
    }

    /// <summary>
    /// ��¥ ���� ������ ����Ʈ�� ��ȯ�� �ش�.
    /// </summary>
    public List<UserData> GetUserDataList()
    {
        List<UserData> userDataList = new List<UserData>();

        for (int i = 0; i < fakeLeagueData.teams.Count; i++)
        {
            for (int j = 0; j < fakeLeagueData.teams[i].users.Count; j++)
            {
                userDataList.Add(fakeLeagueData.teams[i].users[j]);
            }
        }

        return userDataList;
    }
}