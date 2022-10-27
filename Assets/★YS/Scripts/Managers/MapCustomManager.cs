using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCustomManager : MonoBehaviour
{
    // Ŀ���� ���� ������Ʈ
    public GameObject obj;
    // �÷��̾� �Ҵ�
    public GameObject player;
    // ��
    public GameObject[] tabs = new GameObject[2];

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
}
