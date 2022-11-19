using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletinBtnManager : MonoBehaviour
{
    public GameObject article, newsCanvas, createPage;
    public YS_News ys_news;
    public Transform contentTr;
    public Image micImg;
    public Text q1, q2, q3;

    // Start is called before the first frame update
    void Start()
    {
        ys_news = article.GetComponent<YS_News>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
    }

    public void CreatePage()
    {
        if(createPage.activeSelf == false)
        {
            createPage.SetActive(true);
        }
    }

    public void CreateArticle()
    {
        // 뉴스 생성
        GameObject news = Instantiate(article);
        news.transform.SetParent(contentTr, false);
    }

    public void Cancel()
    {
        if (createPage.activeSelf == true)
        {
            createPage.SetActive(false);
        }
    }

    public void MicONOFF()
    {
        Color color = new Color();
        color = micImg.color;
        if(color.a <= 0.3f)
        {
            color.a = 1;
            micImg.color = color;
        }
        else if(color.a == 1)
        {
            color.a = 0.3f;
            micImg.color = color;

            if(q1.enabled == true)
            {
                q1.enabled = false;
                q2.enabled = true;
            }
            else if(q2.enabled == true)
            {
                q2.enabled = false;
                q3.enabled = true;

                // 기사에 들어갈 사진찍기
                YS_Capture.instance.Shot();
            }
        }
    }

    public void Click()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // 팀 생성
            if (hit.transform.gameObject.name == "News")
            {
                if(newsCanvas.activeSelf == false)
                {
                    newsCanvas.SetActive(true);
                }
            }
        }
    }
}
