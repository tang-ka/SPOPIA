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

    // ������ �ڵ����� �Ѿ�Բ� (����)
    float time;
    bool isTimed = false;

    // �� �ΰ�
    public RawImage rawImg;

    // ���� �̸�
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

        // ������ �ð� ����ȭ (����)
        time = (float)PhotonNetwork.Time;

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            // ������ �ڵ����� �Ѿ�Բ� (����)
            if (time % 5 == 0 && isTimed == false) // 5�ʸ���
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
            print("�� �ƴϾ�~");
            return;
        }

        preCategory = category;
        EndCategory(preCategory);

        category = c;

        // ������ �ٲ� ������ ������ �ٽ� ����
        DataManager.instance.SetData();

        switch (category)
        {
            case Category.TEAMRANKING:
                tr.SetActive(on);
                ts.SetActive(!on);
                ta.SetActive(!on);

                teamDataList = DataManager.instance.GetTeamDataList();

                //teamDataList.Sort(SortRank);

                // 1. �� ��ŷ�� ǥ���� �׸��� ����ϰ� �ʹ�.
                CalcTeamRank(teamDataList);
                // 2. ����� ������ ���� RankBar�� ������� �����ϰ� �ʹ�.
                CreateTeamRankBar(teamRankList);
                break;

            case Category.TOPSCORES:
                tr.SetActive(!on);
                ts.SetActive(on);
                ta.SetActive(!on);

                userDataList = DataManager.instance.GetUserDataList();

                // 1. ���� ��ŷ�� ǥ���� �׸��� ����ϰ� �ʹ�.
                userDataList.Sort(SortTopScorers);
                // 2. ����� ������ ���� RankBar�� ������� �����ϰ� �ʹ�.
                CreatePlayerRankBar(userDataList, "goal");
                break;

            case Category.TOPASSISTS:

                tr.SetActive(!on);
                ts.SetActive(!on);
                ta.SetActive(on);

                userDataList = DataManager.instance.GetUserDataList();

                // 1. ���� ��ŷ�� ǥ���� �׸��� ����ϰ� �ʹ�.
                userDataList.Sort(SortTopAssists);
                // 2. ����� ������ ���� RankBar�� ������� �����ϰ� �ʹ�.
                CreatePlayerRankBar(userDataList, "assist");
                break;
        }
    }

    private void EndCategory(Category c)
    {
        switch (c)
        {
            case Category.TEAMRANKING:
                // 3. ������ RankBar�� ��� �����ϰ� �ʹ�.
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
