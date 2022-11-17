using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FormationJsonInfo
{
    public string teamName;
    public Vector3 position;
}

[System.Serializable]
public class FormationArrayJson
{
    public List<FormationJsonInfo> formationInfos;
}

public class FormationManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
