using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_LeaderBoardManager : MonoBehaviour
{
    enum Category
    {
        TeamRanking,
        TopScores,
        TopAssists,
    }
    Category category = Category.TeamRanking;

    public GameObject tr;
    public GameObject ts;
    public GameObject ta;

    bool on = true;

    void Start()
    {
        
    }

    void Update()
    {
        switch (category)
        {
            case Category.TeamRanking:
                CategoryTeamRanking();
                break;
            case Category.TopScores:
                CategoryTopScorers();
                break;
            case Category.TopAssists:
                CategoryTopAssists();
                break;
        }
    }

    private void CategoryTeamRanking()
    {
        tr.SetActive(on);
        ts.SetActive(!on);
        ta.SetActive(!on);
    }

    private void CategoryTopScorers()
    {
        tr.SetActive(!on);
        ts.SetActive(on);
        ta.SetActive(!on);
    }

    private void CategoryTopAssists()
    {
        tr.SetActive(!on);
        ts.SetActive(!on);
        ta.SetActive(on);
    }

}
