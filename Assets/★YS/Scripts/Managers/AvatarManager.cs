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
    public int idx;

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
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, (result) => print("저장성공~^^"), (error) => print("저장실패~^^"));
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
    }

    public void Cancel()
    {
        SceneManager.LoadScene("LoginScene");
    }
}
