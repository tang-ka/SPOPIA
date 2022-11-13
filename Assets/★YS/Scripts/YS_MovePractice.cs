using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class YS_MovePractice : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Move();
        }
    }

    void Move()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // ÆÀ »ý¼º
            if (hit.transform.gameObject.name == "MovePracticeCube")
            {
                SceneManager.LoadScene("PlayGroundScene");
            }
        }
    }

    public void practice()
    {
        SceneManager.LoadScene("YS_ToPlayPractice");
    }
}
