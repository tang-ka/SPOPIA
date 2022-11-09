using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.ComponentModel;

public class SH_DropDownController : MonoBehaviour
{
    TMP_Dropdown dropdown;

    List<string> optionList = new List<string>();

    [SerializeField]
    TeamData teamData = new TeamData();

    // DB에 저장할 때, teams에서 어떤 팀이 선택되었는지 알려주는 public 값 (영수)
    public int idx;

    void Start()
    {
        // 드랍다운 옵션
        dropdown = this.GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();

        // fakeLeagueData.teams 이용해서 드랍다운 옵션 초기화 하고 싶다.
        optionList = DataManager.instance.GetTeamsNames();

        dropdown.AddOptions(optionList);

        // 드랍다운 옵션 변경시 호출 되는 함수를 등록 하고 싶다.
        dropdown.onValueChanged.AddListener(OnValueChanged);

        // 옵션 초기값 넣어주기
        OnValueChanged(0);
    }

    // 옵션을 선택할 때 마다 호출 되는 함수
    private void OnValueChanged(int arg)
    {
        print(optionList[arg]);

        // 클래스는 레퍼런스 타입이라 이렇게 대입해도 원본 데이터가 수정이 된다.
        teamData = DataManager.instance.GetTeamDataList()[arg];

        // DB에 보낼 인덱스 번호 (영수)
        idx = arg;
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

        // DB에 저장 (영수)
        string s = idx.ToString();
        DBManager.instance.UpdateTeamData(teamData, s);
    }
}
