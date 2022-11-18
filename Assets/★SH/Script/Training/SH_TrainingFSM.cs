using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SH_TrainingFSM : MonoBehaviour
{
    public SH_TrainingFSM instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public enum Time
    {
        EXPLANATION,
        PRACTICE,
        FEEDBACK,
    }
    public Time time = Time.EXPLANATION;



    void Start()
    {
        
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
                break;

            case Time.PRACTICE:
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
