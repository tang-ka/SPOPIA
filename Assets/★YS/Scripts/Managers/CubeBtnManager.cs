using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeBtnManager : MonoBehaviour
{
    public GameObject teamInfoPage;
    public InputField inputTeamName, inputFormation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BtnClick();
        }
    }

    void BtnClick()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // 팀 생성
            if (hit.transform.gameObject.name == "CubeButton")
            {
                // 팀정보 세팅
                TeamData teamData = new TeamData();
                teamData.teamName = inputTeamName.text;
                teamData.formation = inputFormation.text;

                // 리그 정보에 팀 추가
                DBManager.instance.leagueInfo.teams.Add(teamData);

                // 리그 DB 업데이트
                DBManager.instance.SaveJsonLeagueData(DBManager.instance.leagueInfo, "LeagueData");
            }

            else if (hit.transform.gameObject.name == "AddUserButton")
            {
                // 리그DB에서 team리스트를 검사한다. (Add될 팀을 찾기 위해)
                for (int i = 0; i < DBManager.instance.leagueInfo.teams.Count; i++)
                {
                    TeamData info = DBManager.instance.leagueInfo.teams[i];

                    // User가 Add될 팀이라면?
                    if(info.teamName == inputTeamName.text)
                    {
                        // 해당 팀의 user리스트에 user를 추가한다.
                        info.users.Add(DBManager.instance.myData);

                        // 리그 DB 업데이트
                        DBManager.instance.SaveJsonLeagueData(DBManager.instance.leagueInfo, "LeagueData");

                        // 찾는 것을 종료한다.
                        break;
                    }
                }
            }
        }
    }
}
