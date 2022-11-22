using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectSportsManager : MonoBehaviour
{
    public GameObject center;
    public float rotSpeed;
    Quaternion rot;
    float rot_y; // eulerAnlges값

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

            if(center.transform.eulerAngles.y >= rot_y  - 0.05f && center.transform.rotation.y <= rot_y + 0.05f)
            {
                Turn();
            }
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

    void Turn()
    {
        rot = Quaternion.Euler(0, center.transform.eulerAngles.y + 120, 0);
        rot_y = center.transform.eulerAngles.y + 120;

        // rot_y는 eulerAngles이라서 360을 넘어가면 빼준다.
        if(rot_y >= 350)
        {
            rot_y -= 360;
        }
    }
}
