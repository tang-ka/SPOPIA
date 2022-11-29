using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YS_News : MonoBehaviour
{
    public GameObject newsContent;
    public RectTransform rect;
    //public Image tumbnail;

    float myloc;

    // Start is called before the first frame update
    void Start()
    {
        newsContent = transform.Find("NewsContent").gameObject;
        // 뉴스 썸네일 설정
        //tumbnail = transform.Find("NewsTumbnail").gameObject.GetComponent<Image>();

        // 뉴스내용 생성 위치 설정
        rect = newsContent.GetComponent<RectTransform>();
        myloc = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(myloc != GetComponent<RectTransform>().anchoredPosition.y)
        {
            float y = GetComponent<RectTransform>().anchoredPosition.y + 150;
            Vector3 newspos = new Vector3(rect.anchoredPosition.x, rect.anchoredPosition.y - y, 0);
            rect.anchoredPosition = newspos;

            myloc = GetComponent<RectTransform>().anchoredPosition.y;
        }
    }

    public void OpenNews()
    {
        newsContent.gameObject.SetActive(true);
    }
}
