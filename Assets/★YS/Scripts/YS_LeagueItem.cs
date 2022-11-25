using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class YS_LeagueItem : MonoBehaviour
{
    public Button btn, enterBtn;
    public Text name;
    LcManager lcManager;

    // Start is called before the first frame update
    void Start()
    {
        lcManager = GameObject.Find("LeagueChoiceManager").GetComponent<LcManager>();

        //btn.onClick.AddListener(OnClick);
        enterBtn.onClick.AddListener(EnterLeague);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void OnClick()
    {
        lcManager.btnLeagueName = name.GetComponent<Text>().text;

        // 리그 데이터 받아오기
        DBManager.instance.GetData(DBManager.instance.testDBid2, "LeagueData");
        for (int i = 0; i < DBManager.instance.leagues.leagueDatas.Count; i++)
        {
            // 누른 해당 리그 데이터를 리그 리스트에서 찾아서 leagueInfo에 넣어주기
            if (DBManager.instance.leagues.leagueDatas[i].leagueName == lcManager.btnLeagueName)
            {
                DBManager.instance.leagueInfo = DBManager.instance.leagues.leagueDatas[i];

                break;
            }
        }
    }*/

    public void EnterLeague()
    {
        // 리그 이름 넣어주기
        lcManager.btnLeagueName = name.GetComponent<Text>().text;

        // 리그 데이터 받아오기
        DBManager.instance.GetData(DBManager.instance.testDBid2, "LeagueData");
        for (int i = 0; i < DBManager.instance.leagues.leagueDatas.Count; i++)
        {
            // 누른 해당 리그 데이터를 리그 리스트에서 찾아서 leagueInfo에 넣어주기
            if (DBManager.instance.leagues.leagueDatas[i].leagueName == lcManager.btnLeagueName)
            {
                DBManager.instance.leagueInfo = DBManager.instance.leagues.leagueDatas[i];

                break;
            }
        }

        // 리그 입장
        lcManager.JoinLeague();
    }
}
