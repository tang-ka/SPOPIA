using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_MouseControl : MonoBehaviour
{
    bool isClickM0 = false;
    bool isClickM1 = false;

    void Start()
    {
        
    }

    void Update()
    {
        MouseInput();
    }

    void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
            isClickM0 = true;
        else if (Input.GetMouseButtonUp(0))
            isClickM0 = false;

        if (Input.GetMouseButtonDown(1))
            isClickM1 = true;
        else if (Input.GetMouseButtonUp(1))
            isClickM1 = false;
    }

    void PointerRay()
    {

    }
}
