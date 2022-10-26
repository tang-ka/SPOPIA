using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCustomManager : MonoBehaviour
{
    public GameObject obj;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateObj()
    {
        GameObject Go = Instantiate(obj, Vector3.zero, Quaternion.identity);
        Go.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 30f);
    }
}
