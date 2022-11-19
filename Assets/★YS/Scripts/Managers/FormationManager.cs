using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FormationJsonInfo
{
    //public string teamName;
    public Vector3 position;
}

[System.Serializable]
public class FormationArrayJson
{
    public List<FormationJsonInfo> formationInfos;
}

public class FormationManager : MonoBehaviour
{
    // �̱���
    public static FormationManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject[] pieces = new GameObject[11];
    //public FormationArrayJson formationArray = new FormationArrayJson();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Ŀ���� �����̼� ����
    public void SaveFormation()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            FormationJsonInfo info = new FormationJsonInfo();

            //info.teamName = DBManager.instance.myData.teamName;
            info.position = pieces[i].GetComponent<RectTransform>().anchoredPosition;

            DBManager.instance.formationDatas.formationInfos.Add(info);
        }

        // DB�� ����
        DBManager.instance.SaveJsonFormation(DBManager.instance.formationDatas, DBManager.instance.myData.teamName);
    }

    // Ŀ���� �����̼� �ҷ�����
    public void LoadFormation()
    {
        // Json���¸� ����� �� �ִ� ������ ���·� ��ȯ
        DBManager.instance.GetData(DBManager.instance.testDBid2, DBManager.instance.myData.teamName);

        for (int i = 0; i < pieces.Length; i++)
        {
            FormationJsonInfo info = DBManager.instance.formationDatas.formationInfos[i];

            LoadPiece(info, i);
        }
    }

    public void LoadPiece(FormationJsonInfo info, int idx)
    {
        // ������Ʈ ����
        pieces[idx].GetComponent<RectTransform>().anchoredPosition = info.position;
    }
}
