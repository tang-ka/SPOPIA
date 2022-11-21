using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SH_UserIcon : MonoBehaviourPun
{
    public Text nickName;
    public string backNumber;

    void Start()
    {
        nickName.text = DBManager.instance.myData.nickName;
        backNumber = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        //backNumber = DBManager.instance.myData.backNum;
    }

    void Update()
    {
        
    }
}
