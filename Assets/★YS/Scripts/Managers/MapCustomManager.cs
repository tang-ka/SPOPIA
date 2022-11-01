using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCustomManager : MonoBehaviour
{
    // 커스텀 생성 오브젝트
    public GameObject obj;
    // 플레이어 할당
    public GameObject player;
    // 탭
    public GameObject[] tabs = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
        // 플레이어 할당
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateObj()
    {
        GameObject Go = Instantiate(obj, Vector3.zero, Quaternion.identity);
        Go.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 30f);

        DBManager.instance.createdObj.Add(Go);
        DBManager.instance.createdPrefab.Add(obj);
    }

    public void ChangeTab()
    {
        for(int i = 0; i < tabs.Length; i++)
        {
            if(tabs[i].transform.name == this.name)
            {
                tabs[i].SetActive(true);
            }
            else if(tabs[i].transform.name != this.name)
            {
                tabs[i].SetActive(false);
            }
        }
    }

    public void SaveJson()
    {
        for(int i = 0; i < DBManager.instance.createdObj.Count; i++)
        {
            SaveJsonInfo info = new SaveJsonInfo();

            info.name = DBManager.instance.createdPrefab[i].name;
            info.position = DBManager.instance.createdObj[i].transform.position;
            info.eulerAngle = DBManager.instance.createdObj[i].transform.eulerAngles;
            info.localScale = DBManager.instance.createdObj[i].transform.localScale;

            DBManager.instance.arrayJson.datas.Add(info);
        }

        // DB에 저장
        DBManager.instance.SaveJsonMapCustom(DBManager.instance.arrayJson, "MapData");
    }
}
