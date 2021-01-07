using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] players;
    public int numInfected;
    public LayerMask layerPlayers;
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        var randomPlayer = Random.Range(0, players.Length - 1);
        players[randomPlayer].GetComponent<Movement>().GetInfected();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, layerPlayers))
            {
                Debug.Log(hit.transform.tag);
            }
        }
    }

    //void InfectPlayers(int num)
    //{
    //    var i = 0;
    //    while(i < num)
    //    {
    //        var randomPlayer = Random.Range(0, players.Length - 1);
    //        players[randomPlayer].GetComponent<Movement>().GetInfected();
            
    //    }
    //}
}
