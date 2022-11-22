using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SH_UserIcon : MonoBehaviourPun
{
    public Text nickName;
    public string backNumber;
    public int viewID;
    //public string image;

    void Start()
    {
        nickName.text = DBManager.instance.myData.nickName;
        backNumber = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        //backNumber = DBManager.instance.myData.backNum;
        //Init(DBManager.instance.myData.nickName, PhotonNetwork.CurrentRoom.PlayerCount.ToString(),
        //    gameObject.GetPhotonView().ViewID);
    }

    //void Init(string nickName, string backNumber, int viewID)
    //{
    //    photonView.RPC(nameof(RPC_Init), RpcTarget.All, nickName, backNumber, viewID);
    //}
    //[PunRPC]
    //public void RPC_Init(string nickName, string backNumber, int viewID)
    //{
    //    this.nickName.text = nickName;
    //    this.backNumber = backNumber;
    //    this.viewID = viewID;
    //}

    void Update()
    {
        
    }
}
