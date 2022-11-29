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
    Color c; // 팀 이름 색(알파)
    public GameObject go; // 팀 이름 오브젝트 동적 할당

    // 참여 완료 팝업
    public GameObject goodPopUp, goodPopUp2;

    // 유저 카드 관련
    TeamData myTeam;

    // 팀 로고
    public RawImage rawImg;

    // firebase 관련
    FirebaseStorage storage;
    StorageReference storageRef;
    bool b_download = false;

    // 파일 이름
    string filename;

    // 유저 카드 번호
    int num;

    // Start is called before the first frame update
    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;

        // 처음에 유저들 생성
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

        // 팀 생성 되었으면, 페이지 바꿔주기 (유저생성 있는 페이지로)
        if(go != null)
        {
            teamPage.SetActive(false);
            userPage.SetActive(true);
        }
    }

    void BtnClick() // User추가
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
                // 선수 등록 팝업
                userInfoPage.SetActive(true);

                /*// 선수 등록 완료 팝업
                goodPopUp2.SetActive(true);

                // 리그DB에서 team리스트를 검사한다. (Add될 팀을 찾기 위해)
                for (int i = 0; i < DBManager.instance.leagueInfo.teams.Count; i++)
                {
                    TeamData info = DBManager.instance.leagueInfo.teams[i];

                    // User가 Add될 팀이라면?
                    if(info.teamName == go.GetComponent<TextMesh>().text)
                    {
                        // 내 데이터에 teamName을 추가한다.
                        DBManager.instance.myData.teamName = go.GetComponent<TextMesh>().text;

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
                        // 위치
                        Vector3 loc = transform.parent.transform.parent.transform.position;
                      
                        if (num < 9)
                        {
                            PhotonNetwork.Instantiate("PlayerProfile", new Vector3(loc.x + 43 - (num * 10), loc.y + 5, loc.z + 40), Quaternion.Euler(0, 90, 0));
                        }
                        else
                        {
                            PhotonNetwork.Instantiate("PlayerProfile", new Vector3(loc.x + 38 - ((num - 9) * 10), loc.y + 10, loc.z + 45), Quaternion.Euler(0, 90, 0));
                        }

                        print("개인 프로필 생성!");

                        // 찾는 것을 종료한다.
                        break;
                    }
                }*/
            }

            /*else if(hit.transform.gameObject.name == "testCube")
            {
                // 개인 프로필 생성
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
        // 팝업 꺼주기
        goodPopUp.SetActive(false);
        teamInfoPage.SetActive(false);

        // 위치
        Vector3 loc = transform.parent.transform.parent.transform.position;

        // 팀명 생성될 때, 이펙트
        PhotonNetwork.Instantiate("TeamNameEffect", new Vector3(loc.x, loc.y + 100f, loc.z), Quaternion.identity);

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

        // RPC 보내기
        photonView.RPC(nameof(RpcAddTeam), RpcTarget.OthersBuffered, inputTeamName.text, loc);
    }

    public void AddUser()
    {
        // 선수 등록 완료 팝업
        userInfoPage.SetActive(false);
        goodPopUp2.SetActive(true);

        // 리그DB에서 team리스트를 검사한다. (Add될 팀을 찾기 위해)
        for (int i = 0; i < DBManager.instance.leagueInfo.teams.Count; i++)
        {
            TeamData info = DBManager.instance.leagueInfo.teams[i];

            // User가 Add될 팀이라면?
            if (info.teamName == go.GetComponent<TextMesh>().text)
            {
                // 내 데이터에 teamName, backnumber를 추가한다.
                DBManager.instance.myData.teamName = go.GetComponent<TextMesh>().text;
                DBManager.instance.myData.backNumber = int.Parse(inputBackNum.text);

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
                // 위치
                Vector3 loc = transform.parent.transform.parent.transform.position;

                if (num < 9)
                {
                    PhotonNetwork.Instantiate("PlayerProfile", new Vector3(loc.x + 43 - (num * 10), loc.y + 5, loc.z + 40), Quaternion.Euler(0, 90, 0));
                }
                else
                {
                    PhotonNetwork.Instantiate("PlayerProfile", new Vector3(loc.x + 38 - ((num - 9) * 10), loc.y + 10, loc.z + 45), Quaternion.Euler(0, 90, 0));
                }

                print("개인 프로필 생성!");

                // 찾는 것을 종료한다.
                break;
            }
        }
    }

    [PunRPC]
    void RpcAddTeam(string _text, Vector3 _loc)
    {
        // 경기장에 팀명 띄우기
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
            // 개인 프로필 생성
            GameObject userCard;
            userCard = (GameObject)Resources.Load("YS/UserCard");

            // 위치
            Vector3 loc = transform.parent.transform.parent.transform.position;

            // 카드 정보 설정
            // 이미지 세팅
            DownloadLogoImage();
            userCard.transform.Find("Canvas").transform.Find("TeamLogo").transform.Find("Logo").gameObject.GetComponent<RawImage>().texture = rawImg.texture;

            // 이미지 세팅
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
            // 개인 프로필 생성
            GameObject userCard;
            userCard = (GameObject)Resources.Load("YS/UserCard");

            // 위치
            Vector3 loc = transform.parent.transform.parent.transform.position;

            // 카드 정보 설정
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

    // 파이어베이스 DB에서 이미지 다운로드
    /*public void DownloadLogoImage()
    {
        filename = "logo_" + myTeam.teamName + ".png";

        storageRef = storage.GetReferenceFromUrl("gs://spopia-image.appspot.com"); // Storage 경로

        StorageReference image = storageRef.Child(filename); //  파일 이름

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
