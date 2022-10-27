using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_PlayerCrossHair : MonoBehaviour
{
    public Transform cam;

    void Start()
    {
        
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 20))
        {
            if (hit.transform.gameObject.name == "MatchDataSphere")
            {
                hit.transform.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }
}
