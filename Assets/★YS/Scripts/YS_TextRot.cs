using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YS_TextRot : MonoBehaviour
{
    GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        // ��������(Text�� ��� �ո� ���Բ�) -> ���⺤�͸� �����ؾ� ��!
        transform.LookAt(transform.position - cam.transform.position);
    }
}
