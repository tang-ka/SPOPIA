using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using JetBrains.Annotations;
using static SH_PlayerRot;
using static UnityEngine.Timeline.AnimationPlayableAsset;
using Photon.Pun.UtilityScripts;

public class SH_PlayerFSM : MonoBehaviourPun
{ 
    public enum State
    {
        NORMAL,
        UIPLAYING,
        TEACH,
        LEARN,
    }
    public State state = State.NORMAL;
    public State preState;

    SH_PlayerMove pm;
    SH_PlayerRot pr;
    SH_PlayerCrossHair pch;

    void Start()
    {
        pm = GetComponent<SH_PlayerMove>();
        pr = GetComponent<SH_PlayerRot>();
        pch = GetComponent<SH_PlayerCrossHair>();

        pr.CusorControll(CursorLockMode.Locked, false);
    }

    void Update()
    {
        switch (state)
        {
            case State.NORMAL:
                StateNORMAL();
                break;

            case State.UIPLAYING:
                StateUIPLAYING();
                break;

            case State.TEACH:
                StateTEACH();
                break;

            case State.LEARN:
                StateLEARN();
                break;
        }
    }

    private void StateNORMAL()
    {
        pm.PlayerMove();

        if (pr.isTabClick)
            pr.PlayerRot(ViewState.FIRST, false);
        else if (pr.isTabClick == false)
            pr.PlayerRot(ViewState.THIRD, false);
    }

    private void StateUIPLAYING()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pch.dataInputTable.SetActive(false);
            ChangeState(State.NORMAL);
        }
    }

    private void StateTEACH()
    {
        
    }

    private void StateLEARN()
    {
        pm.PlayerMove();
        pr.PlayerRot(ViewState.THIRD, false);
    }


    public void ChangeState(State s)
    {
        photonView.RPC("RPC_ChangeState", RpcTarget.All, s);
    }

    [PunRPC]
    public void RPC_ChangeState(State s)
    {
        if (state == s)
        {
            print("같은 상태 입니다. : " + state);
            return;
        }

        preState = state;
        EndState(preState);

        state = s;

        switch (state)
        {
            case State.NORMAL:
                pr.CusorControll(CursorLockMode.Locked, false);
                break;

            case State.UIPLAYING:
                pr.CusorControll(CursorLockMode.None, true);
                break;

            case State.TEACH:
                pr.CusorControll(CursorLockMode.None, true);
                break;

            case State.LEARN:
                pr.CusorControll(CursorLockMode.Locked, false);
                break;
        }
    }

    public void EndState(State s)
    {
        //photonView.RPC("RpcEndState", RpcTarget.All, s);
        switch (s)
        {
            case State.NORMAL:
                break;

            case State.UIPLAYING:
                break;

            case State.TEACH:
                break;

            case State.LEARN:
                break;
        }
    }
}
