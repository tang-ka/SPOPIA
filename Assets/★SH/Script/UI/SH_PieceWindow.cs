using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEditor.PackageManager;

public class SH_PieceWindow : MonoBehaviourPun
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

    PhotonView parentPhotonView;
    SH_BlueTeam myParent;

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

        parentPhotonView = transform.parent.gameObject.GetPhotonView();
        myParent = GetComponentInParent<SH_BlueTeam>();
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

        parentPhotonView.RPC(nameof(myParent.RPC_SyncDistance), RpcTarget.Others, gameObject.name, first.name, second.name);

        first = null;
        second = null;
        print("코루틴 끝");
    }

    public void SyncDistance(string _firstName, string _secondName)
    {
        first = SH_TrainingUIManager.instance.blueParent.Find(_firstName).gameObject;
        second = SH_TrainingUIManager.instance.blueParent.Find(_secondName).gameObject;

        GameObject line = Instantiate(lineFactory, lineParent);
        line.GetComponent<SH_DistanceLine>().Init(first, second);

        distDeleteList += line.GetComponent<SH_DistanceLine>().DeleteSelf;
        second.GetComponent<SH_PieceWindow>().distDeleteList += line.GetComponent<SH_DistanceLine>().DeleteSelf;

        second = null;
        first = null;
    }

    public void OnClickBtnDistDelete()
    {
        if (distDeleteList == null) return;
        window.SetActive(false);
        distDeleteList();

        parentPhotonView.RPC(nameof(myParent.RPC_DistDelete), RpcTarget.Others, gameObject.name);
    }

    public void DistDelete()
    {
        distDeleteList();
    }

    GameObject start;
    Vector3 end;
    List<GameObject> arrowList = new List<GameObject>();
    bool rpcFlag;
    bool m0Flag = false;
    bool m1Flag = false;

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

        rpcFlag = true;

        while (true)
        {
            yield return null;

            print("코루틴 시작");
            end = Input.mousePosition;

            arrow.GetComponent<RectTransform>().anchoredPosition = start.transform.localPosition;
            arrow.transform.up = start.transform.position - end;

            dist = Vector3.Distance(start.transform.position, end);
            arrow.GetComponent<RectTransform>().sizeDelta = new Vector2(3, dist);
            float tempDist = dist;

            dist = Mathf.Round(dist);
            dist /= 10;
            distance.text = dist.ToString() + "m";  
            distance.transform.up = Vector3.up;

            if (SH_MouseControl.instance.isClickedM0)
            {
                arrow.GetComponent<SH_Arrow>().Init(start, end);
                arrowList.Add(arrow);

                m0Flag = true;
                m1Flag = false;

                parentPhotonView.RPC(nameof(myParent.RPC_SyncArrow), RpcTarget.Others,
                    gameObject.name, start.name, end, rpcFlag, tempDist, distance.text, m0Flag, m1Flag);

                break;
            }
            // 우클릭시 arrow 생성 취소
            else if (SH_MouseControl.instance.isClickedM1)
            {
                Destroy(arrow);

                m0Flag = false;
                m1Flag = true;

                parentPhotonView.RPC(nameof(myParent.RPC_SyncArrow), RpcTarget.Others,
                    gameObject.name, start.name, end, rpcFlag, tempDist, distance.text, m0Flag, m1Flag);

                break;
            }

            parentPhotonView.RPC(nameof(myParent.RPC_SyncArrow), RpcTarget.Others,
                gameObject.name, start.name, end, rpcFlag, tempDist, distance.text, m0Flag, m1Flag);

            rpcFlag = false;
        }

        m0Flag = false;
        m1Flag = false;
        arrow = null;
        distance = null;
    }

    GameObject arrow = null;
    Text distance = null;

    public void SyncArrow(string _startName, Vector3 _end, 
        bool _rpcFlag, float _dist, string _distance, bool _m0Flag, bool _m1Flag)
    {
        if (_rpcFlag)
        {
            arrow = Instantiate(arrowFactory, arrowParent);
            distance = arrow.transform.Find("txtDistance").GetComponent<Text>();
            start = SH_TrainingUIManager.instance.blueParent.Find(_startName).gameObject;
        }
        else
        {
            end = _end;
            arrow.GetComponent<RectTransform>().anchoredPosition = start.transform.localPosition;
            arrow.transform.up = start.transform.position - end;

            arrow.GetComponent<RectTransform>().sizeDelta = new Vector2(3, _dist);

            distance.text = _distance;
            distance.transform.up = Vector3.up;

            if (_m0Flag)
            {
                arrow.GetComponent<SH_Arrow>().Init(start, end);
                arrowList.Add(arrow);
            }
            // 우클릭시 arrow 생성 취소
            else if (_m1Flag)
            {
                Destroy(arrow);
            }
        }
    }

    public void OnClickBtnArrowDelete()
    {
        if (arrowList.Count <= 0) return;
        window.SetActive(false);
        arrowCount = 0;

        for (int i = 0; i < arrowList.Count; i++)
        {
            Destroy(arrowList[i].gameObject);
        }

        parentPhotonView.RPC(nameof(myParent.RPC_ArrowDelete), RpcTarget.Others, gameObject.name);
    }

    public void ArrowDelete()
    {
        for (int i = 0; i < arrowList.Count; i++)
        {
            Destroy(arrowList[i].gameObject);
        }
    }
}
