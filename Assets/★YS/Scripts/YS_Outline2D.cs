using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class YS_Outline2D : MonoBehaviour
{
    AvatarManager btn;
    int temp_idx;

    private void Start()
    {
        btn = GameObject.Find("AvatarManager").GetComponent<AvatarManager>();
    }

    private void Update()
    {
        Outline();
    }

    void Outline()
    {
        temp_idx = int.Parse(gameObject.name);

        // �ٸ� ��ư�� Ŭ���ϸ� �ڽ��� outline�� �����Բ�
        if (btn.idx != temp_idx)
        {
            transform.Find("Outline").gameObject.SetActive(false);
        }
        // ������ ������
        else
        {
            transform.Find("Outline").gameObject.SetActive(true);
        }
    }
}