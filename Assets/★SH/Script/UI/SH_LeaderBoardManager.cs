using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;

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

    public GameObject rankBarFactory;
    public Transform rankBarParent;
    

    List<GameObject> ranking;

    [SerializeField]
    List<TeamData> teamDataList = new List<TeamData>();
    [SerializeField]
    List<TeamData> teamRankList = new List<TeamData>();

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
            print("�� �ƴϾ�~");
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

                // 1. �� ��ŷ�� ǥ���� �׸��� ����ϰ� �ʹ�.
                CalcTeamRank(teamDataList);
                // 2. ����� ������ ���� RankBar�� ������� �����ϰ� �ʹ�.
                CreateRankBar(teamRankList);
                // 3. ������ RankBar�� ��� �����ϰ� �ʹ�.

                break;

            case Category.TOPSCORES:
                tr.SetActive(!on);
                ts.SetActive(on);
                ta.SetActive(!on);
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

        // ���� ���
        for (int i = 0; i < teams.Count; i++)
        {
            int pts = 3 * teams[i].win + teams[i].draw;
            teams[i].points = pts;
        }

        // ��ŷ ���
        for (int i = 0; i < teams.Count; i++)
        {
            // 1. ���� ����Ʈ�� ���� ���� �˰� �ʹ�.
            for (int j = 0; j < teams.Count; j++)
            {
                if (teams[i].points >= teams[j].points)
                    continue;
                else
                    rank++;
            }
            // 2. temp�� rank�� �°� ��� �ʹ�.
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

        // teamRankList�� Ŭ���� �ϰ� temp�� �̿��� �ٽ� �������ش�.
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
            GameObject bar = Instantiate(rankBarFactory, rankBarParent);
            bar.transform.position = rankBarParent.position + Vector3.down * 7 * i;
        }
    }
}
