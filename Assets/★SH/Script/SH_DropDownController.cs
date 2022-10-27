using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SH_DropDownController : MonoBehaviour
{
    TMP_Dropdown options;

    List<string> optionList = new List<string>();

    void Start()
    {
        options = this.GetComponent<TMP_Dropdown>();

        options.ClearOptions();

        // leagueData.teams �̿��ؼ� �ʱ�ȭ �ϱ�
        optionList.Add("FC XR");
        optionList.Add("FC AI");
        optionList.Add("FC Net");
        optionList.Add("FC Cre");

        options.AddOptions(optionList);
    }

    void Update()
    {
        
    }
}
