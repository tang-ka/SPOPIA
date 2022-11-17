using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_CheatKey : MonoBehaviour
{
    public InputField id;
    public InputField pw;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            id.text = "gur0907@hanyang.ac.kr";
            pw.text = "gur135";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            id.text = "a@gmail.com";
            pw.text = "123456";
        }
    }
}
