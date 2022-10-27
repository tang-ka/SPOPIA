using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    // 내용 (방이름 (0 / 0))
    public Text roomInfo;

    // 설명
    public Text roomDesc;

    // map id
    int map_id;

    // 클릭이 되었을 때 호출되는 함수를 가지고있는 변수
    public Action<string, int> onClickAction;

    public void SetInfo(string roomName, int curPlayer, byte maxPlayer)
    {
        // 게임오브젝트의 이름을 roomName으로!
        name = roomName;

        // 방이름 (0 / 0)
        roomInfo.text = roomName + "\t(" + curPlayer + " / " + maxPlayer + ")";
    }

    public void SetInfo(RoomInfo info)
    {
        SetInfo((string)info.CustomProperties["room_name"], info.PlayerCount, info.MaxPlayers);

        // desc 설정
        roomDesc.text = (string)info.CustomProperties["desc"];

        // map id 설정
        map_id = (int)info.CustomProperties["map_id"];
    }

    public void OnClick()
    {
        //// 1. InputRoomName 게임오브젝트 찾자
        //GameObject go = GameObject.Find("InputRoomName");
        //// 2. InputFiled 컴포넌트 찾자
        //InputField inputField = go.GetComponent<InputField>();
        //// 3. text에 roomName넣자.
        //inputField.text = name;

        // 만약에 onClickAction가 null이 아니라면
        if (onClickAction != null)
        {
            // onClickAction 실행
            onClickAction(name, map_id);
        }
    }
}
