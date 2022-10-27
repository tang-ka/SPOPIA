using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_ShowBar : MonoBehaviour
{
    public UserData userData = new UserData();

    public Text playerName;
    public Button btnDelete;

    public SH_InputMatchResult matchResult;
    public Action<SH_ShowBar> deleteList;

    void Start()
    {
        matchResult = GetComponentInParent<SH_InputMatchResult>();
        deleteList = matchResult.DeleteListScorer;
        deleteList += matchResult.DeleteListAssist;
    }

    void Update()
    {
        
    }

    public void Init(UserData data)
    {
        userData = data;
        playerName.text = data.nickName;
    }

    public void AddGoal()
    {
        userData.goal++;
    }

    public void AddAssist()
    {
        userData.assist++;
    }

    public void OnCLickDelete()
    {
        Destroy(gameObject);
        deleteList(gameObject.GetComponent<SH_ShowBar>());
    }

}
