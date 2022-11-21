using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SH_PieceWindow : MonoBehaviour
{
    public Button btnInputText;
    public Button btnDistance;
    public Button btnDistDelete;
    public Button btnArrow;
    public Button btnArrowDelete;

    public InputField inputBackNumber;
    public InputField inputName;

    public Text backNumber;
    public Text playerName;

    public GameObject window;

    public Transform lineParent;
    public GameObject lineFactory;

    public Transform arrowParent;
    public GameObject arrowFactory;

    int arrowCount = 0;

    void Start()
    {
        lineParent = GameObject.Find("GroundBG").transform.Find("LineParent");
        arrowParent = GameObject.Find("GroundBG").transform.Find("ArrowParent");

        btnInputText.onClick.AddListener(OnClickBtnInputText);
        btnDistance.onClick.AddListener(OnClickBtnDistance);
        btnDistDelete.onClick.AddListener(OnClickBtnDistDelete);
        btnArrow.onClick.AddListener(OnClickBtnArrow);
        btnArrowDelete.onClick.AddListener(OnClickBtnArrowDelete);

        inputBackNumber.onSubmit.AddListener(OnSubmitInputBackNumber);
        inputName.onSubmit.AddListener(OnSubmitInputName);

        window.SetActive(false);
    }

    void Update()
    {
        if (arrowCount > 0)
            btnArrowDelete.gameObject.SetActive(true);
        else
            btnArrowDelete.gameObject.SetActive(false);

        if (distDeleteList == null)
            btnDistDelete.gameObject.SetActive(false);
        else
            btnDistDelete.gameObject.SetActive(true);

    }

    private void OnClickBtnInputText()
    {
        inputBackNumber.gameObject.SetActive(true);
        inputName.gameObject.SetActive(true);
        window.SetActive(false);

        inputBackNumber.ActivateInputField();
    }

    private void OnSubmitInputBackNumber(string s)
    {
        backNumber.text = s;

        backNumber.gameObject.SetActive(false);
        inputName.ActivateInputField();
    }

    private void OnSubmitInputName(string s)
    {
        playerName.text = s;

        inputBackNumber.gameObject.SetActive(false);
        inputName.gameObject.SetActive(false);

        backNumber.gameObject.SetActive(true);
        playerName.gameObject.SetActive(true);
    }

    GameObject first, second;

    [SerializeField]
    public Action distDeleteList;
    //List<GameObject> distList = new List<GameObject>();

    public void OnClickBtnDistance()
    {
        window.SetActive(false);

        SH_MouseControl.instance.leftClickPiece = null;

        print("거리계산");
        first = this.gameObject;
        StartCoroutine(SelectSecond());
    }

    IEnumerator SelectSecond()
    {
        print("코루틴 시작");
        
        yield return new WaitUntil(() => SH_MouseControl.instance.leftClickPiece != null);

        second = SH_MouseControl.instance.leftClickPiece;
        GameObject line = Instantiate(lineFactory, lineParent);
        line.GetComponent<SH_DistanceLine>().Init(first, second);

        distDeleteList += line.GetComponent<SH_DistanceLine>().DeleteSelf;
        second.GetComponent<SH_PieceWindow>().distDeleteList += line.GetComponent<SH_DistanceLine>().DeleteSelf;

        second = null;
        first = null;
        print("코루틴 끝");
    }

    public void OnClickBtnDistDelete()
    {
        if (distDeleteList == null) return;
        window.SetActive(false);
        distDeleteList();
    }

    GameObject start;
    Vector3 end;
    List<GameObject> arrowList = new List<GameObject>();

    private void OnClickBtnArrow()
    {
        arrowCount++;

        window.SetActive(false);
        print("Arrow Button");
        start = this.gameObject;

        StartCoroutine(SelectEnd());
    }

    IEnumerator SelectEnd()
    {
        float dist;
        GameObject arrow = Instantiate(arrowFactory, arrowParent);
        Text distance = arrow.transform.Find("txtDistance").GetComponent<Text>();

        while (true)
        {
            yield return null;

            print("코루틴 시작");
            end = Input.mousePosition;

            arrow.GetComponent<RectTransform>().anchoredPosition = start.transform.localPosition;
            arrow.transform.up = start.transform.position - end;

            dist = Vector3.Distance(start.transform.position, end);
            arrow.GetComponent<RectTransform>().sizeDelta = new Vector2(3, dist);

            dist = Mathf.Round(dist);
            dist /= 10;
            distance.text = dist.ToString() + "m";  
            distance.transform.up = Vector3.up;

            if (SH_MouseControl.instance.isClickedM0)
            {
                arrow.GetComponent<SH_Arrow>().Init(start, end);
                arrowList.Add(arrow);
                break;
            }
            // 우클릭시 arrow 생성 취소
            else if (SH_MouseControl.instance.isClickedM1)
            {
                Destroy(arrow);
                break;
            }
        }
    }

    public void OnClickBtnArrowDelete()
    {
        window.SetActive(false);
        arrowCount = 0;

        for (int i = 0; i < arrowList.Count; i++)
        {
            Destroy(arrowList[i].gameObject);
        }
    }
}
