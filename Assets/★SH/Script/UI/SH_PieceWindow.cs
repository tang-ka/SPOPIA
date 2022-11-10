using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SH_PieceWindow : MonoBehaviour
{
    public Button btnInputText;
    public Button btnDistance;
    public Button btnArrow;

    public InputField inputBackNumber;
    public InputField inputName;

    public Text backNumber;
    public Text name;

    public GameObject window;

    // 식당 예약인원 변경 전화하기
    void Start()
    {
        btnInputText.onClick.AddListener(OnClickBtnInputText);
        btnInputText.onClick.AddListener(OnClickBtnDistance);
        btnInputText.onClick.AddListener(OnClickBtnArrow);

        inputBackNumber.onSubmit.AddListener(OnSubmitInputBackNumber);
        inputName.onSubmit.AddListener(OnSubmitInputName);
    }

    private void OnClickBtnInputText()
    {
        inputBackNumber.gameObject.SetActive(true);
        inputName.gameObject.SetActive(true);
        window.SetActive(false);

        inputBackNumber.ActivateInputField();
    }

    private void OnSubmitInputBackNumber(string s)
    {
        backNumber.text = s;

        backNumber.gameObject.SetActive(false);
        inputName.ActivateInputField();
    }

    private void OnSubmitInputName(string s)
    {
        name.text = s;

        inputBackNumber.gameObject.SetActive(false);
        inputName.gameObject.SetActive(false);

        backNumber.gameObject.SetActive(true);
        name.gameObject.SetActive(true);
    }

    private void OnClickBtnDistance()
    {
        print("Distance Button");
    }

    private void OnClickBtnArrow()
    {
        print("Arrow Button");
    }

}
