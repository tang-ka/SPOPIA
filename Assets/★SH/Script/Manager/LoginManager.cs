using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public InputField inputID;
    public InputField inputPW;

    public Button btnLogin;

    // Start is called before the first frame update
    void Start()
    {
        inputPW.onValueChanged.AddListener(OnValueChanged);
        inputPW.onSubmit.AddListener(OnSubmit);
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
        PhotonNetwork.ConnectUsingSettings();
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
}
