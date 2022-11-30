using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using Firebase;
using Firebase.Storage;
using Firebase.Extensions;
using System;
using System.IO;

// 파이어베이스 이미지(파일)DB 구조
// 업로드
// 1. 유니티에서 파일을 선택하여 이미지를 넣어줌
// 2. 유니티에서 이미지를 로컬로 저장
// 3. 로컬로 저장되어있는 경로를 통해 파이어베이스 이미지DB에 업로드
// 다운로드
// 1. 파이어베이스 이미지DB에 url + 해당 파일의 이름으로 접근
// 2. 접근한 이미지를 바로 유니티에서 사용

public class ImageStorageManager : MonoBehaviour
{
    // 파일 브라우저 관련
    string path;
    public RawImage rawImg;

    // firebase 관련
    FirebaseStorage storage;
    StorageReference storageRef;

    // 파일 이름
    string filename;

    // 무슨 버튼을 눌렀는지
    string btnName;

    // Start is called before the first frame update
    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;

        rawImg = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenFileBrowser()
    {
        btnName = EventSystem.current.currentSelectedGameObject.name;

        path = EditorUtility.OpenFilePanel("이미지파일을 선택해주세요.", "", "png, jpg, jpeg");
        UploadImage();
    }

    void UploadImage()
    {
        if (path != null)
        {
            WWW www = new WWW("file:///" + path);
            rawImg.texture = www.texture;

            // 선택된 www의 텍스쳐를 넘긴다.
            StartCoroutine(nameof(SaveImage), www.texture);
        }
    }

    // 이미지 로컬에 저장
    IEnumerator SaveImage(Texture2D wwwTexture)
    {
        yield return new WaitForEndOfFrame();

        // 변환
        Texture2D texture2D = new Texture2D(wwwTexture.width, wwwTexture.height, TextureFormat.ARGB32, false);
        texture2D.SetPixels(0, 0, wwwTexture.width, wwwTexture.height, wwwTexture.GetPixels());
        texture2D.Apply();

        // 파일 이름
        if (btnName == "ProfileImageUpload")
        {
            filename = "pf_" + DBManager.instance.myData.nickName + ".png";
        }
        else if (btnName == "TeamLogoUpload")
        {
            filename = "logo_" + DBManager.instance.myData.teamName + ".png";
        }
        else if (btnName == "BodyProfileUpload")
        {
            filename = "body_" + DBManager.instance.myData.nickName + ".png";
        }

        // 저장
        File.WriteAllBytes(Application.streamingAssetsPath + "/" + filename, texture2D.EncodeToPNG());

        // 서버에 업로드
        //UploadStorage(filename);
    }

    // 파이어베이스 DB에 이미지 업로드
    /*public void UploadStorage(string s)
    {
        storageRef = storage.RootReference.Child(s); // 저장 이름
        string localPath = "file://" + Application.streamingAssetsPath + "/" + s;
        storageRef.PutFileAsync(localPath).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                print("파이어베이스 저장 잘됐다 욘석아~!");
            }
        });
    }*/

    // 파이어베이스 DB에서 이미지 다운로드
    /*IEnumerator DownloadStorage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        
        yield return request.SendWebRequest();

        if(request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            rawImg.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }

    public void DownloadImage()
    {
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

    public void DownloadImage()
    {
        byte[] byteTexture = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/" + filename);

        if (byteTexture.Length > 0)
        {
            Texture2D t = new Texture2D(0, 0);
            t.LoadImage(byteTexture);

            rawImg.texture = t;
        }
    }
}
