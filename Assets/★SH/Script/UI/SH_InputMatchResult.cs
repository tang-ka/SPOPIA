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
    public SH_DropDownController selectedTeam;

    public GameObject showBarFactory;

    Transform scorerParent;
    Transform assistParent;

    public List<SH_ShowBar> listScorer = new List<SH_ShowBar>();
    public List<SH_ShowBar> listAssist = new List<SH_ShowBar>();

    //public TeamData teamData = new TeamData();

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

    public void OnScoreValueChanged(string inputText)
    {
        if (inputText.Length == 0) return;

        goal = int.Parse(inputText);
    }

    public void EnterInputScore(string inputText)
    {
        isInputScore = true;
        goal = int.Parse(inputText);
        //teamData.goal += goal;
        print(goal);
    }

    public void EnterInputScorer(string inputText)
    {
        if (inputText.Length == 0) return;

        inputScorer.text = "";
        inputScorer.ActivateInputField();

        print("Scorer : " + inputText);

        // 통신 -> 닉네임 존재여부

        // 있으면
        GameObject bar = Instantiate(showBarFactory, scorerParent);
        SH_ShowBar sb = bar.GetComponent<SH_ShowBar>();

        // 가상데이터
        UserData data = new UserData();
        data.nickName = inputText;
        data.goal = Random.Range(1, 5);

        sb.Init(data);
        sb.deleteList += DeleteListScorer;

        listScorer.Add(sb);
    }

    public void EnterInputAssist(string inputText)
    {
        if (inputText.Length == 0) return;

        inputAssist.text = "";
        inputAssist.ActivateInputField();

        print("Assist : " + inputText);

        // 통신 -> 닉네임 존재여부

        // 있으면
        GameObject bar = Instantiate(showBarFactory, assistParent);
        SH_ShowBar sb = bar.GetComponent<SH_ShowBar>();

        // 가상데이터
        UserData data = new UserData();
        data.nickName = inputText;
        data.assist = Random.Range(1, 5);

        sb.Init(data);
        sb.deleteList += DeleteListAssist;

        listAssist.Add(sb);
    }

    public void DeleteListScorer(SH_ShowBar sb)
    {
        listScorer.Remove(sb);
    }
    public void DeleteListAssist(SH_ShowBar sb)
    {
        listAssist.Remove(sb);
    }

    public void OnClickOkay()
    {
        for (int i = 0; i < listScorer.Count; i++)
        {
            listScorer[i].AddGoal();
        }

        for (int i = 0; i < listAssist.Count; i++)
        {
            listAssist[i].AddAssist();
        }

        selectedTeam.SetMatchData(goal, opponent.goal);

        AllClear(listScorer.Count, listAssist.Count);
    }

    public void AllClear(int scorer, int assist)
    {
        inputScorer.text = "";
        inputScore.text = "";
        inputAssist.text = "";

        for (int i = 0; i < scorer; i++)
        {
            listScorer[0].OnCLickDelete();
        }

        for (int i = 0; i < assist; i++)
        {
            listAssist[0].OnCLickDelete();
        }

        isInputScore = false;
    }

    // Team을 선택하면 그 팀의 정보를 수정하고 싶다.
    // Drop Down에서 팀이름을 선택하면 저장 하고싶다.
}
