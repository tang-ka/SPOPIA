using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.ComponentModel;

public class SH_DropDownController : MonoBehaviour
{
    TMP_Dropdown options;

    List<string> optionList = new List<string>();

    [SerializeField] TeamData teamData = new TeamData();

    void Start()
    {
        options = this.GetComponent<TMP_Dropdown>();

        options.ClearOptions();

        // leagueData.teams 이용해서 초기화 하기
        optionList.Add("FC XR");
        optionList.Add("FC AI");
        optionList.Add("FC Net");
        optionList.Add("FC Cre");

        options.AddOptions(optionList);

        options.onValueChanged.AddListener(OnValueChanged);
        OnValueChanged(0);
    }

    private void OnValueChanged(int arg)
    {
        print(optionList[arg]);

        // 가짜 데이터
        teamData.teamName = optionList[arg];
    }

    public void SetMatchData(int ourScore, int oppScore)
    {
        teamData.goal += ourScore;
        teamData.lossGoal += oppScore;
        teamData.matchCount++;

        if (ourScore > oppScore)
            teamData.win++;
        else if (ourScore < oppScore)
            teamData.lose++;
        else
            teamData.draw++;
    }

    void Update()
    {
        
    }
}
