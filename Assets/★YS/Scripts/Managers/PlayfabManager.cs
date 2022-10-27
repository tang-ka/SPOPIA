using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

//public class UserData
//{
//    // private Data
//    public string nickName;
//    public int age;
//    public int playerLevel; // 0: normal, 1 : middle, 2: high, 3: university 4: K2, 5: K1
//    public string position;
//    public string teamName;
//    public int height;
//    public int weight;
//    //public string profileImage;

//    // public Data (record)
//    public int goal;
//    public int assist;
//    public int matchCount;
//}

//public class TeamData
//{
//    // private Data
//    public int memberNum;
//    public string region;
//    public int[] levelerCount = new int[4] { 0, 0, 9, 4 };
//    public string formation;

//    // public Data (record)
//    public int goal;
//    public int lossGoal;
//    public int matchCount;
//    public int win;
//    public int lose;
//    public int draw;
//}

public class PlayfabManager : MonoBehaviour
{
    //public InputField dbTest;
    public string dbID = "A45FE526BA86DD94";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData()
    {
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "A", "1" }, { "B", "2" } }, Permission = UserDataPermission.Public};
        PlayFabClientAPI.UpdateUserData(request, (result) => print("올 데이터 저장 성공했는데?"), (error) => print("데이터 저장 실패했다ㅋㅋㅋ"));
    }

    public void GetData()
    {
        var request = new GetUserDataRequest() { PlayFabId = dbID };
        PlayFabClientAPI.GetUserData(request, (result) => print(result.Data["UserData"].Value), (error) => print("너 데이터 불러오기 실패했어"));
        PlayFabClientAPI.GetUserData(request, (result) => print(result.Data["TeamData"].Value), (error) => print("너 데이터 불러오기 실패했어"));
    }

    public void SaveJson()
    {
        UserData userData = new UserData();
        TeamData teamData = new TeamData();

        // testData
        userData.nickName = "tangka";
        userData.age = 27;
        userData.playerLevel = 4;
        userData.position = "CM";
        userData.teamName = "FC MTVS";
        userData.height = 175;
        userData.weight = 75;

        userData.goal = 10;
        userData.assist = 10;
        userData.matchCount = 5;

        teamData.memberNum = 13;
        teamData.region = "Pangyo";
        //teamData.levelerCount;
        teamData.formation = "4-4-2";

        teamData.goal = 23;
        teamData.lossGoal = 8;
        teamData.matchCount = 5;
        teamData.win = 4;
        teamData.lose = 0;
        teamData.draw = 1;

        // To Playfab
        Dictionary<string, string> dataDic = new Dictionary<string, string>();
        Dictionary<string, string> dataDic2 = new Dictionary<string, string>();
        dataDic.Add("UserData", JsonUtility.ToJson(userData));
        dataDic2.Add("TeamData", JsonUtility.ToJson(teamData));

        SetUserData(dataDic);
        SetUserData(dataDic2);
    }

    public void SetUserData(Dictionary<string, string> jsonData)
    {
        var request = new UpdateUserDataRequest() { Data = jsonData, Permission = UserDataPermission.Public };
        PlayFabClientAPI.UpdateUserData(request, (result) => print("올 데이터 저장 성공했는데?"), (error) => print("데이터 저장 실패했다ㅋㅋㅋ"));
    }
}
