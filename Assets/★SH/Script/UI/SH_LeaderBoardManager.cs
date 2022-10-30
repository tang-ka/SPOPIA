using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SH_LeaderBoardManager : MonoBehaviour
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
    }

    private void CategoryTEAMRANKING()
    {
        print("Show me the Team Ranking");
    }

    private void CategoryTopScorers()
    {
        print("Show me the Top Scorers");
    }

    private void CategoryTOPASSISTS()
    {
        print("Show me the Top Assists");
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

        switch (category)
        {
            case Category.TEAMRANKING:
                tr.SetActive(on);
                ts.SetActive(!on);
                ta.SetActive(!on);

                teamDataList = DataManager.instance.GetTeamDataList();

                // 1. 팀 랭킹에 표시할 항목을 계산하고 싶다.
                CalcTeamRank(teamDataList);
                // 2. 계산한 정보를 담은 RankBar를 순서대로 생성하고 싶다.
                CreateRankBar(teamRankList);
                break;

            case Category.TOPSCORES:
                tr.SetActive(!on);
                ts.SetActive(on);
                ta.SetActive(!on);

                userDataList = DataManager.instance.GetUserDataList(); ;

                break;

            case Category.TOPASSISTS:
                tr.SetActive(!on);
                ts.SetActive(!on);
                ta.SetActive(on);
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
                break;

            case Category.TOPASSISTS:
                break;
        }
    }

    void ChangeIndex()
    {
        index++;
        index %= 3;
        ChangeCategory((Category)index);
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

    void CreateRankBar(List<TeamData> teams)
    {
        for (int i = 0; i < teams.Count; i++)
        {
            GameObject bar = Instantiate(teamRankBarFactory, rankBarParent);
            bar.transform.position = rankBarParent.position + new Vector3(0, -7 * i, 0);

            Transform canvas = bar.transform.Find("Canvas");
            canvas.Find("Rank").GetComponent<Text>().text = teams[i].rank.ToString();
            //canvas.Find("Emblem").GetComponent<Text>().text = teams[i].rank.ToString();
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
}
