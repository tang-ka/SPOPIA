using Newtonsoft.Json.Linq;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public int serverID;
    // user info
    public string nickName;
    public int age;
    public int playerLevel; // 0: normal, 1: middle, 2: high, 3: university, 4: over
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
    public int[] levelerCount = new int[5] { 0, 0, 9, 4, 0 };
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
        
        SetTeamData();
    }

    void SetTeamData()
    {
        SetFakeTeamData();
    }

    void SetFakeTeamData()
    {
        TeamData temp;

        string[] teamsName = { "FC XR", "FC AI", "FC Net", "FC Cre", "FC Tangka" };

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

            // 가짜 리그 데이터의 teams에 가짜 팀데이터 추가
            fakeLeagueData.teams.Add(temp);
        }
    }

    void SetRealTeamData()
    {
        
    }

    public List<string> GetTeamsNames()
    {
        List<string> teamNames = new List<string>();

        for (int i = 0; i < fakeLeagueData.teams.Count; i++)
        {
            teamNames.Add(fakeLeagueData.teams[i].teamName);
        }
        
        return teamNames;
    }

    public List<TeamData> GetTeamDataList()
    {
        return fakeLeagueData.teams;
    }
}