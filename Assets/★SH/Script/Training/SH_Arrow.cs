using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class SH_Arrow : MonoBehaviour
{
    GameObject start;
    Vector3 end;

    public Text distance;

    public void Init(GameObject start, Vector3 end)
    {
        this.start = start;
        this.end = end;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (start == null) return;
        GetComponent<RectTransform>().anchoredPosition = start.transform.localPosition;
    }

    void DrawArrow()
    {
        GetComponent<RectTransform>().anchoredPosition = start.transform.localPosition;
        transform.up = start.transform.position - end;

        float dist = Vector3.Distance(start.transform.position, end);
        GetComponent<RectTransform>().sizeDelta = new Vector2(3, dist);

        dist = Mathf.Round(dist);
        dist /= 10;
        //distance.text = dist.ToString() + "m";
        //distance.transform.up = Vector3.up;
    }
}
