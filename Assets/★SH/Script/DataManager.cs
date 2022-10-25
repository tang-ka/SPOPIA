using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    // user info
    string nickName;
    int age;
    int playerLevel; // 0: normal, 2 : middle, 3: high, 4: university 5: K2, 6: K1
    string position;
    string teamName;
    int height;
    int weight;
    string profileImage;

    //int 
    //int backNumber;
    //string favoritePlayer;
    //Avatar

    // user record
    int goal;
    int assist;
    int matchCount;

    //int SPOPIAscore : 개인 전투력
    //recent5
}

public class TeamData
{
    // team info
    int memberNum;
    string region;
    int[] levelerCount = new int[5];
    string formation; // 4-4-2, 4-2-3-1

    // team record
    int goal;
    int lossGoal;
    int matchCount;
    int win;
    int lose;
    int draw;

    //int SPOPIAscore : 팀 전투력
    //recent5
}