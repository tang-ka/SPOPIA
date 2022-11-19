using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SH_Portal : MonoBehaviourPun
{
    public bool isReadyEnter = false;
    public GameObject triggerPlayer;

    public Button btnYes;
    public Button btnNo;

    public GameObject enterBG;

    Vector3 dir = Vector3.zero;
        
    void Start()
    {
        btnYes.onClick.AddListener(OnClickYes);
        btnNo.onClick.AddListener(OnClickNo);

        enterBG.SetActive(false);
    }

    private void OnClickYes()
    {
        print("����~~~!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
    }

    private void OnClickNo()
    {
        print("���������!!!!!!!!!!!!!!!!!!!!!!");
    }

    void Update()
    {
        if (enterBG.activeSelf && triggerPlayer != null)
        {
            dir = transform.position - triggerPlayer.transform.position;
            dir.y = 0;
            dir.Normalize();

            enterBG.transform.forward = dir;

            //float angle = Vector3.Angle(dir, enterBG.transform.right);
            //enterBG.transform.eulerAngles = new Vector3(90, 0, angle);
        }
    }

    // Ʈ���ſ� �ɸ��� ĵ������ �Ѱ�ʹ�.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                triggerPlayer = other.gameObject;
                enterBG.SetActive(true);
                isReadyEnter = true;
            }
        }
    }

    // Ʈ���ſ��� ������ ĵ������ ���� �ʹ�.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == triggerPlayer)
        {
            enterBG.SetActive(false);
            isReadyEnter = false;
        }
    }
}
