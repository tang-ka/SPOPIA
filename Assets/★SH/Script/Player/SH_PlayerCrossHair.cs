using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_PlayerCrossHair : MonoBehaviour
{
    public Transform cam;

    public GameObject DataInputTable;

    void Start()
    {
        
    }

    MeshRenderer sphere;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20))
        {
            if (hit.transform.gameObject.name == "MatchDataSphere")
            {
                hit.transform.GetComponent<MeshRenderer>().enabled = true;
                sphere = hit.transform.GetComponent<MeshRenderer>();

                if (Input.GetMouseButtonDown(0))
                {
                    DataInputTable.SetActive(true);
                }
            }
            else
            {
                if (sphere == null) return;
                sphere.enabled = false;
            }
        }
    }
}
