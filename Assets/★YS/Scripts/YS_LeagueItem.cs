using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class YS_LeagueItem : MonoBehaviour
{
    public Button btn;
    LcManager lcManager;

    // Start is called before the first frame update
    void Start()
    {
        lcManager = GameObject.Find("LeagueChoiceManager").GetComponent<LcManager>();

        btn.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        lcManager.btnLeagueName = DBManager.instance.leagueInfo.leagueName;
    }
}
