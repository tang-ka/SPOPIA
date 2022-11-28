using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_MessageTrigger : MonoBehaviour
{
    public GameObject triggerPlayer;
    public GameObject sooJungChe;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerPlayer == null) return;

        triggerPlayer.GetComponent<SH_PlayerCrossHair>().dataInputCenterMsg.SetActive(sooJungChe.activeSelf);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<PhotonView>().IsMine)
            {
                triggerPlayer = other.gameObject;
                sooJungChe.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == triggerPlayer)
        {
            sooJungChe.SetActive(false);
        }
    }
}
