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
    // 싱글톤
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

    // 커스텀 포메이션 저장
    public void SaveFormation()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            FormationJsonInfo info = new FormationJsonInfo();

            //info.teamName = DBManager.instance.myData.teamName;
            info.position = pieces[i].GetComponent<RectTransform>().anchoredPosition;

            DBManager.instance.formationDatas.formationInfos.Add(info);
        }

        // DB에 저장
        DBManager.instance.SaveJsonFormation(DBManager.instance.formationDatas, DBManager.instance.myData.teamName);
    }

    // 커스텀 포메이션 불러오기
    public void LoadFormation()
    {
        // Json형태를 사용할 수 있는 데이터 형태로 변환
        DBManager.instance.GetData(DBManager.instance.testDBid2, DBManager.instance.myData.teamName);

        for (int i = 0; i < pieces.Length; i++)
        {
            FormationJsonInfo info = DBManager.instance.formationDatas.formationInfos[i];

            LoadPiece(info, i);
        }
    }

    public void LoadPiece(FormationJsonInfo info, int idx)
    {
        // 오브젝트 생성
        pieces[idx].GetComponent<RectTransform>().anchoredPosition = info.position;
    }
}
