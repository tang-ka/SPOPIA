using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YS_MapCustom : MonoBehaviour
{
    // ���콺�� ������Ʈ ������ �Ÿ�
    Vector3 dis;
    // ���콺�� ���ȴ���
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
                isClick = true; // ������ ���� ��, �ٽ� ��� �ȵǰԲ� true�� �ٲ��ֱ�
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
