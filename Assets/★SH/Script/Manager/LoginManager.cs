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

    // ȸ�����Կ�(����)
    public InputField emailSignUp, pwSignUp, nameSignUp;
    // DB�Ŵ���(����)
    DBManager DBManager;

    // Start is called before the first frame update
    void Start()
    {
        //inputPW.onValueChanged.AddListener(OnValueChanged); // ���� : �������� ���Ƴ����ϴ�.
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
        // �α��� ������ ��ġ�ϸ�
        if (btnLogin.interactable || true)
        {
            //���� ����!
            OnClickLogin();
        }
        // ��ġ���� ������
        else
        {
            print("�α��� ������ ��ġ���� �ʽ��ϴ�.");
        }
    }

    public void OnClickLogin()
    {
        //���� ���� ��û
        //PhotonNetwork.ConnectUsingSettings();

        // �÷����� �α��� (����)
        var request = new LoginWithEmailAddressRequest { Email = inputID.text, Password = inputPW.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    public override void OnConnected()
    {
        base.OnConnected();
        print("�α��� ����..");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("SPOPIA�� ���Ű� ȯ���մϴ�!!");
        PhotonNetwork.LoadLevel("WorldChoiceScene");
    }

    // �÷����� ȸ������ (����)
    public void RegisterBtn()
    {
        var request = new RegisterPlayFabUserRequest { Email = emailSignUp.text, Password = pwSignUp.text, Username = nameSignUp.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
    }

    void OnLoginSuccess(LoginResult result)
    {
        print("�α��� ����");
        DBManager.GetLeaderboard(result.PlayFabId);
    }

    void OnLoginFailure(PlayFabError error) => print("�α��� ����");

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        print("ȸ������ ����");
        DBManager.SetStat();
    }

    void OnRegisterFailure(PlayFabError error) => print("ȸ������ ����");

    
}
