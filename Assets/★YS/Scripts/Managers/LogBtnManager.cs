using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogBtnManager : MonoBehaviour
{
    public GameObject signupPage, loginPage;

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
        loginPage.SetActive(true);
        signupPage.SetActive(false);
    }
}
