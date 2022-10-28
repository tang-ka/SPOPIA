using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_PlayerRot : MonoBehaviour
{
    Transform cam;
    public Transform player;
    public Transform camPivot;
    public Transform[] camPos = new Transform[2];
    Transform targetCamPos;
    int index = 0;

    public float rotSpeed = 300;
    float rotX;
    float rotY;

    bool isClick = false;

    public enum ViewState
    {
        FIRST,
        THIRD
    }
    //ViewState view = ViewState.FIRST;

    void Start()
    {
        camPivot.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cam = Camera.main.transform;
    }

    void Update()
    {
        ClickKey();

        if (isClick)
            PlayerRot(ViewState.FIRST, false);
        else if (isClick == false)
            PlayerRot(ViewState.THIRD, false);
            
    }

    public void PlayerRot(ViewState s, bool isLookAround)
    {
        if (s == ViewState.FIRST)
        {
            index = 0;
        }
        else if (s == ViewState.THIRD)
        {
            index = 1;
        }
        targetCamPos = camPos[index];

        cam.position = Vector3.Lerp(cam.position, targetCamPos.position, 20 * Time.deltaTime);

        if (Vector3.Distance(cam.position, camPos[index].position) < 0.05f)
        {
            cam.position = camPos[index].position;
        }

        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");

        rotX += mx * rotSpeed * Time.deltaTime;
        rotY -= my * rotSpeed * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, -70.0f, 85.0f);

        if (isLookAround == false)
        {
            player.transform.localEulerAngles = new Vector3(0, rotX, 0);
            camPivot.transform.localEulerAngles = new Vector3(rotY, 0, 0);
        }
        else
        {
            camPivot.transform.localEulerAngles = new Vector3(rotY, rotX, 0);
        }
    }

    public void ClickKey()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            isClick = !isClick;
    }
}
