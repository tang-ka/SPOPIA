using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Firebase;
using Firebase.Storage;
using Firebase.Extensions;
using Photon.Pun;

public class CubeBtnManager : MonoBehaviourPunCallbacks
{
    public GameObject teamInfoPage, teamPage, userPage;
    public InputField inputTeamName, inputFormation;
    Color c; // �� �̸� ��(����)
    public GameObject go; // �� �̸� ������Ʈ ���� �Ҵ�

    // ���� �Ϸ� �˾�
    public GameObject goodPopUp, goodPopUp2;

    // ���� ī�� ����
    TeamData myTeam;

    // �� �ΰ�
    public RawImage rawImg;

    // firebase ����
    FirebaseStorage storage;
    StorageReference storageRef;
    bool b_download = false;

    // ���� �̸�
    string filename;

    // Start is called before the first frame update
    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;

        StartCoroutine(CreateUserCards());
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

        // �� ���� �Ǿ�����, ������ �ٲ��ֱ� (�������� �ִ� ��������)
        if(go != null)
        {
            teamPage.SetActive(false);
            userPage.SetActive(true);
        }
    }

    void BtnClick() // User�߰�
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
                // ���� ��� �Ϸ� �˾�
                goodPopUp2.SetActive(true);

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

    public void GoodPopUp()
    {
        goodPopUp.SetActive(true);
    }

    public void GoodPopUp2()
    {
        goodPopUp2.SetActive(false);
    }

    public void AddTeam()
    {
        // �˾� ���ֱ�
        goodPopUp.SetActive(false);
        teamInfoPage.SetActive(false);

        // ��ġ
        Vector3 loc = transform.parent.transform.parent.transform.position;

        // ���� ������ ��, ����Ʈ
        PhotonNetwork.Instantiate("TeamNameEffect", new Vector3(loc.x, loc.y + 100f, loc.z), Quaternion.identity);

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
        go = Instantiate(Resources.Load<GameObject>("YS/TeamName"), new Vector3(loc.x, loc.y + 100f, loc.z), Quaternion.identity);
        go.GetComponent<TextMesh>().text = inputTeamName.text;
        c = go.GetComponent<TextMesh>().color;
        c.a = 0;
        go.GetComponent<TextMesh>().color = c;

        go.transform.Find("Canvas").transform.Find("TeamLogo").gameObject.GetComponent<RawImage>();

        if (!go.transform.Find("Canvas").transform.Find("TeamLogo").gameObject.GetComponent<RawImage>().texture)
        {
            DownloadLogoImage();
        }
        go.transform.Find("Canvas").transform.Find("TeamLogo").gameObject.GetComponent<RawImage>().texture = rawImg.texture;

        // RPC ������
        photonView.RPC(nameof(RpcAddTeam), RpcTarget.OthersBuffered, inputTeamName.text, loc);
    }

    [PunRPC]
    void RpcAddTeam(string _text, Vector3 _loc)
    {
        // ����忡 ���� ����
        go = Instantiate(Resources.Load<GameObject>("YS/TeamName"), new Vector3(_loc.x, _loc.y + 100f, _loc.z), Quaternion.identity);
        go.GetComponent<TextMesh>().text = _text;
        c = go.GetComponent<TextMesh>().color;
        c.a = 0;
        go.GetComponent<TextMesh>().color = c;
    }

    public void Cancel()
    {
        teamInfoPage.SetActive(false);
    }

    IEnumerator CreateUserCards()
    {
        for (int i = 0; DBManager.instance.leagueInfo.teams.Count > i; i++)
        {
            if (go.GetComponent<TextMesh>().text == DBManager.instance.leagueInfo.teams[i].teamName)
            {
                myTeam = DBManager.instance.leagueInfo.teams[i];

                break;
            }
        }

        for(int i = 0; myTeam.users.Count > i; i++)
        {
            // ���� ������ ����
            GameObject userCard;
            userCard = (GameObject)Resources.Load("YS/UserCard");

            // ��ġ
            Vector3 loc = transform.parent.transform.parent.transform.position;

            // ī�� ���� ����
            if (!userCard.transform.Find("Canvas").transform.Find("TeamLogo").transform.Find("Logo").gameObject.GetComponent<RawImage>().texture)
            {
                DownloadLogoImage();
            }

            yield return new WaitUntil(() => b_download == true);
            b_download = false;

            userCard.transform.Find("Canvas").transform.Find("TeamLogo").transform.Find("Logo").gameObject.GetComponent<RawImage>().texture = rawImg.texture;
            userCard.transform.Find("Canvas").transform.Find("BackNumber").gameObject.GetComponent<Text>().text = myTeam.users[i].backNumber.ToString();
            userCard.transform.Find("Canvas").transform.Find("Position").gameObject.GetComponent<Text>().text = myTeam.users[i].position;
            userCard.transform.Find("Canvas").transform.Find("NickName").gameObject.GetComponent<Text>().text = myTeam.users[i].nickName;
            userCard.transform.Find("Canvas").transform.Find("Name").gameObject.GetComponent<Text>().text = myTeam.users[i].realName;
            userCard.transform.Find("Canvas").transform.Find("Height").gameObject.GetComponent<Text>().text = myTeam.users[i].height.ToString() + "cm";
            userCard.transform.Find("Canvas").transform.Find("Weight").gameObject.GetComponent<Text>().text = myTeam.users[i].weight.ToString() + "kg";

            if(i < 9)
            {
                Instantiate(userCard, new Vector3(loc.x + 43 - (i * 10), loc.y + 5, loc.z + 40), Quaternion.Euler(0, 90, 0));
            }
            else
            {
                Instantiate(userCard, new Vector3(loc.x + 38 - ((i - 9) * 10), loc.y + 10, loc.z + 45), Quaternion.Euler(0, 90, 0));
            }
        }
    }

    // ���̾�̽� DB���� �̹��� �ٿ�ε�
    public void DownloadLogoImage()
    {
        filename = "logo_" + myTeam.teamName + ".png";

        storageRef = storage.GetReferenceFromUrl("gs://spopia-image.appspot.com"); // Storage ���

        StorageReference image = storageRef.Child(filename); //  ���� �̸�

        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                StartCoroutine(DownloadStorage(task.Result.ToString()));
            }
        });
    }

    IEnumerator DownloadStorage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            rawImg.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            b_download = true;
        }
    }
}
