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
    public GameObject teamInfoPage, teamPage, userPage, userInfoPage;
    public InputField inputTeamName, inputFormation, inputBackNum;
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

    // ���� ī�� ��ȣ
    int num;

    // Start is called before the first frame update
    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;

        // ó���� ������ ����
        //StartCoroutine(CreateUserCards());
        CreateUserCards();
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
                // ���� ��� �˾�
                userInfoPage.SetActive(true);

                /*// ���� ��� �Ϸ� �˾�
                goodPopUp2.SetActive(true);

                // ����DB���� team����Ʈ�� �˻��Ѵ�. (Add�� ���� ã�� ����)
                for (int i = 0; i < DBManager.instance.leagueInfo.teams.Count; i++)
                {
                    TeamData info = DBManager.instance.leagueInfo.teams[i];

                    // User�� Add�� ���̶��?
                    if(info.teamName == go.GetComponent<TextMesh>().text)
                    {
                        // �� �����Ϳ� teamName�� �߰��Ѵ�.
                        DBManager.instance.myData.teamName = go.GetComponent<TextMesh>().text;

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
                        // ��ġ
                        Vector3 loc = transform.parent.transform.parent.transform.position;
                      
                        if (num < 9)
                        {
                            PhotonNetwork.Instantiate("PlayerProfile", new Vector3(loc.x + 43 - (num * 10), loc.y + 5, loc.z + 40), Quaternion.Euler(0, 90, 0));
                        }
                        else
                        {
                            PhotonNetwork.Instantiate("PlayerProfile", new Vector3(loc.x + 38 - ((num - 9) * 10), loc.y + 10, loc.z + 45), Quaternion.Euler(0, 90, 0));
                        }

                        print("���� ������ ����!");

                        // ã�� ���� �����Ѵ�.
                        break;
                    }
                }*/
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

    public void AddUser()
    {
        // ���� ��� �Ϸ� �˾�
        userInfoPage.SetActive(false);
        goodPopUp2.SetActive(true);

        // ����DB���� team����Ʈ�� �˻��Ѵ�. (Add�� ���� ã�� ����)
        for (int i = 0; i < DBManager.instance.leagueInfo.teams.Count; i++)
        {
            TeamData info = DBManager.instance.leagueInfo.teams[i];

            // User�� Add�� ���̶��?
            if (info.teamName == go.GetComponent<TextMesh>().text)
            {
                // �� �����Ϳ� teamName, backnumber�� �߰��Ѵ�.
                DBManager.instance.myData.teamName = go.GetComponent<TextMesh>().text;
                DBManager.instance.myData.backNumber = int.Parse(inputBackNum.text);

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
                // ��ġ
                Vector3 loc = transform.parent.transform.parent.transform.position;

                if (num < 9)
                {
                    PhotonNetwork.Instantiate("PlayerProfile", new Vector3(loc.x + 43 - (num * 10), loc.y + 5, loc.z + 40), Quaternion.Euler(0, 90, 0));
                }
                else
                {
                    PhotonNetwork.Instantiate("PlayerProfile", new Vector3(loc.x + 38 - ((num - 9) * 10), loc.y + 10, loc.z + 45), Quaternion.Euler(0, 90, 0));
                }

                print("���� ������ ����!");

                // ã�� ���� �����Ѵ�.
                break;
            }
        }
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
    public void Cancel2()
    {
        userInfoPage.SetActive(false);
    }

    void CreateUserCards()
    {
        for (int i = 0; DBManager.instance.leagueInfo.teams.Count > i; i++)
        {
            if (go.GetComponent<TextMesh>().text == DBManager.instance.leagueInfo.teams[i].teamName)
            {
                myTeam = DBManager.instance.leagueInfo.teams[i];

                break;
            }
        }

        for (num = 0; myTeam.users.Count > num; num++)
        {
            // ���� ������ ����
            GameObject userCard;
            userCard = (GameObject)Resources.Load("YS/UserCard");

            // ��ġ
            Vector3 loc = transform.parent.transform.parent.transform.position;

            // ī�� ���� ����
            // �̹��� ����
            DownloadLogoImage();
            userCard.transform.Find("Canvas").transform.Find("TeamLogo").transform.Find("Logo").gameObject.GetComponent<RawImage>().texture = rawImg.texture;

            // �̹��� ����
            DownloadBodyImage();

            userCard.transform.Find("Canvas").transform.Find("Profile").transform.Find("ProfileImage").gameObject.GetComponent<RawImage>().texture = rawImg.texture;
            userCard.transform.Find("Canvas").transform.Find("BackNumber").gameObject.GetComponent<Text>().text = myTeam.users[num].backNumber.ToString();
            userCard.transform.Find("Canvas").transform.Find("Position").gameObject.GetComponent<Text>().text = myTeam.users[num].position;
            userCard.transform.Find("Canvas").transform.Find("NickName").gameObject.GetComponent<Text>().text = myTeam.users[num].nickName;
            userCard.transform.Find("Canvas").transform.Find("Name").gameObject.GetComponent<Text>().text = myTeam.users[num].realName;
            userCard.transform.Find("Canvas").transform.Find("Height").gameObject.GetComponent<Text>().text = myTeam.users[num].height.ToString() + "cm";
            userCard.transform.Find("Canvas").transform.Find("Weight").gameObject.GetComponent<Text>().text = myTeam.users[num].weight.ToString() + "kg";

            if (num < 9)
            {
                Instantiate(userCard, new Vector3(loc.x + 43 - (num * 10), loc.y + 5, loc.z + 40), Quaternion.Euler(0, 90, 0));
            }
            else
            {
                Instantiate(userCard, new Vector3(loc.x + 38 - ((num - 9) * 10), loc.y + 10, loc.z + 45), Quaternion.Euler(0, 90, 0));
            }
        }
    }

    /*IEnumerator CreateUserCards()
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
    }*/

    // ���̾�̽� DB���� �̹��� �ٿ�ε�
    /*public void DownloadLogoImage()
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
    }*/
    public void DownloadLogoImage()
    {
        filename = "logo_" + myTeam.teamName + ".png";

        byte[] byteTexture = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/" + filename);

        if(byteTexture.Length > 0)
        {
            Texture2D t = new Texture2D(0, 0);
            t.LoadImage(byteTexture);

            rawImg.texture = t;
        }
    }

    public void DownloadBodyImage()
    {
        filename = "body_" + DBManager.instance.myData.nickName + ".png";

        byte[] byteTexture = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/" + filename);

        if (byteTexture.Length > 0)
        {
            Texture2D t = new Texture2D(0, 0);
            t.LoadImage(byteTexture);

            rawImg.texture = t;
        }
    }

    /*IEnumerator DownloadStorage(string url)
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
    }*/
}
