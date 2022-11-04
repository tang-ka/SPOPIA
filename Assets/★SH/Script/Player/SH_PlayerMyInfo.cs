using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SH_PlayerMyInfo : MonoBehaviour
{
    Vector3 myPosition;
    Vector3 onPosition;
    Vector3 offPosition;

    bool isOn = false;

    void Start()
    {
        onPosition = new Vector3(-400, 0, 0);
        offPosition = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            isOn = !isOn;

        MyInfoMove(isOn);
    }

    void MyInfoMove(bool isOn)
    {
        // 켜고싶다.
        if (isOn)
        {
            myPosition = Vector3.Lerp(myPosition, onPosition, Time.deltaTime * 10);

            if ((myPosition - onPosition).magnitude < 0.05f)
                myPosition = onPosition;
        }
        // 끄고 싶다.
        else
        {
            myPosition = Vector3.Lerp(myPosition, offPosition, Time.deltaTime * 10);

            if ((myPosition - offPosition).magnitude < 0.05f)
                myPosition = offPosition;
        }

        this.GetComponent<RectTransform>().anchoredPosition = myPosition;
    }
}
