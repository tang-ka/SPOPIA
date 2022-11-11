using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SH_DistanceLine : MonoBehaviour
{
    GameObject first;
    GameObject second;

    public Text distance;

    public void Init(GameObject first, GameObject second)
    {
        this.first = first;
        this.second = second;
    }

    void Start()
    {

    }

    void Update()
    {
        DrawLine();
    }

    void DrawLine()
    {
        GetComponent<RectTransform>().anchoredPosition = first.transform.localPosition;
        transform.up = first.transform.position - second.transform.position;

        float dist = Vector3.Distance(first.transform.position, second.transform.position);
        GetComponent<RectTransform>().sizeDelta = new Vector2(3, dist);

        dist = Mathf.Round(dist);
        dist /= 10;
        distance.text = dist.ToString() + "m";
        distance.transform.up = Vector3.up;
    }
}
