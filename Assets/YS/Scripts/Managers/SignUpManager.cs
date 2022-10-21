using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class SignUpManager : MonoBehaviour
{
    public InputField emailInputField, pwInputField, nameInputField;

    void Start()
    {
        PlayFabSettings.TitleId = "7FD3E";
    }

    public void LoginBtn()
    {
        var request = new LoginWithEmailAddressRequest { Email = emailInputField.text, Password = pwInputField.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }


    public void RegisterBtn()
    {
        var request = new RegisterPlayFabUserRequest { Email = emailInputField.text, Password = pwInputField.text, Username = nameInputField.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }


    void OnLoginSuccess(LoginResult result) => print("�α��� ����");

    void OnLoginFailure(PlayFabError error) => print("�α��� ����");

    void OnRegisterSuccess(RegisterPlayFabUserResult result) => print("ȸ������ ����");

    void OnRegisterFailure(PlayFabError error) => print("ȸ������ ����");
}
