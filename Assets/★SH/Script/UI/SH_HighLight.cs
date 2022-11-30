using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_HighLight : MonoBehaviour
{
    public Button btnCoach;
    public Button btnPlayer;

    public Image coachHighLight;
    public Image playerHighLight;

    void Start()
    {
        coachHighLight.gameObject.SetActive(false);
        playerHighLight.gameObject.SetActive(false);

        btnCoach.onClick.AddListener(CoachHighLight);
        btnPlayer.onClick.AddListener(PlayerHighLight);
    }

    private void PlayerHighLight()
    {
        coachHighLight.gameObject.SetActive(false);
        playerHighLight.gameObject.SetActive(true);
    }

    private void CoachHighLight()
    {
        coachHighLight.gameObject.SetActive(true);
        playerHighLight.gameObject.SetActive(false);
    }
}
