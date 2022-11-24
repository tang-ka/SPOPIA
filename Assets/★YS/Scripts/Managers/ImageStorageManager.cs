using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Firebase;
using Firebase.Storage;
using System;
using System.IO;

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
    public RawImage rawImg, loadTest;

    // firebase ����
    FirebaseStorage storage;
    StorageReference storageRef;

    // Start is called before the first frame update
    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;

        storage = FirebaseStorage.DefaultInstance;
        storageRef = storage.GetReferenceFromUrl("");

        StorageReference image = storageRef.Child(""); //  ���� �̸�

        image.GetDownloadUrlAsync().ContinueWith(task =>
        {
            if(!task.IsFaulted && !task.IsCanceled)
            {
                StartCoroutine(DownloadStorage(Convert.ToString(task.Result)));
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenFileBrowser()
    {
        path = EditorUtility.OpenFilePanel("�̹��������� �������ּ���.", "", "png, jpg, jpeg");
        UploadImage();
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
        //string filename = "1";

        // ����
        File.WriteAllBytes(Application.streamingAssetsPath + "/SavedImage.png", texture2D.EncodeToPNG());

        // ������ ���ε�
        UploadStorage("SavedImage.png");
    }

    // ���̾�̽� DB�� �̹��� ���ε�
    public void UploadStorage()
    {
        storageRef = storage.RootReference.Child(""); // ���� �̸�
        string local = "file://" + Application.streamingAssetsPath + "";
        storageRef.PutFileAsync(local).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                print("���̾�̽� ���� �ߵƴ� �漮��~!");
            }
        });
    }

    // ���̾�̽� DB���� �̹��� �ٿ�ε�
    IEnumerator DownloadStorage(string url)
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
}
