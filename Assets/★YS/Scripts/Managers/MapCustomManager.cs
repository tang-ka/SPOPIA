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
    // Ŀ���� ���� ������Ʈ
    public GameObject obj;
    // �÷��̾� �Ҵ�
    public GameObject player;
    // ��
    public GameObject[] tabs = new GameObject[2];
    // Json�迭
    ArrayJson arrayJson = new ArrayJson();

    // Start is called before the first frame update
    void Start()
    {
        // �÷��̾� �Ҵ�
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

        // ������� ������Ʈ�� ����Ʈ�� �߰�
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
        // arrayJson�� Json���� ��ȯ
        //string jsonData = JsonUtility.ToJson(arrayJson);

        // DB�� ����
        DBManager.instance.SaveJsonMapCustom(arrayJson, "MapData");
    }

    public void LoadJson()
    {

    }
}
