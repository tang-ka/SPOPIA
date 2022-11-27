using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class AvatarManager : MonoBehaviour
{
    public InputField nickNameInputField, ageInputField, positionInputField, heightInputField, weightInputField;
    public int idx = -1;

    // 체크 표시
    public GameObject checkMark, checkNoMark;

    // 확인 메시지
    public GameObject registerGoodPage, reallyPage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectAvatar()
    {
        idx = int.Parse(EventSystem.current.currentSelectedGameObject.name);
    }

    public void CheckNickName()
    {
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = nickNameInputField.text };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, (result) => { checkMark.SetActive(true); checkNoMark.SetActive(false); }, (error) => { checkNoMark.SetActive(true); checkMark.SetActive(false); });
    }

    public void Ok()
    {
        UserData data = new UserData();

        data.avatarIdx = idx;
        data.nickName = nickNameInputField.text;
        data.age = int.Parse(ageInputField.text);
        data.position = positionInputField.text;
        data.height = int.Parse(heightInputField.text);
        data.weight = int.Parse(weightInputField.text);

        DBManager.instance.SaveJsonUser(data, "MyData");

        // 바로 Select씬으로 가게끔
        SceneManager.LoadScene("YS_SelectScene");
    }

    public void Cancel()
    {
        SceneManager.LoadScene("LoginScene");
    }

    public void RegisterGoodPage()
    {
        if(registerGoodPage.activeSelf == false)
        {
            registerGoodPage.SetActive(true);
        }
        else if(registerGoodPage.activeSelf == true)
        {
            registerGoodPage.SetActive(false);
        }
    }

    public void ReallyPage()
    {
        if (reallyPage.activeSelf == false)
        {
            reallyPage.SetActive(true);
        }
        else if (reallyPage.activeSelf == true)
        {
            reallyPage.SetActive(false);
        }
    }
}
