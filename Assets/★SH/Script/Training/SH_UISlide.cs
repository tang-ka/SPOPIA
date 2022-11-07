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
        ��ĭ��������(isSlideOn);
    }

    public void OnClickOpen()
    {
        isSlideOn = !isSlideOn;
    }

    public void ��ĭ��������(bool isOn)
    {
        // �Ѱ�ʹ�.
        if (isOn)
        {
            myPos = Vector3.Lerp(myPos, onPos, Time.deltaTime * 10);
            // �ȳ��ϼ��� ���� ������ �𸣽ð٧c? �� �˷��帱�ſ��� �ֳ��ϸ� �׳� �˷��帮�� �Ⱦ��.
            // �౸���� �ѳ����̳׿� �౸�� �Ͻó�����? Ȥ�� �������̵嵵 ������ �� �ֳ���?
            // �װ� ���ٸ� �౸�� �ƴ϶�� �����մϴ�. ���� �־��ּ���.
            // ���ĸ� �پ�Ѵ� �� ����ñ� �ٶ�� ������ ���ϴ�. ��ĭ�� ������
            if ((myPos - onPos).magnitude < 0.05f)
                myPos = onPos;
        }
        // ���� �ʹ�.
        else
        {
            myPos = Vector3.Lerp(myPos, offPos, Time.deltaTime * 10);

            if ((myPos - offPos).magnitude < 0.05f)
                myPos = offPos;
        }

        this.GetComponent<RectTransform>().anchoredPosition = myPos;
    }
}
