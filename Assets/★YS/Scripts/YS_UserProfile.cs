using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YS_UserProfile : MonoBehaviour
{
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

        //canvas.transform.Find("BackNumber").gameObject.GetComponent<Text>().text
        canvas.transform.Find("Position").gameObject.GetComponent<Text>().text = DBManager.instance.myData.position;
        canvas.transform.Find("NickName").gameObject.GetComponent<Text>().text = DBManager.instance.myData.nickName;
        canvas.transform.Find("Height").gameObject.GetComponent<Text>().text = DBManager.instance.myData.height.ToString();
        canvas.transform.Find("Weight").gameObject.GetComponent<Text>().text = DBManager.instance.myData.weight.ToString();
    }
}
