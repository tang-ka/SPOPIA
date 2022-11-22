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

        // 다른 버튼을 클릭하면 자신의 outline이 꺼지게끔
        if (btn.idx != temp_idx)
        {
            transform.Find("Outline").gameObject.SetActive(false);
        }
        // 같으면 켜지게
        else
        {
            transform.Find("Outline").gameObject.SetActive(true);
        }
    }
}