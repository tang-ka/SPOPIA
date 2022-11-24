using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CubeBtnManager : MonoBehaviourPunCallbacks
{
    public GameObject teamInfoPage;
    public InputField inputTeamName, inputFormation;
    Color c; // �� �̸� ��(����)
    public GameObject go; // �� �̸� ������Ʈ ���� �Ҵ�

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

        // �� �̸� ������ ��, ���̵��� ȿ��
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
        // ���� - �� ��ȯ�Ҷ� mainī�޶� ��� ���� ���� �Ŷ����� �߰�
        //if (Camera.current == null) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // �� ����
            if (hit.transform.gameObject.name == "CubeButton")
            {
                /*// ������ ����
                TeamData teamData = new TeamData();
                teamData.teamName = inputTeamName.text;
                teamData.formation = inputFormation.text;

                // ���� ������ �� �߰�
                DBManager.instance.leagueInfo.teams.Add(teamData);

                // ���� DB ������Ʈ
                DBManager.instance.SaveJsonLeagueData(DBManager.instance.leagueInfo, "LeagueData");*/

                // teamInfoPage ����
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
                        for (int j = 0; j < DBManager.instance.leagues.leagueDatas.Count; j++)
                        {
                            if (DBManager.instance.leagues.leagueDatas[j].leagueName == DBManager.instance.leagueInfo.leagueName)
                            {
                                DBManager.instance.leagues.leagueDatas[j] = DBManager.instance.leagueInfo;

                                break;
                            }
                        }

                        // DB�� �������� ��û
                        DBManager.instance.SaveJsonLeagueData(DBManager.instance.leagues, "LeagueData");

                        // ���� ������ ����
                        PhotonNetwork.Instantiate("PlayerProfile", new Vector3(85, 8, 384.404388f), Quaternion.Euler(0, 90, 0));

                        print("���� ������ ����!");

                        // ã�� ���� �����Ѵ�.
                        break;
                    }
                }
            }

            /*else if(hit.transform.gameObject.name == "testCube")
            {
                // ���� ������ ����
                PhotonNetwork.Instantiate("PlayerProfile", new Vector3(85, 8, 384.404388f), Quaternion.identity);
            }*/
        }
    }

    public void AddTeam()
    {
        // ���� ������ ��, ����Ʈ
        PhotonNetwork.Instantiate("TeamNameEffect", new Vector3(130, 2.09446955f + 100f, 220), Quaternion.identity);

        // ������ ����
        TeamData teamData = new TeamData();
        teamData.teamName = inputTeamName.text;
        teamData.formation = inputFormation.text;

        // ���� ������ �� �߰�
        DBManager.instance.leagueInfo.teams.Add(teamData);

        // ���� DB ������Ʈ
        for (int i = 0; i < DBManager.instance.leagues.leagueDatas.Count; i++)
        {
            if (DBManager.instance.leagues.leagueDatas[i].leagueName == DBManager.instance.leagueInfo.leagueName)
            {
                DBManager.instance.leagues.leagueDatas[i] = DBManager.instance.leagueInfo;

                break;
            }
        }

        // DB�� �������� ��û
        DBManager.instance.SaveJsonLeagueData(DBManager.instance.leagues, "LeagueData");

        // ����忡 ���� ����
        go = Instantiate(Resources.Load<GameObject>("YS/TeamName"), new Vector3(130, 2.09446955f + 100f, 220), Quaternion.identity);
        go.GetComponent<TextMesh>().text = inputTeamName.text;
        c = go.GetComponent<TextMesh>().color;
        c.a = 0;
        go.GetComponent<TextMesh>().color = c;

        // RPC ������
        photonView.RPC(nameof(RpcAddTeam), RpcTarget.OthersBuffered, inputTeamName.text);
    }

    [PunRPC]
    void RpcAddTeam(string _text)
    {
        // ����忡 ���� ����
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
