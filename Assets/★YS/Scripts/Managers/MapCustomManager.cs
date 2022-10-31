using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveJsonInfo
{
    public GameObject go;
    public Vector3 position;
    public Vector3 eulerAngle;
    public Vector3 localScale;
}

public class ArrayJson
{
    public List<SaveJsonInfo> datas;
}

public class MapCustomManager : MonoBehaviour
{
    // 커스텀 생성 오브젝트
    public GameObject obj;
    // 플레이어 할당
    public GameObject player;
    // 탭
    public GameObject[] tabs = new GameObject[2];
    // Json배열
    ArrayJson arrayJson = new ArrayJson();

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

        // 만들어진 오브젝트를 리스트에 추가
        SaveJsonInfo info = new SaveJsonInfo();

        info.go = obj;
        info.position = obj.transform.position;
        info.eulerAngle = obj.transform.eulerAngles;
        info.localScale = obj.transform.localScale;

        arrayJson.datas.Add(info);
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
        // arrayJson을 Json으로 변환
        //string jsonData = JsonUtility.ToJson(arrayJson);

        // DB에 저장
        DBManager.instance.SaveJsonMapCustom(arrayJson, "MapData");
    }

    public void LoadJson()
    {

    }
}
