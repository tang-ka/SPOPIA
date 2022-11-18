using Photon.Pun;
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
        enterBG.SetActive(false);
    }

    void Update()
    {
        if (enterBG.activeSelf)
        {
            dir = transform.position - triggerPlayer.transform.position;
            dir.y = 0;
            dir.Normalize();

            float angle = Vector3.Angle(dir, enterBG.transform.right);
            enterBG.transform.eulerAngles = new Vector3(90, 0, angle);
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
