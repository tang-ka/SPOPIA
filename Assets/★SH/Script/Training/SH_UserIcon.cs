using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SH_UserIcon : MonoBehaviourPun
{
    PhotonView borrowPhotonView;

    public Text txtNickName;

    public string nickName;
    public int backNumber;
    public int viewID;
    //public string image;

    private void Awake()
    {
        borrowPhotonView = PgManager.instance.gameObject.GetPhotonView();
    }
    void Start()
    {
        //nickName.text = DBManager.instance.myData.nickName;
        //backNumber = PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        //backNumber = DBManager.instance.myData.backNum;
        //Init(DBManager.instance.myData.nickName, PhotonNetwork.CurrentRoom.PlayerCount.ToString(),
        //    gameObject.GetPhotonView().ViewID);
    }

    //public void Init(int viewID)
    //{
    //    borrowPhotonView.RPC(nameof(RPC_Init), RpcTarget.All, viewID);
    //}
    public void Init(int viewID)
    {
        nickName = PhotonView.Find(viewID).Owner.NickName;
            //GetComponent<SH_PlayerFSM>().sendNickName;

        txtNickName.text = nickName;
        backNumber = PhotonView.Find(viewID).GetComponent<SH_PlayerFSM>().sendBackNumber;
        this.viewID = viewID;
    }

    void Update()
    {
        
    }
}
