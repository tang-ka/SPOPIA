using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_InputMatchResult : MonoBehaviour
{
    public InputField inputScore;
    public InputField inputScorer;
    public InputField inputAssist;

    public SH_InputMatchResult opponent;

    public GameObject showBarFactory;

    Transform scorerParent;
    Transform assistParent;

    public List<SH_ShowBar> listScorer = new List<SH_ShowBar>();
    public List<SH_ShowBar> listAssist = new List<SH_ShowBar>();

    public TeamData teamData = new TeamData();

    bool isInputScore = false;
    int goal;

    void Start()
    {
        inputScore.onSubmit.AddListener(EnterInputScore);
        inputScore.onValueChanged.AddListener(OnScoreValueChanged);
        inputScorer.onSubmit.AddListener(EnterInputScorer);
        inputAssist.onSubmit.AddListener(EnterInputAssist);

        scorerParent = inputScorer.transform.Find("Shower").Find("Viewport").Find("Content");
        assistParent = inputAssist.transform.Find("Shower").Find("Viewport").Find("Content");
    }

    void Update()
    {
    }

    public void OnScoreValueChanged(string inputText)
    {
        if (inputText.Length == 0) return;
        goal = int.Parse(inputText);
    }

    public void EnterInputScore(string inputText)
    {
        isInputScore = true;
        goal = int.Parse(inputText);
        teamData.goal += goal;
        print(goal);
    }

    public void EnterInputScorer(string inputText)
    {
        inputScorer.text = "";
        inputScorer.ActivateInputField();

        print("Scorer : " + inputText);

        // ��� -> �г��� ���翩��

        // ������
        GameObject bar = Instantiate(showBarFactory, scorerParent);
        SH_ShowBar sb = bar.GetComponent<SH_ShowBar>();

        // ��������
        UserData data = new UserData();
        data.nickName = inputText;
        data.goal = Random.Range(1, 5);

        sb.Init(data);
        
        listScorer.Add(sb);
    }

    public void EnterInputAssist(string inputText)
    {
        inputAssist.text = "";
        inputAssist.ActivateInputField();

        print("Assist : " + inputText);

        // ��� -> �г��� ���翩��

        // ������
        GameObject bar = Instantiate(showBarFactory, assistParent);
        SH_ShowBar sb = bar.GetComponent<SH_ShowBar>();

        // ��������
        UserData data = new UserData();
        data.nickName = inputText;
        data.assist = Random.Range(1, 5);

        sb.Init(data);

        listAssist.Add(sb);
    }

    public void OnClickOkay()
    {
        if (isInputScore == false)
        {
            teamData.goal += goal;
            print(goal);
        }

        for (int i = 0; i < listScorer.Count; i++)
        {
            listScorer[i].AddGoal();
        }

        for (int i = 0; i < listAssist.Count; i++)
        {
            listAssist[i].AddAssist();
        }

        if (goal > opponent.goal)
            teamData.win++;
        else if (goal < opponent.goal)
            teamData.lose++;
        else
            teamData.draw++;

        AllClear();
    }

    public void AllClear()
    {
        inputScorer.text = "";
        inputScore.text = "";
        inputAssist.text = "";

        isInputScore = false;
    }

    // Team�� �����ϸ� �� ���� ������ �����ϰ� �ʹ�.
    // Drop Down���� ���̸��� �����ϸ� ���� �ϰ�ʹ�.
}
