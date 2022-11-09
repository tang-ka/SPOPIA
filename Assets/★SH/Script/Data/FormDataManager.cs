using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Formation
{
    public Vector3[] pos = new Vector3[11];
}

//[System.Serializable]
//public class FormationList : MonoBehaviour
//{
//    public List<Formation> formations = new List<Formation>();
//}

public class FormDataManager : MonoBehaviour
{
    public static FormDataManager instance;

    private Dictionary<string, Formation> formDic = new Dictionary<string, Formation>();

    //private FormationList formationList;

    Formation formation0 = new Formation();
    Formation formation1 = new Formation();
    Formation formation2 = new Formation();
    Formation formation3 = new Formation();

    private void Awake()
    {
        if (instance == null)
            instance = this;

        SetFormationList();
    }

    public void SetFormationList()
    {
        #region 4-3-3
        formation0.pos[0] = new Vector3(495, 0, 0);
        formation0.pos[1] = new Vector3(330, 225, 0);
        formation0.pos[2] = new Vector3(330, 75, 0);
        formation0.pos[3] = new Vector3(330, -75, 0);
        formation0.pos[4] = new Vector3(330, -225, 0);
        formation0.pos[5] = new Vector3(75, 150, 0);
        formation0.pos[6] = new Vector3(75, 0, 0);
        formation0.pos[7] = new Vector3(75, -150, 0);
        formation0.pos[8] = new Vector3(-200, 260, 0);
        formation0.pos[9] = new Vector3(-200, 0, 0);
        formation0.pos[10] = new Vector3(-200, -260, 0);
        #endregion

        #region 4-4-2
        formation1.pos[0] = new Vector3(495, 0, 0);
        formation1.pos[1] = new Vector3(330, 225, 0);
        formation1.pos[2] = new Vector3(330, 75, 0);
        formation1.pos[3] = new Vector3(330, -75, 0);
        formation1.pos[4] = new Vector3(330, -225, 0);
        formation1.pos[5] = new Vector3(75, 225, 0);
        formation1.pos[6] = new Vector3(75, 75, 0);
        formation1.pos[7] = new Vector3(75, -75, 0);
        formation1.pos[8] = new Vector3(75, -225, 0);
        formation1.pos[9] = new Vector3(-200, 75, 0);
        formation1.pos[10] = new Vector3(-200, -75, 0);
        #endregion

        #region 4-2-3-1
        formation2.pos[0] = new Vector3(495, 0, 0);
        formation2.pos[1] = new Vector3(330, 225, 0);
        formation2.pos[2] = new Vector3(330, 75, 0);
        formation2.pos[3] = new Vector3(330, -75, 0);
        formation2.pos[4] = new Vector3(330, -225, 0);
        formation2.pos[5] = new Vector3(190, 75, 0);
        formation2.pos[6] = new Vector3(190, -75, 0);
        formation2.pos[8] = new Vector3(0, 260, 0);
        formation2.pos[7] = new Vector3(30, 0, 0);
        formation2.pos[9] = new Vector3(0, -260, 0);
        formation2.pos[10] = new Vector3(-200, 0, 0);
        #endregion

        #region 3-5-2
        formation3.pos[0] = new Vector3(495, 0, 0);
        formation3.pos[1] = new Vector3(330, 150, 0);
        formation3.pos[2] = new Vector3(330, 0, 0);
        formation3.pos[3] = new Vector3(330, -150, 0);
        formation3.pos[4] = new Vector3(75, 260, 0);
        formation3.pos[5] = new Vector3(10, 75, 0);
        formation3.pos[6] = new Vector3(160, 0, 0);
        formation3.pos[7] = new Vector3(10, -75, 0);
        formation3.pos[8] = new Vector3(75, -260, 0);
        formation3.pos[9] = new Vector3(-200, 75, 0);
        formation3.pos[10] = new Vector3(-200, -75, 0);
        #endregion

        formDic.Add("4-3-3", formation0);
        formDic.Add("4-4-2", formation1);
        formDic.Add("4-2-3-1", formation2);
        formDic.Add("3-5-2", formation3);
    }

    public List<string> GetFormNames()
    {
        List<string> FormationNames = new List<string>();

        foreach (string key in formDic.Keys)
        {
            FormationNames.Add(key);
        }

        return FormationNames;
    }

    public Formation GetForm(string key)
    {
        Formation form = new Formation();
        form =  formDic[key];

        return form;
    }
}
