using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YS_MapCustom : MonoBehaviour
{
    // 마우스와 오브젝트 사이의 거리
    Vector3 dis;
    // 마우스가 눌렸는지
    bool isClick = false;
    // rot 값 저장
    float tempX, tempY;
    // ray가 맞았는지 판단
    bool isCheck = false;
    // rot 짐벌락 해결
    Vector3 rot;
    // 복제 키 작동 중
    bool isCopy = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            isCopy = true;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Check();

            if(isCopy == true)
            {
                string name = transform.gameObject.name.Substring(0, transform.gameObject.name.IndexOf("("));
                GameObject go = Instantiate(Resources.Load<GameObject>("YS/" + name));
                //Instantiate(transform.gameObject);

                // 복제품을 원본과 똑같은 위치에
                go.transform.position = transform.position;
                go.transform.eulerAngles = transform.eulerAngles;
                go.transform.localScale = transform.localScale;

                DBManager.instance.createdObj.Add(transform.gameObject);
                DBManager.instance.createdPrefab.Add(transform.gameObject);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Check();

            if(isCheck == true)
            {
                tempX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                tempY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            }
        }

        if(Input.GetMouseButton(0))
        {
            if(isCheck == true)
            {
                ObjMove();
            }
        }
        else if(Input.GetMouseButton(1))
        {
            if(isCheck == true)
            {
                ObjRotate();
            }
        }
        
        if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            ClickUp();
        }
    }

    void ClickUp()
    {
        isClick = false;
        isCheck = false;
        isCopy = false;
    }

    void ObjMove()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 10000, Color.cyan);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            float y = transform.position.y;

            if (Input.mouseScrollDelta.y != 0)
            {
                y += Input.mouseScrollDelta.y * 0.1f;
            }

            if (isClick == false)
            {
                dis = transform.position - hit.point;
                isClick = true; // 누르고 있을 땐, 다시 계산 안되게끔 true로 바꿔주기
            }

            transform.position = new Vector3(hit.point.x + dis.x, y, hit.point.z + dis.z);
        }
    }

    void ObjRotate()
    {
        Vector3 mousepoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float dis_x = mousepoint.x - tempX;
        float dis_y = mousepoint.y - tempY;

        if (dis_x > 2)
        {
            rot.y += dis_x * 0.1f;
        }
        else if (dis_x < -2)
        {
            rot.y += dis_x * 0.1f;
        }
        else if (dis_y > 2)
        {
            rot.x -= dis_y * 0.1f;
        }
        else if (dis_y < -2)
        {
            rot.x -= dis_y * 0.1f;
        }

        transform.eulerAngles = rot;
    }

    void Check()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject == transform.gameObject)
            {
                isCheck = true;
            }
        }
    }
}
