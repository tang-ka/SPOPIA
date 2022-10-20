using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YS_NewsText : MonoBehaviour
{
    public GameObject newsText1, newsText2;
    public Text text1, text2;
    public Image img;

    // Start is called before the first frame update
    void Start()
    {
        newsText1 = transform.Find("Text1").gameObject;
        text1 = newsText1.GetComponent<Text>();
        newsText2 = transform.Find("Text2").gameObject;
        text2 = newsText2.GetComponent<Text>();
        img = transform.Find("Image").gameObject.GetComponent<Image>();

        text1.text = "¹¹ ¾îÂ¼¶ó°í!";
        text2.text = "ÀúÈñ°¡ 100% ÀÌ±é´Ï´Ù.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
