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
        // 빌보드기능(Text는 계속 앞만 보게끔) -> 방향벡터를 생각해야 함!
        transform.LookAt(transform.position - cam.transform.position);
    }
}
