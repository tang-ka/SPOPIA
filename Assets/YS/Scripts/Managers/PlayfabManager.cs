using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour
{
    //public InputField dbTest;
    public string dbID = "2B6812EA0A908140";

    // Start is called before the first frame update
    void Start()
    {
        PlayFabSettings.TitleId = "7FD3E";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData()
    {
        var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "A", "1" }, { "B", "2" } } };
        PlayFabClientAPI.UpdateUserData(request, (result) => print("�� ������ ���� �����ߴµ�?"), (error) => print("������ ���� �����ߴ٤�����"));
    }

    public void GetData()
    {
        var request = new GetUserDataRequest() { PlayFabId = dbID };
        PlayFabClientAPI.GetUserData(request, (result) => print(result.Data["A"].Value), (error) => print("�� ������ �ҷ����� �����߾�"));
    }
}
