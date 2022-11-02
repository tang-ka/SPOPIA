using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YS_MapCustomCam : MonoBehaviour
{
    float speed = 1;
    SH_PlayerRot sh_pr;

    // Start is called before the first frame update
    void Start()
    {
        sh_pr = GameObject.Find("Player").GetComponent<SH_PlayerRot>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            Basic();
            sh_pr.rotSpeed = 300f;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            Good();
            sh_pr.rotSpeed = 0;
        }
    }

    void Basic()
    {
        speed = 1f;
        float rotSpeed = 0.5f;

        Vector3 goodPos = new Vector3(0, 0.8000002f, 0);
        transform.localPosition = goodPos;
        Vector3 rot = new Vector3(0, 0, 0f);
        transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, rot, rotSpeed * Time.deltaTime);
    }

    void Good()
    {
        speed = 1f;
        float rotSpeed = 0.5f;

        Vector3 goodPos = new Vector3(0, 400f, 0);
        transform.localPosition = goodPos;
        Vector3 rot = new Vector3(90, 0, 0f);
        transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, rot, rotSpeed * Time.deltaTime);
    }
}
