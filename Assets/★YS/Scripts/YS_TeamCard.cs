using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Firebase;
using Firebase.Storage;
using Firebase.Extensions;

public class YS_TeamCard : MonoBehaviour
{
    TeamData myTeam;
    public CubeBtnManager cubeManager;

    // 팀 로고
    public RawImage rawImg;

    // firebase 관련
    FirebaseStorage storage;
    StorageReference storageRef;

    // 파일 이름
    string filename;

    // Start is called before the first frame update
    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
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

        if(!canvas.transform.Find("TeamLogo").transform.Find("Logo").gameObject.GetComponent<RawImage>().texture)
        {
            DownloadImage();
        }
        canvas.transform.Find("TeamLogo").transform.Find("Logo").gameObject.GetComponent<RawImage>().texture = rawImg.texture;
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

    public void DownloadImage()
    {
        filename = "logo_" + myTeam.teamName + ".png";

        byte[] byteTexture = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/" + filename);

        if (byteTexture.Length > 0)
        {
            Texture2D t = new Texture2D(0, 0);
            t.LoadImage(byteTexture);

            rawImg.texture = t;
        }
    }

    // 파이어베이스 DB에서 이미지 다운로드
    /*public void DownloadImage()
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
        }
    }*/
}
