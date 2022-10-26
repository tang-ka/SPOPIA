using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YS_MapCustom : MonoBehaviour
{
    // 마우스와 오브젝트 사이의 거리
    Vector3 dis;
    // 마우스가 눌렸는지
    bool isClick = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        if(Input.GetMouseButton(0))
        {
            ObjMove();
        }
        else if(Input.GetMouseButton(1))
        {
            
        }
        
        if(Input.GetMouseButtonUp(0))
        {
            ClickUp();
        }
    }

    void ClickUp()
    {
        isClick = false;
    }

    void ObjMove()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 10000, Color.cyan);

        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            float y = transform.position.y;

            if (Input.mouseScrollDelta.y != 0)
            {
                y += Input.mouseScrollDelta.y * 0.1f;
            }

            if(isClick == false)
            {
                dis = transform.position - hit.point;
                isClick = true; // 누르고 있을 땐, 다시 계산 안되게끔 true로 바꿔주기
            }

            transform.position = new Vector3(hit.point.x + dis.x, y, hit.point.z + dis.z);
        }
    }

    void ObjRotate()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 10000, Color.cyan);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            transform.eulerAngles = new Vector3();
        }
    }
}
