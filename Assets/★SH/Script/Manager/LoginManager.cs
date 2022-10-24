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
        PhotonNetwork.ConnectUsingSettings();
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
}
