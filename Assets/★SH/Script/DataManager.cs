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