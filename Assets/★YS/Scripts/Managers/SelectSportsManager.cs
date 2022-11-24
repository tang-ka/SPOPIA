using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectSportsManager : MonoBehaviour
{
    public GameObject center;
    public float rotSpeed;
    Quaternion rot;
    float rot_y; // eulerAnlges��

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Turn Lerp
        center.transform.rotation = Quaternion.Lerp(center.transform.rotation, rot, rotSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            SelectSports();
        }
    }

    void SelectSports()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if(hit.transform.gameObject.name == "Soccer")
            {
                SceneManager.LoadScene("LeagueChoiceScene");
            }
        }
    }

    public void RightTurn()
    {
        rot_y += 120;
        rot = Quaternion.Euler(0, rot_y, 0);
    }

    public void LeftTurn()
    {
        rot_y -= 120;
        rot = Quaternion.Euler(0, rot_y, 0);
    }
}
