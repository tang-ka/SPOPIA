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
using SimpleFileBrowser;

// ���̾�̽� �̹���(����)DB ����
// ���ε�
// 1. ����Ƽ���� ������ �����Ͽ� �̹����� �־���
// 2. ����Ƽ���� �̹����� ���÷� ����
// 3. ���÷� ����Ǿ��ִ� ��θ� ���� ���̾�̽� �̹���DB�� ���ε�
// �ٿ�ε�
// 1. ���̾�̽� �̹���DB�� url + �ش� ������ �̸����� ����
// 2. ������ �̹����� �ٷ� ����Ƽ���� ���

public class ImageStorageManager : MonoBehaviour
{
    // ���� ������ ����
    string path;
    public RawImage rawImg;

    // firebase ����
    FirebaseStorage storage;
    StorageReference storageRef;

    // ���� �̸�
    string filename;

    // ���� ��ư�� ��������
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

        //path = EditorUtility.OpenFilePanel("�̹��������� �������ּ���.", "", "png, jpg, jpeg");
        ShowFileBrowser();
    }

    void UploadImage()
    {
        if (path != null)
        {
            WWW www = new WWW("file:///" + path);
            rawImg.texture = www.texture;

            // ���õ� www�� �ؽ��ĸ� �ѱ��.
            StartCoroutine(nameof(SaveImage), www.texture);
        }
    }

    // �̹��� ���ÿ� ����
    IEnumerator SaveImage(Texture2D wwwTexture)
    {
        yield return new WaitForEndOfFrame();

        // ��ȯ
        Texture2D texture2D = new Texture2D(wwwTexture.width, wwwTexture.height, TextureFormat.ARGB32, false);
        texture2D.SetPixels(0, 0, wwwTexture.width, wwwTexture.height, wwwTexture.GetPixels());
        texture2D.Apply();

        // ���� �̸�
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

        // ����
        File.WriteAllBytes(Application.streamingAssetsPath + "/" + filename, texture2D.EncodeToPNG());

        // ������ ���ε�
        //UploadStorage(filename);
    }

    // ���̾�̽� DB�� �̹��� ���ε�
    /*public void UploadStorage(string s)
    {
        storageRef = storage.RootReference.Child(s); // ���� �̸�
        string localPath = "file://" + Application.streamingAssetsPath + "/" + s;
        storageRef.PutFileAsync(localPath).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                print("���̾�̽� ���� �ߵƴ� �漮��~!");
            }
        });
    }*/

    // ���̾�̽� DB���� �̹��� �ٿ�ε�
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

    public void ShowFileBrowser()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));

        FileBrowser.SetDefaultFilter(".jpg");

        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log(FileBrowser.Result[i]);

            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

            string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
        }

        path = FileBrowser.Result[0];

        UploadImage();
    }
}
