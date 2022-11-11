using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletinBtnManager : MonoBehaviour
{
    public GameObject article, newsCanvas;
    public YS_News ys_news;
    public Transform contentTr;

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

    public void CreateArticle()
    {
        // ���� ����
        GameObject news = Instantiate(article);
        news.transform.SetParent(contentTr, false);
    }

    public void Click()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // �� ����
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
