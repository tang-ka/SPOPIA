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

        // ���� ������ �޾ƿ���
        DBManager.instance.GetData(DBManager.instance.testDBid2, "LeagueData");
        for (int i = 0; i < DBManager.instance.leagues.leagueDatas.Count; i++)
        {
            // ���� �ش� ���� �����͸� ���� ����Ʈ���� ã�Ƽ� leagueInfo�� �־��ֱ�
            if (DBManager.instance.leagues.leagueDatas[i].leagueName == lcManager.btnLeagueName)
            {
                DBManager.instance.leagueInfo = DBManager.instance.leagues.leagueDatas[i];

                break;
            }
        }
    }*/

    public void EnterLeague()
    {
        // ���� �̸� �־��ֱ�
        lcManager.btnLeagueName = name.GetComponent<Text>().text;

        // ���� ������ �޾ƿ���
        DBManager.instance.GetData(DBManager.instance.testDBid2, "LeagueData");
        for (int i = 0; i < DBManager.instance.leagues.leagueDatas.Count; i++)
        {
            // ���� �ش� ���� �����͸� ���� ����Ʈ���� ã�Ƽ� leagueInfo�� �־��ֱ�
            if (DBManager.instance.leagues.leagueDatas[i].leagueName == lcManager.btnLeagueName)
            {
                DBManager.instance.leagueInfo = DBManager.instance.leagues.leagueDatas[i];

                break;
            }
        }

        // ���� ����
        lcManager.JoinLeague();
    }
}
