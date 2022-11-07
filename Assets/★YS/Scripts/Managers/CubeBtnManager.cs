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
        }
    }
}
