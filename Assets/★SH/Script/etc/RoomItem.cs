using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    // ���� (���̸� (0 / 0))
    public Text roomInfo;

    // ����
    public Text roomDesc;

    // map id
    int map_id;

    // Ŭ���� �Ǿ��� �� ȣ��Ǵ� �Լ��� �������ִ� ����
    public Action<string, int> onClickAction;

    public void SetInfo(string roomName, int curPlayer, byte maxPlayer)
    {
        // ���ӿ�����Ʈ�� �̸��� roomName����!
        name = roomName;

        // ���̸� (0 / 0)
        roomInfo.text = roomName + "\t(" + curPlayer + " / " + maxPlayer + ")";
    }

    public void SetInfo(RoomInfo info)
    {
        SetInfo((string)info.CustomProperties["room_name"], info.PlayerCount, info.MaxPlayers);

        // desc ����
        roomDesc.text = (string)info.CustomProperties["desc"];

        // map id ����
        map_id = (int)info.CustomProperties["map_id"];
    }

    public void OnClick()
    {
        //// 1. InputRoomName ���ӿ�����Ʈ ã��
        //GameObject go = GameObject.Find("InputRoomName");
        //// 2. InputFiled ������Ʈ ã��
        //InputField inputField = go.GetComponent<InputField>();
        //// 3. text�� roomName����.
        //inputField.text = name;

        // ���࿡ onClickAction�� null�� �ƴ϶��
        if (onClickAction != null)
        {
            // onClickAction ����
            onClickAction(name, map_id);
        }
    }
}
