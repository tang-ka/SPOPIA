using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SH_MouseControl : MonoBehaviourPun
{
    public static SH_MouseControl instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public bool isClickingM0 = false;
    public bool isClickingM1 = false;
    public bool isClickedM0 = false;
    public bool isClickedM1 = false;

    bool pieceFlag = false;
    bool isWindowOpen = false;

    float clickTimeM0 = 0;
    float clickTimeM1 = 0;
    float clickedTime = 0.1f;

    Vector3 sizeUp = Vector3.one * 1.2f;

    public Canvas m_canvas;
    GraphicRaycaster m_gr;
    PointerEventData m_ped;
    List<RaycastResult> results;
    //RaycastResult piece = new RaycastResult();

    public GameObject leftClickPiece;
    Transform slcPiece;
    Transform preSlcPiece;
    GameObject preWindow;

    public GameObject outlineFactory;

    void Start()
    {
        clickTimeM0 = clickedTime;
        clickTimeM1 = clickedTime;
        
        //m_canvas = 자신이 사용하는 캔버스 넣기.
        m_gr = m_canvas.GetComponent<GraphicRaycaster>();
        m_ped = new PointerEventData(null);

    }

    void Update()
    {
        MouseInput();
        print(isClickedM0 + ", " + isClickingM0);

        CanvaseRay();
        MovePiece();
    }

    void MouseInput()
    {
        if (isClickingM0)
            clickTimeM0 -= Time.deltaTime;

        if (isClickingM1)
            clickTimeM1 -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            isClickingM0 = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isClickingM0 = false;
            isClickedM0 = false;
            clickTimeM0 = clickedTime;
        }

        if (Input.GetMouseButtonDown(1))
        {
            isClickingM1 = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isClickingM1 = false;
            isClickedM1 = false;
            clickTimeM1 = clickedTime;
        }
        
        if (isClickingM0 && clickTimeM0 > 0)
        {
            isClickedM0 = true;
        }
        else
            isClickedM0 = false;

        if (isClickingM1 && clickTimeM1 > 0)
        {
            isClickedM1 = true;
        }
        else
            isClickedM1 = false;

    }

    void CanvaseRay()
    {
        m_ped.position = Input.mousePosition;
        results = new List<RaycastResult>();
        m_gr.Raycast(m_ped, results);

        if (results.Count > 0)
        {
            if (results[0].gameObject.CompareTag("BluePiece"))
            {
                preSlcPiece = slcPiece;
                slcPiece = results[0].gameObject.transform.parent;
                slcPiece.localScale = sizeUp;
                pieceFlag = true;

                // 우클릭시 윈도우를 열고 싶다.
                if (isClickedM1)
                {
                    if (preWindow != null)
                    {
                        preWindow.SetActive(false);
                        //preWindow.transform.parent.localScale = Vector3.one;
                    }

                    preWindow = slcPiece.Find("Window").gameObject;
                    preWindow.SetActive(true);
                    isWindowOpen = true;
                }

                // 좌클릭시
                if (isClickedM0)
                {
                    leftClickPiece = results[0].gameObject.transform.parent.gameObject;
                    leftClickPiece.transform.SetAsLastSibling();
                }

            }
            else if (results[0].gameObject.CompareTag("Window"))
            {

            }
            else
            {
                if (pieceFlag && !isWindowOpen)
                {
                    slcPiece.localScale = Vector3.one;

                    // 피스 밖에서 마우스 입력이 없으면 셀렉피스 초기화
                    if (!isClickedM0 && !isClickingM0)
                        slcPiece = null;

                    pieceFlag = false;

                }
                // 피스가 아닌 곳을 클릭하면 윈도우를 닫고 싶다.
                if (isClickedM0 || isClickedM1)
                {
                    if (slcPiece == null || preWindow == null) return;
                    slcPiece.localScale = Vector3.one;
                    preWindow.SetActive(false);
                    isWindowOpen = false;
                }
            }
        }
    }

    Vector3 targetPosition = Vector3.zero;
    Vector3 curPosition = Vector3.zero;

    public void MovePiece()
    {
        if (SH_TrainingUIManager.instance.isCoach == false) return;

        if (slcPiece == null) return;

        string name = slcPiece.name;

        if (results.Count > 0)
        {
            targetPosition.x = results[0].screenPosition.x - 960;
            targetPosition.y = results[0].screenPosition.y - 537;
            targetPosition.z = 0;

            if (!isClickedM0 && isClickingM0)
            {
                curPosition = slcPiece.localPosition;
                curPosition = Vector3.Lerp(curPosition, targetPosition, Time.deltaTime * 10000);
                slcPiece.localPosition = curPosition;

                photonView.RPC(nameof(RPC_MovePiece), RpcTarget.Others, name, targetPosition);
            }
        }
    }
    [PunRPC]
    void RPC_MovePiece(string _name, Vector3 _targetPosition)
    {
        slcPiece = SH_TrainingUIManager.instance.blueParent.Find(_name);

        curPosition = slcPiece.localPosition;
        curPosition = Vector3.Lerp(curPosition, _targetPosition, Time.deltaTime * 10000);
        slcPiece.localPosition = curPosition;
    }

    RaycastResult GetReults(int idx)
    {
        return results[idx];
    }

    void InputText()
    {

    }
}
