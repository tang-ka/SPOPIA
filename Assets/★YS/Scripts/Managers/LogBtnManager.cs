using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogBtnManager : MonoBehaviour
{
    public GameObject signupPage, loginPage, reallyPage, registerGoodPage;

    // Start is called before the first frame update
    void Start()
    {
        signupPage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SignUpClick()
    {
        loginPage.SetActive(false);
        signupPage.SetActive(true);
    }

    public void CancelClick()
    {
        reallyPage.SetActive(true);
    }

    public void ReallyYes()
    {
        reallyPage.SetActive(false);
        loginPage.SetActive(true);
        signupPage.SetActive(false);
    }

    public void ReallyNo()
    {
        reallyPage.SetActive(false);
    }

    public void RegisterGood()
    {
        registerGoodPage.SetActive(false);
        loginPage.SetActive(true);
        signupPage.SetActive(false);
    }
}
