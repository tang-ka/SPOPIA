using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class SignUpManager : MonoBehaviour
{
    public InputField emailSignUp, pwSignUp, nameSignUp, idInputField, pwInputField, pwAgain;

    public void LoginBtn()
    {
        var request = new LoginWithEmailAddressRequest { Email = idInputField.text, Password = pwInputField.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }


    public void RegisterBtn()
    {
        if(pwSignUp.text == pwAgain.text)
        {
            var request = new RegisterPlayFabUserRequest { Email = emailSignUp.text, Password = pwSignUp.text, Username = nameSignUp.text };
            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
        }
    }


    void OnLoginSuccess(LoginResult result) => print("�α��� ����");

    void OnLoginFailure(PlayFabError error) => print("�α��� ����");

    void OnRegisterSuccess(RegisterPlayFabUserResult result) => print("ȸ������ ����");

    void OnRegisterFailure(PlayFabError error) => print("ȸ������ ����");
}
