using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CubeBtnManager : MonoBehaviourPunCallbacks
{
    public GameObject teamInfoPage;
    public InputField inputTeamName, inputFormation;
    Color c; // 팀 이름 색(알파)
    public GameObject go; // 팀 이름 오브젝트 동적 할당

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

        // 팀 이름 생성될 때, 페이드인 효과
        if(go != null && go.GetComponent<TextMesh>().color.a < 1)
        {
            c.a += 0.00005f;
            if(c.a > 0.01f)
            {
                c.a += 0.005f;
                go.GetComponent<TextMesh>().color = c;
            }
        }
    }

    void BtnClick()
    {
        // 상혁 - 씬 전환할때 main카메라 없어서 오류 나는 거때문에 추가
        //if (Camera.current == null) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // 팀 생성
            if (hit.transform.gameObject.name == "CubeButton")
            {
                /*// 팀정보 세팅
                TeamData teamData = new TeamData();
                teamData.teamName = inputTeamName.text;
                teamData.formation = inputFormation.text;

                // 리그 정보에 팀 추가
                DBManager.instance.leagueInfo.teams.Add(teamData);

                // 리그 DB 업데이트
                DBManager.instance.SaveJsonLeagueData(DBManager.instance.leagueInfo, "LeagueData");*/

                // teamInfoPage 열기
                if(teamInfoPage.activeSelf == false)
                {
                    teamInfoPage.SetActive(true);
                }
                else
                {
                    teamInfoPage.SetActive(false);
                }
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
                        for (int j = 0; j < DBManager.instance.leagues.leagueDatas.Count; j++)
                        {
                            if (DBManager.instance.leagues.leagueDatas[j].leagueName == DBManager.instance.leagueInfo.leagueName)
                            {
                                DBManager.instance.leagues.leagueDatas[j] = DBManager.instance.leagueInfo;

                                break;
                            }
                        }

                        // DB에 수정사항 요청
                        DBManager.instance.SaveJsonLeagueData(DBManager.instance.leagues, "LeagueData");

                        // 개인 프로필 생성
                        PhotonNetwork.Instantiate("PlayerProfile", new Vector3(85, 8, 384.404388f), Quaternion.Euler(0, 90, 0));

                        print("개인 프로필 생성!");

                        // 찾는 것을 종료한다.
                        break;
                    }
                }
            }

            /*else if(hit.transform.gameObject.name == "testCube")
            {
                // 개인 프로필 생성
                PhotonNetwork.Instantiate("PlayerProfile", new Vector3(85, 8, 384.404388f), Quaternion.identity);
            }*/
        }
    }

    public void AddTeam()
    {
        // 팀명 생성될 때, 이펙트
        PhotonNetwork.Instantiate("TeamNameEffect", new Vector3(130, 2.09446955f + 100f, 220), Quaternion.identity);

        // 팀정보 세팅
        TeamData teamData = new TeamData();
        teamData.teamName = inputTeamName.text;
        teamData.formation = inputFormation.text;

        // 리그 정보에 팀 추가
        DBManager.instance.leagueInfo.teams.Add(teamData);

        // 리그 DB 업데이트
        for (int i = 0; i < DBManager.instance.leagues.leagueDatas.Count; i++)
        {
            if (DBManager.instance.leagues.leagueDatas[i].leagueName == DBManager.instance.leagueInfo.leagueName)
            {
                DBManager.instance.leagues.leagueDatas[i] = DBManager.instance.leagueInfo;

                break;
            }
        }

        // DB에 수정사항 요청
        DBManager.instance.SaveJsonLeagueData(DBManager.instance.leagues, "LeagueData");

        // 경기장에 팀명 띄우기
        go = Instantiate(Resources.Load<GameObject>("YS/TeamName"), new Vector3(130, 2.09446955f + 100f, 220), Quaternion.identity);
        go.GetComponent<TextMesh>().text = inputTeamName.text;
        c = go.GetComponent<TextMesh>().color;
        c.a = 0;
        go.GetComponent<TextMesh>().color = c;

        // RPC 보내기
        photonView.RPC(nameof(RpcAddTeam), RpcTarget.OthersBuffered, inputTeamName.text);
    }

    [PunRPC]
    void RpcAddTeam(string _text)
    {
        // 경기장에 팀명 띄우기
        go = Instantiate(Resources.Load<GameObject>("YS/TeamName"), new Vector3(130, 2.09446955f + 100f, 220), Quaternion.identity);
        go.GetComponent<TextMesh>().text = _text;
        c = go.GetComponent<TextMesh>().color;
        c.a = 0;
        go.GetComponent<TextMesh>().color = c;
    }

    public void Cancel()
    {
        teamInfoPage.SetActive(false);
    }
}
