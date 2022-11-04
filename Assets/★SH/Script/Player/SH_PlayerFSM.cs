using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;
using JetBrains.Annotations;
using static SH_PlayerRot;
using static UnityEngine.Timeline.AnimationPlayableAsset;

public class SH_PlayerFSM : MonoBehaviourPun
{ 
    public enum State
    {
        NORMAL,
        UIPLAYING,
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
        }
    }

    private void StateNORMAL()
    {
        pm.PlayerMove();
        pr.CusorControll(CursorLockMode.Locked, false);

        if (pr.isClick)
            pr.PlayerRot(ViewState.FIRST, false);
        else if (pr.isClick == false)
            pr.PlayerRot(ViewState.THIRD, false);
    }

    private void StateUIPLAYING()
    {
        pr.CusorControll(CursorLockMode.None, true);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pch.DataInputTable.SetActive(false);
            ChangeState(State.NORMAL);
        }
    }

    public void ChangeState(State s)
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
                break;

            case State.UIPLAYING:
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
        }
    }
}
