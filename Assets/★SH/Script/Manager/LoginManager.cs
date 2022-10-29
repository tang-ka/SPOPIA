using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public InputField inputID;
    public InputField inputPW;
    public Button btnLogin;

    // 회원가입용(영수)
    public InputField emailSignUp, pwSignUp, nameSignUp;
    // DB매니저(영수)
    DBManager DBManager;

    // Start is called before the first frame update
    void Start()
    {
        //inputPW.onValueChanged.AddListener(OnValueChanged); // 영수 : 오류나서 막아놨습니다.
        inputPW.onSubmit.AddListener(OnSubmit);

        // DBManager
        DBManager = GameObject.Find("DBManager").GetComponent<DBManager>();
    }

    public void OnValueChanged(string s)
    {
        btnLogin.interactable = s.Length > 0 && inputID.text.Length > 0;
    }

    public void OnSubmit(string s)
    {
        // 로그인 정보가 일치하면
        if (btnLogin.interactable || true)
        {
            //접속 하자!
            OnClickLogin();
        }
        // 일치하지 않으면
        else
        {
            print("로그인 정보가 일치하지 않습니다.");
        }
    }

    public void OnClickLogin()
    {
        //서버 접속 요청
        //PhotonNetwork.ConnectUsingSettings();

        // 플레이팹 로그인 (영수)
        var request = new LoginWithEmailAddressRequest { Email = inputID.text, Password = inputPW.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    public override void OnConnected()
    {
        base.OnConnected();
        print("로그인 성공..");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("SPOPIA에 오신걸 환영합니다!!");
        PhotonNetwork.LoadLevel("WorldChoiceScene");
    }

    // 플레이팹 회원가입 (영수)
    public void RegisterBtn()
    {
        var request = new RegisterPlayFabUserRequest { Email = emailSignUp.text, Password = pwSignUp.text, Username = nameSignUp.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }

    void OnLoginSuccess(LoginResult result)
    {
        print("로그인 성공");
        DBManager.GetLeaderboard(result.PlayFabId);
    }

    void OnLoginFailure(PlayFabError error) => print("로그인 실패");

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        print("회원가입 성공");
        DBManager.SetStat();
    }

    void OnRegisterFailure(PlayFabError error) => print("회원가입 실패");

    
}
