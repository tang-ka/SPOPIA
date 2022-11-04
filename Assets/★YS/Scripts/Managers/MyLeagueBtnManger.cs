using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyLeagueBtnManger : MonoBehaviour
{
    public GameObject teamInfoPage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateLeague()
    {
        SceneManager.LoadScene("YS_MapTypeScene");
    }

    public void CreateTeam()
    {
        if(teamInfoPage.activeSelf == false)
        {
            teamInfoPage.SetActive(true);
        }
        else
        {
            teamInfoPage.SetActive(false);
        }
    }

    public void EnterLeague()
    {
        SceneManager.LoadScene("LeagueAreaScene");
    }
}
