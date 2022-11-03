using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YS_MapCustomCam : MonoBehaviour
{
    SH_PlayerRot sh_pr;
    SH_PlayerMove sh_pm;
    SH_PlayerCrossHair sh_pch;
    GameObject player;
    public int camNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        sh_pr = player.GetComponent<SH_PlayerRot>();
        sh_pm = player.GetComponent<SH_PlayerMove>();
        sh_pch = player.GetComponent<SH_PlayerCrossHair>();
    }

    // Update is called once per frame
    void Update()
    {
        KeyNum();

        if (camNum == 0)
        {
            sh_pr.enabled = true;
            sh_pm.enabled = true;
            sh_pch.enabled = true;
            sh_pr.rotSpeed = 300f;
            Basic();
        }
        else if (camNum == 1)
        {
            sh_pr.enabled = false;
            sh_pm.enabled = false;
            sh_pch.enabled = false;
            sh_pr.rotSpeed = 0;
            player.transform.eulerAngles = new Vector3(0, 0, 0);
            TopView();
        }
    }

    void Basic()
    {
        float rotSpeed = 1f;

        Vector3 goodPos = new Vector3(0, 1.7f, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, goodPos, rotSpeed * Time.deltaTime);
        /*Vector3 rot = new Vector3(0, 0, 0f);
        transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, rot, rotSpeed * Time.deltaTime);*/
    }

    void TopView()
    {
        float rotSpeed = 1f;

        Vector3 goodPos = new Vector3(0, 400f, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, goodPos, rotSpeed * Time.deltaTime);
        Vector3 rot = new Vector3(90, 0, 0f);
        transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, rot, rotSpeed * Time.deltaTime);
    }

    void KeyNum()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            camNum = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            camNum = 1;
        }
    }
}
