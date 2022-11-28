using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using static UnityEngine.ParticleSystem;
using Unity.VisualScripting;

public class SH_BlueTeam : MonoBehaviourPun
{

    void Start()
    {
        
    }

    void Update()
    {

    }

    [PunRPC]
    public void RPC_SyncText(string _pieceName, string _s, bool isBackNum)
    {
        if (isBackNum)
            transform.Find(_pieceName).GetComponent<SH_PieceWindow>().SyncBackNumber(_s);
        else
            transform.Find(_pieceName).GetComponent<SH_PieceWindow>().SyncName(_s);
    }

    [PunRPC]
    public void RPC_SyncDistance(string _pieceName, string _firstName, string _secondName)
    {
        transform.Find(_pieceName).GetComponent<SH_PieceWindow>().SyncDistance(_firstName, _secondName);
    }

    [PunRPC]
    public void RPC_DistDelete(string _pieceName)
    {
        transform.Find(_pieceName).GetComponent<SH_PieceWindow>().DistDelete();
    }

    [PunRPC]
    public void RPC_SyncArrow(string _pieceName, string _startName, Vector3 _end,
        bool _rpcFlag, float _dist, string _distance, bool _m0Flag, bool _m1Flag)
    {
        transform.Find(_pieceName).GetComponent<SH_PieceWindow>().
            SyncArrow(_startName, _end, _rpcFlag, _dist, _distance, _m0Flag, _m1Flag);
    }

    [PunRPC]
    public void RPC_ArrowDelete(string _pieceName)
    {
        transform.Find(_pieceName).GetComponent<SH_PieceWindow>().ArrowDelete();
    }
}
