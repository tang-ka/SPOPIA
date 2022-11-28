using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SH_LeaderBoardManager : MonoBehaviourPunCallbacks
{
    enum Category
    {
        TEAMRANKING,
        TOPSCORES,
        TOPASSISTS,
    }
    Category category = Category.TOPASSISTS;
    Category preCategory;

    public GameObject tr;
    public GameObject ts;
    public GameObject ta;

    public GameObject teamRankBarFactory;
    public GameObject playerRankBarFactory;
    public Transform rankBarParent;

    [SerializeField]
    List<TeamData> teamDataList = new List<TeamData>();
    [SerializeField]
    List<TeamData> teamRankList = new List<TeamData>();

    [SerializeField]
    List<UserData> userDataList = new List<UserData>();
    [SerializeField]
    List<UserData> playerRankList = new List<UserData>();

    bool on = true;
    int index = 0;

    // 전광판 자동으로 넘어가게끔 (영수)
    float time;
    bool isTimed = false;

    // 팀 로고
    public RawImage rawImg;

    // 파일 이름
    string filename;

    void Start()
    {
        ChangeCategory(Category.TEAMRANKING);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.BackQuote))
        {
            ChangeIndex();
        }

        switch (category)
        {
            case Category.TEAMRANKING:
                CategoryTEAMRANKING();
                break;

            case Category.TOPSCORES:
                CategoryTopScorers();
                break;

            case Category.TOPASSISTS:
                CategoryTOPASSISTS();
                break;
        }

        // 전광판 시간 동기화 (영수)
        time = (float)PhotonNetwork.Time;

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            // 전광판 자동으로 넘어가게끔 (영수)
            if (time % 5 == 0 && isTimed == false) // 5초마다
            {
                ChangeIndex();
                isTimed = true;
            }

            if (time % 5 != 0)
            {
                isTimed = false;
            }
        }
    }

    private void CategoryTEAMRANKING()
    {
        //print("Show me the Team Ranking");
    }

    private void CategoryTopScorers()
    {
        //print("Show me the Top Scorers");
    }

    private void CategoryTOPASSISTS()
    {
        //print("Show me the Top Assists");
    }

    void ChangeCategory(Category c)
    {
        if (category == c)
        {
            print("응 아니야~");
            return;
        }

        preCategory = category;
        EndCategory(preCategory);

        category = c;

        // 페이지 바뀔 때마다 데이터 다시 설정
        DataManager.instance.SetData();

        switch (category)
        {
            case Category.TEAMRANKING:
                tr.SetActive(on);
                ts.SetActive(!on);
                ta.SetActive(!on);

                teamDataList = DataManager.instance.GetTeamDataList();

                //teamDataList.Sort(SortRank);

                // 1. 팀 랭킹에 표시할 항목을 계산하고 싶다.
                CalcTeamRank(teamDataList);
                // 2. 계산한 정보를 담은 RankBar를 순서대로 생성하고 싶다.
                CreateTeamRankBar(teamRankList);
                break;

            case Category.TOPSCORES:
                tr.SetActive(!on);
                ts.SetActive(on);
                ta.SetActive(!on);

                userDataList = DataManager.instance.GetUserDataList();

                // 1. 득점 랭킹에 표시할 항목을 계산하고 싶다.
                userDataList.Sort(SortTopScorers);
                // 2. 계산한 정보를 담은 RankBar를 순서대로 생성하고 싶다.
                CreatePlayerRankBar(userDataList, "goal");
                break;

            case Category.TOPASSISTS:

                tr.SetActive(!on);
                ts.SetActive(!on);
                ta.SetActive(on);

                userDataList = DataManager.instance.GetUserDataList();

                // 1. 득점 랭킹에 표시할 항목을 계산하고 싶다.
                userDataList.Sort(SortTopAssists);
                // 2. 계산한 정보를 담은 RankBar를 순서대로 생성하고 싶다.
                CreatePlayerRankBar(userDataList, "assist");
                break;
        }
    }

    private void EndCategory(Category c)
    {
        switch (c)
        {
            case Category.TEAMRANKING:
                // 3. 생성한 RankBar를 모두 삭제하고 싶다.
                DestroyRankBar();
                break;

            case Category.TOPSCORES:
                DestroyRankBar();
                break;

            case Category.TOPASSISTS:
                DestroyRankBar();
                break;
        }
    }

    void ChangeIndex()
    {
        index++;
        index %= 3;
        ChangeCategory((Category)index);

        photonView.RPC(nameof(RpcChangeIndex), RpcTarget.OthersBuffered, index);
    }

    [PunRPC]
    void RpcChangeIndex(int _index)
    {
        ChangeCategory((Category)_index);
    }

    void CalcTeamRank(List<TeamData> teams)
    {
        TeamData[] temp = new TeamData[teams.Count];

        int rank = 1;

        // 승점 계산
        for (int i = 0; i < teams.Count; i++)
        {
            int pts = 3 * teams[i].win + teams[i].draw;
            teams[i].points = pts;
        }

        // 랭킹 계산
        for (int i = 0; i < teams.Count; i++)
        {
            // 1. 가장 포인트가 많은 팀을 알고 싶다.
            for (int j = 0; j < teams.Count; j++)
            {
                if (teams[i].points >= teams[j].points)
                    continue;
                else
                    rank++;
            }
            // 2. temp에 rank에 맞게 담고 싶다.
            teams[i].rank = rank;
            while (true)
            {
                if (temp[rank - 1] != null)
                {
                    rank++;
                }
                else
                {
                    temp[rank - 1] = teams[i];
                    break;
                }
            }
            rank = 1;
        }

        // teamRankList를 클리어 하고 temp를 이용해 다시 정렬해준다.
        teamRankList.Clear();
        for (int i = 0; i < temp.Length; i++)
        {
            teamRankList.Add(temp[i]);
        }
    }

    void CreateTeamRankBar(List<TeamData> teams)
    {
        for (int i = 0; i < teams.Count; i++)
        {
            GameObject bar = Instantiate(teamRankBarFactory, rankBarParent);
            bar.transform.position = rankBarParent.position + new Vector3(0, -7 * i, 0);

            Transform canvas = bar.transform.Find("Canvas");
            canvas.Find("Rank").GetComponent<Text>().text = teams[i].rank.ToString();
            DownloadLogoImage(teams[i].teamName);
            canvas.Find("Emblem").GetComponent<RawImage>().texture = rawImg.texture;
            canvas.Find("ClubName").GetComponent<Text>().text = teams[i].teamName;
            canvas.Find("Played").GetComponent<Text>().text = teams[i].matchCount.ToString();
            canvas.Find("Won").GetComponent<Text>().text = teams[i].win.ToString();
            canvas.Find("Drawn").GetComponent<Text>().text = teams[i].draw.ToString();
            canvas.Find("Lost").GetComponent<Text>().text = teams[i].lose.ToString();
            canvas.Find("GoalsFor").GetComponent<Text>().text = teams[i].goal.ToString();
            canvas.Find("GoalsAgainst").GetComponent<Text>().text = teams[i].lossGoal.ToString();
            canvas.Find("Points").GetComponent<Text>().text = teams[i].points.ToString();
        }
    }

    void DestroyRankBar()
    {
        foreach (Transform tr in rankBarParent)
        {
            Destroy(tr.gameObject);
        }
    }

    int SortTopScorers(UserData left, UserData right)
    {
        if (left.goal < right.goal)
        {
            return 1;
        }
        return -1;
    }

    int SortTopAssists(UserData left, UserData right)
    {
        if (left.assist < right.assist)
        {
            return 1;
        }
        return -1;
    }

    void CreatePlayerRankBar(List<UserData> users, string first)
    {
        Text nickNameText;

        switch (first)
        {
            case "goal":
                for (int i = 0; i < users.Count; i++)
                {
                    GameObject bar = Instantiate(playerRankBarFactory, rankBarParent);
                    bar.transform.position = rankBarParent.position + new Vector3(0, -7 * i, 0);
                    Transform canvas = bar.transform.Find("Canvas");

                    canvas.Find("Rank").GetComponent<Text>().text = users[i].goalRank.ToString();
                    DownloadLogoImage(users[i].teamName);
                    canvas.Find("Emblem").GetComponent<RawImage>().texture = rawImg.texture;
                    nickNameText = canvas.Find("PlayerName").GetComponent<Text>();
                    nickNameText.text = users[i].nickName;
                    canvas.Find("First").GetComponent<Text>().text = users[i].goal.ToString();
                    canvas.Find("Second").GetComponent<Text>().text = users[i].assist.ToString();
                    canvas.Find("Played").GetComponent<Text>().text = users[i].matchCount.ToString();

                    SetFont(ref nickNameText, 50, 0, 10, 12, 14);
                }
                break;

            case "assist":
                for (int i = 0; i < users.Count; i++)
                {
                    GameObject bar = Instantiate(playerRankBarFactory, rankBarParent);
                    bar.transform.position = rankBarParent.position + new Vector3(0, -7 * i, 0);
                    Transform canvas = bar.transform.Find("Canvas");

                    canvas.Find("Rank").GetComponent<Text>().text = users[i].goalRank.ToString();
                    DownloadLogoImage(users[i].teamName);
                    canvas.Find("Emblem").GetComponent<RawImage>().texture = rawImg.texture;
                    nickNameText = canvas.Find("PlayerName").GetComponent<Text>();
                    nickNameText.text = users[i].nickName;
                    canvas.Find("First").GetComponent<Text>().text = users[i].assist.ToString();
                    canvas.Find("Second").GetComponent<Text>().text = users[i].goal.ToString();
                    canvas.Find("Played").GetComponent<Text>().text = users[i].matchCount.ToString();

                    SetFont(ref nickNameText, 50, 0, 10, 12, 14);
                }
                break;
        }
    }

    void SetFont(ref Text inputText, int sizeStep,
        int first = 0, int second = 98, int third = 99, int fourth = 100)
    {
        int len = inputText.text.Length;

        if (len >= first && len < second)
            inputText.fontSize = 250;

        else if (len >= second && len < third)
            inputText.fontSize = 250 - sizeStep;

        else if (len >= third && len < fourth)
            inputText.fontSize = 250 - (sizeStep * 2);
    }

    public void DownloadLogoImage(string teamName)
    {
        filename = "logo_" + teamName + ".png";

        byte[] byteTexture = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/" + filename);

        if (byteTexture.Length > 0)
        {
            Texture2D t = new Texture2D(0, 0);
            t.LoadImage(byteTexture);

            rawImg.texture = t;
        }
    }
}
