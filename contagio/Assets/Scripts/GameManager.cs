using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] players;
    public int numInfected;
    public LayerMask layerPlayers;

    public Button bQuarentena;
    public Button bMascara;
    public Button bTeste;
    public Button bCancelar;

    public Text score;

    private int nInfectedPlayers;
    private int nPlayers;


    private GameObject playerSelected;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        var randomPlayer = Random.Range(0, players.Length - 1);
        players[randomPlayer].GetComponent<Movement>().GetInfected();
        nPlayers = players.Length;
        nInfectedPlayers = 1;
        updateScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Time.timeScale = 1;
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, layerPlayers))
            {
                Time.timeScale = 0;
                playerSelected = hit.transform.gameObject;
                bQuarentena.gameObject.SetActive(true);
                bMascara.gameObject.SetActive(true);
                bTeste.gameObject.SetActive(true);
                bCancelar.gameObject.SetActive(true);
                

            }
        }
    }

    public void hideUI()
    {
        Time.timeScale = 1;
        bQuarentena.gameObject.SetActive(false);
        bMascara.gameObject.SetActive(false);
        bTeste.gameObject.SetActive(false);
        bCancelar.gameObject.SetActive(false);
    }

    public void useMask()
    {
        playerSelected.GetComponent<Movement>().wearMask();
    }

    public void makeTest()
    {
        bool result = playerSelected.GetComponent<Movement>().isInfected();
    }

    private void updateScore()
    {
        score.text = nInfectedPlayers.ToString() + " / " + players.Length.ToString();
    }

    public void playerIsNowInfected()
    {
        nInfectedPlayers += 1;
        updateScore();
    }

}
