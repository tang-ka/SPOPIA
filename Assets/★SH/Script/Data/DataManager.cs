using Newtonsoft.Json.Linq;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    //int SPOPIAscore : 개인 전투력
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

    //int SPOPIAscore : 팀 전투력
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

    // Map관련
    public string mapType;
}

[System.Serializable]
public class LeagueArray
{
    public List<LeagueData> leagueDatas;
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

        // 가짜 LeagueData 생성 -> 그 안에 팀 데이터(teams) 생성
        fakeLeagueData = new LeagueData();
        fakeLeagueData.teams = new List<TeamData>();

        SetData();
    }

    public void SetData()
    {
        // 가짜 데이터
        //SetLeagueData();
        //SetFakeTeamData();
        //SetFakeUserData();

        // 실제 데이터
        SetRealTeamData();
        SetRealUserData();
    }

    void SetFakeTeamData()
    {
        TeamData temp;

        string[] teamsName = { "FC Robot", "FC Mech", "FC Archi", "FC Elec" };

        for (int i = 0; i < teamsName.Length; i++)
        {
            // 가짜 팀 데이터에 이름만 설정해준다.
            temp = new TeamData();
            temp.teamName = teamsName[i];

            temp.goal = i * 2;
            temp.lossGoal = i;
            temp.matchCount = i;
            temp.win = i;
            //temp.lose = i;
            //temp.draw = i;

            // 가짜 팀 데이터 안에 유저 데이터(users) 생성
            temp.users = new List<UserData>();

            // 가짜 리그 데이터의 teams에 가짜 팀데이터 추가
            fakeLeagueData.teams.Add(temp);

            // DB에 저장
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
        // 처음에 초기화 해주기(안해주면 계속 Add 됨)
        fakeLeagueData.teams.Clear();

        for (int i = 0; i < DBManager.instance.leagueInfo.teams.Count; i++)
        {
            TeamData temp = new TeamData();
            temp = DBManager.instance.leagueInfo.teams[i];

            // 팀 데이터 안에 유저 데이터(users) 생성
            //temp.users = new List<UserData>();

            // 리그 데이터의 teams에 팀데이터 추가
            fakeLeagueData.teams.Add(temp);

            // DB에 저장
            string s = i.ToString();
            DBManager.instance.SaveJsonTeamData(temp, s);
        }
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
        for (int i = 0; i < fakeLeagueData.teams.Count; i++)
        {
            for (int j = 0; j < fakeLeagueData.teams[i].users.Count; j++)
            {
                UserData temp = new UserData();
                temp.nickName = fakeLeagueData.teams[i].users[j].nickName;

                temp.goal = fakeLeagueData.teams[i].users[j].goal;
                temp.assist = fakeLeagueData.teams[i].users[j].assist;
                temp.matchCount = fakeLeagueData.teams[i].users[j].matchCount;
            }
        }
    }

    void SetLeagueData()
    {
        DBManager.instance.GetData(DBManager.instance.testDBid2, "LeagueData");
    }

    /// <summary>
    /// 가짜 팀 데이터의 이름을 반환해준다.
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
    /// 가짜 팀 데이터 리스트를 반환해 준다.
    /// </summary>
    /// <returns></returns>
    public List<TeamData> GetTeamDataList()
    {
        return fakeLeagueData.teams;
    }

    /// <summary>
    /// 가짜 유저 데이터 리스트를 반환해 준다.
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