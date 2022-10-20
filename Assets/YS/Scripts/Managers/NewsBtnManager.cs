using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsBtnManager : MonoBehaviour
{
    public GameObject article;
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
        
    }

    public void CreateArticle()
    {
        // 뉴스 생성
        GameObject news = Instantiate(article);
        news.transform.SetParent(contentTr, false);
    }
}
