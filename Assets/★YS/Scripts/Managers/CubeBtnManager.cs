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
            // �� ����
            if (hit.transform.gameObject.name == "CubeButton")
            {
                // ������ ����
                TeamData teamData = new TeamData();
                teamData.teamName = inputTeamName.text;
                teamData.formation = inputFormation.text;

                // ���� ������ �� �߰�
                DBManager.instance.leagueInfo.teams.Add(teamData);

                // ���� DB ������Ʈ
                DBManager.instance.SaveJsonLeagueData(DBManager.instance.leagueInfo, "LeagueData");
            }

            else if (hit.transform.gameObject.name == "AddUserButton")
            {
                // ����DB���� team����Ʈ�� �˻��Ѵ�. (Add�� ���� ã�� ����)
                for (int i = 0; i < DBManager.instance.leagueInfo.teams.Count; i++)
                {
                    TeamData info = DBManager.instance.leagueInfo.teams[i];

                    // User�� Add�� ���̶��?
                    if(info.teamName == inputTeamName.text)
                    {
                        // �ش� ���� user����Ʈ�� user�� �߰��Ѵ�.
                        info.users.Add(DBManager.instance.myData);

                        // ���� DB ������Ʈ
                        DBManager.instance.SaveJsonLeagueData(DBManager.instance.leagueInfo, "LeagueData");

                        // ã�� ���� �����Ѵ�.
                        break;
                    }
                }
            }
        }
    }
}
