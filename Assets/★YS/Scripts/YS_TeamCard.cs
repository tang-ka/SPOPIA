using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YS_TeamCard : MonoBehaviour
{
    TeamData myTeam;
    public CubeBtnManager cubeManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 팀 설정
        if(cubeManager.go != null)
        {
            if(myTeam == null)
            {
                for(int i = 0; DBManager.instance.leagueInfo.teams.Count > i; i++)
                {
                    if(cubeManager.go.GetComponent<TextMesh>().text == DBManager.instance.leagueInfo.teams[i].teamName)
                    {
                        myTeam = DBManager.instance.leagueInfo.teams[i];

                        break;
                    }
                }
            }
        }

        // 실시간으로 정보 설정해주기
        SetInfo();
    }

    string s; // 골득실 +- 표시
    void SetInfo()
    {
        GameObject canvas = transform.Find("Canvas").gameObject;

        canvas.transform.Find("TeamName").gameObject.GetComponent<Text>().text = myTeam.teamName;
        canvas.transform.Find("Win").gameObject.GetComponent<Text>().text = myTeam.win + "승";
        canvas.transform.Find("Draw").gameObject.GetComponent<Text>().text = myTeam.draw + "무";
        canvas.transform.Find("Lose").gameObject.GetComponent<Text>().text = myTeam.lose + "패";
        canvas.transform.Find("MatchCount").gameObject.GetComponent<Text>().text = myTeam.matchCount.ToString();
        canvas.transform.Find("WinPoints").gameObject.GetComponent<Text>().text = myTeam.points.ToString();
        canvas.transform.Find("Goal").gameObject.GetComponent<Text>().text = myTeam.goal.ToString();
        canvas.transform.Find("LossGoal").gameObject.GetComponent<Text>().text = myTeam.lossGoal.ToString();
        if(myTeam.goal - myTeam.lossGoal < 0)
        {
            s = "-" + (myTeam.goal - myTeam.lossGoal).ToString();
        }
        else if(myTeam.goal - myTeam.lossGoal > 0)
        {
            s = "+" + (myTeam.goal - myTeam.lossGoal).ToString();
        }
        else
        {
            s = "0";
        }
        canvas.transform.Find("Goal-Loss").gameObject.GetComponent<Text>().text = s;
    }
}
