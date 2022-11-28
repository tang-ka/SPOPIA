using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YS_UserProfile : MonoBehaviour
{
    // 파일 이름
    string filename;

    // Start is called before the first frame update
    void Start()
    {
        // 만들어질 때, 정보 설정해주기
        SetInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetInfo()
    {
        GameObject canvas = transform.Find("Canvas").gameObject;

        // 카드 정보 설정
        if (!canvas.transform.Find("TeamLogo").transform.Find("Logo").gameObject.GetComponent<RawImage>().texture)
        {
            DownloadLogoImage();
        }

        //canvas.transform.Find("TeamLogo").transform.Find("Logo").gameObject.GetComponent<RawImage>().texture = rawImg.texture;
        canvas.transform.Find("BackNumber").gameObject.GetComponent<Text>().text = DBManager.instance.myData.backNumber.ToString();
        canvas.transform.Find("Position").gameObject.GetComponent<Text>().text = DBManager.instance.myData.position;
        canvas.transform.Find("NickName").gameObject.GetComponent<Text>().text = DBManager.instance.myData.nickName;
        canvas.transform.Find("Name").gameObject.GetComponent<Text>().text = DBManager.instance.myData.realName;
        canvas.transform.Find("Height").gameObject.GetComponent<Text>().text = DBManager.instance.myData.height.ToString() + "cm";
        canvas.transform.Find("Weight").gameObject.GetComponent<Text>().text = DBManager.instance.myData.weight.ToString() + "kg";
    }

    public void DownloadLogoImage()
    {
        filename = "logo_" + DBManager.instance.myData.teamName + ".png";

        byte[] byteTexture = System.IO.File.ReadAllBytes(Application.streamingAssetsPath + "/" + filename);

        if (byteTexture.Length > 0)
        {
            Texture2D t = new Texture2D(0, 0);
            t.LoadImage(byteTexture);

            transform.Find("Canvas").transform.Find("TeamLogo").transform.Find("Logo").gameObject.GetComponent<RawImage>().texture = t;
        }
    }
}
