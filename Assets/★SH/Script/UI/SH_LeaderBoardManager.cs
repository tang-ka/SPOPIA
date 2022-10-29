using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SH_LeaderBoardManager : MonoBehaviour
{
    enum Category
    {
        TEAMRANKING,
        TOPSCORES,
        TOPASSISTS,
    }
    Category category = Category.TEAMRANKING;
    Category preCategory;

    public GameObject tr;
    public GameObject ts;
    public GameObject ta;

    public GameObject rankBarFactory;
    public Transform rankBarParent;

    List<GameObject> ranking;

    bool on = true;
    int index = 1;

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            ranking.Add();
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            ChangeIndex();
        }

        switch (category)
        {
            case Category.TEAMRANKING:
                CategoryTEAMRANKING();
                break;

            case Category.TOPSCORES:
                CategoryTopScorers();
                break;

            case Category.TOPASSISTS:
                CategoryTOPASSISTS();
                break;
        }
    }

    private void CategoryTEAMRANKING()
    {
        print("Show me the Team Ranking");
    }

    private void CategoryTopScorers()
    {
        print("Show me the Top Scorers");
    }

    private void CategoryTOPASSISTS()
    {
        print("Show me the Top Assists");
    }

    void ChangeCategory(Category c)
    {
        if (category == c)
        {
            print("응 아니야~");
            return;
        }

        preCategory = category;
        EndCategory(preCategory);

        category = c;

        switch (category)
        {
            case Category.TEAMRANKING:
                tr.SetActive(on);
                ts.SetActive(!on);
                ta.SetActive(!on);

                GameObject bar = Instantiate(rankBarFactory, rankBarParent);
                break;

            case Category.TOPSCORES:
                tr.SetActive(!on);
                ts.SetActive(on);
                ta.SetActive(!on);
                break;

            case Category.TOPASSISTS:
                tr.SetActive(!on);
                ts.SetActive(!on);
                ta.SetActive(on);
                break;
        }
    }

    private void EndCategory(Category c)
    {
        switch (c)
        {
            case Category.TEAMRANKING:
                break;

            case Category.TOPSCORES:
                break;

            case Category.TOPASSISTS:
                break;
        }
    }

    void ChangeIndex()
    {
        index++;
        index %= 3;
        ChangeCategory((Category)index);
    }

}
