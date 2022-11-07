using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_UISlide : MonoBehaviour
{
    Vector3 myPos;
    public Vector3 onPos;
    Vector3 offPos;

    public Button btn;

    bool isSlideOn = false;

    void Start()
    {
        offPos = new Vector3(0, 0, 0);
        btn.onClick.AddListener(OnClickOpen);
    }

    void Update()
    {
        와칸다포에버(isSlideOn);
    }

    public void OnClickOpen()
    {
        isSlideOn = !isSlideOn;
    }

    public void 와칸다포에버(bool isOn)
    {
        // 켜고싶다.
        if (isOn)
        {
            myPos = Vector3.Lerp(myPos, onPos, Time.deltaTime * 10);
            // 안녕하세요 제가 누군지 모르시겟쬬? 안 알려드릴거에요 왜냐하면 그냥 알려드리기 싫어요.
            // 축구장을 켜놓으셨네요 축구를 하시나보죠? 혹시 오프사이드도 감지할 수 있나요?
            // 그게 없다면 축구가 아니라고 생각합니다. 당장 넣어주세요.
            // 피파를 뛰어넘는 걸 만드시길 바라며 무운을 빕니다. 와칸다 포에버
            if ((myPos - onPos).magnitude < 0.05f)
                myPos = onPos;
        }
        // 끄고 싶다.
        else
        {
            myPos = Vector3.Lerp(myPos, offPos, Time.deltaTime * 10);

            if ((myPos - offPos).magnitude < 0.05f)
                myPos = offPos;
        }

        this.GetComponent<RectTransform>().anchoredPosition = myPos;
    }
}
