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

    // DB�� ������ ��, teams���� � ���� ���õǾ����� �˷��ִ� public �� (����)
    public int idx;

    void Start()
    {
        // ����ٿ� �ɼ�
        dropdown = this.GetComponent<TMP_Dropdown>();
        dropdown.ClearOptions();

        // fakeLeagueData.teams �̿��ؼ� ����ٿ� �ɼ� �ʱ�ȭ �ϰ� �ʹ�.
        optionList = DataManager.instance.GetTeamsNames();

        dropdown.AddOptions(optionList);

        // ����ٿ� �ɼ� ����� ȣ�� �Ǵ� �Լ��� ��� �ϰ� �ʹ�.
        dropdown.onValueChanged.AddListener(OnValueChanged);

        // �ɼ� �ʱⰪ �־��ֱ�
        OnValueChanged(0);
    }

    // �ɼ��� ������ �� ���� ȣ�� �Ǵ� �Լ�
    private void OnValueChanged(int arg)
    {
        print(optionList[arg]);

        // Ŭ������ ���۷��� Ÿ���̶� �̷��� �����ص� ���� �����Ͱ� ������ �ȴ�.
        teamData = DataManager.instance.GetTeamDataList()[arg];

        // DB�� ���� �ε��� ��ȣ (����)
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

        // DB�� ���� (����)
        string s = idx.ToString();
        DBManager.instance.UpdateTeamData(teamData, s);
    }
}
