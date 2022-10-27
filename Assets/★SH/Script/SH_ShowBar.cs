using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_ShowBar : MonoBehaviour
{
    public UserData userData = new UserData();

    public Text playerName;
    public Button btnDelete;
    void Start()
    {
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

}
