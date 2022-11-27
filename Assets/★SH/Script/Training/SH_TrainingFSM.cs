using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

public class SH_TrainingFSM : MonoBehaviourPun
{
    public SH_TrainingFSM instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;


    }

    [SerializeField]
    List<GameObject> coaches = new List<GameObject>();
    [SerializeField]
    List<GameObject> players = new List<GameObject>();

    public enum Time
    {
        EXPLANATION,
        PRACTICE,
        FEEDBACK,
    }
    public Time time = Time.EXPLANATION;

    void Start()
    {
        coaches = PgManager.instance.coachList;
        players = PgManager.instance.playerList;
    }

    void Update()
    {
        switch (time)
        {
            case Time.EXPLANATION:
                TimeEXPLANATION();
                break;

            case Time.PRACTICE:
                TimePRACTICE();
                break;

            case Time.FEEDBACK:
                TimeFEEDBACK();
                break;
        }
    }

    private void TimeEXPLANATION()
    {
        //print("It is Explanation time~~!!");
    }

    private void TimePRACTICE()
    {
        print("It is Practice time~~!!");
    }

    private void TimeFEEDBACK()
    {
        print("It is Feedback time~~!!");
    }

    public void ChangeTime(Time t)
    {
        photonView.RPC(nameof(RPC_ChangeTime), RpcTarget.All, t);
    }
    [PunRPC]
    public void RPC_ChangeTime(Time t)
    {
        if (time == t)
        {
            print("같은 타이밍 입니다만! : " + t.ToString());
            return;
        }

        EndTime(t);

        time = t;

        switch (time)
        {
            case Time.EXPLANATION:
                for (int i = 0; i < coaches.Count; i++)
                {
                    SH_PlayerFSM fsm = coaches[i].GetComponent<SH_PlayerFSM>();
                    fsm.ChangeState(SH_PlayerFSM.State.TEACH);
                }
                for (int i = 0; i < players.Count; i++)
                {
                    SH_PlayerFSM fsm = players[i].GetComponent<SH_PlayerFSM>();
                    fsm.ChangeState(SH_PlayerFSM.State.LEARN);
                }
                break;

            case Time.PRACTICE:
                for (int i = 0; i < coaches.Count; i++)
                {
                    SH_PlayerFSM fsm = coaches[i].GetComponent<SH_PlayerFSM>();
                    fsm.ChangeState(SH_PlayerFSM.State.NORMAL);
                }
                for (int i = 0; i < players.Count; i++)
                {
                    SH_PlayerFSM fsm = players[i].GetComponent<SH_PlayerFSM>();
                    fsm.ChangeState(SH_PlayerFSM.State.NORMAL);
                }
                break;

            case Time.FEEDBACK:
                break;
        }
    }

    public void EndTime(Time t)
    {
        //photonView.RPC("RpcEndTime", RpcTarget.All, s);
        switch (t)
        {
            case Time.EXPLANATION:
                break;

            case Time.PRACTICE:
                break;

            case Time.FEEDBACK:
                break;
        }
    }
}
