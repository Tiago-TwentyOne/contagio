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
    public Text maskText;
    public Text testText;


    private int nInfectedPlayers;
    private int nPlayers;
    private bool visibleUI = false;

    public int maskNum = 3;


    private GameObject playerSelected;

    public CameraMovement cameraMovement;

    public int testTimer = 20;
    private bool testReadyToUse = true;
    private float timePassed;
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
        if (Input.GetMouseButtonDown(0) && !visibleUI)
        {
            Time.timeScale = 1;
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, layerPlayers))
            {
                playerSelected = hit.transform.gameObject;
                showUI();
            }
        }
        if (!testReadyToUse)
        {
            timePassed += Time.deltaTime;
            testText.text = $"Fazer o teste ({(int)(testTimer - timePassed)}s)";
            if (timePassed >= testTimer)
            {
                testText.text = "Fazer o teste";
                bTeste.GetComponent<Button>().interactable = true;
                timePassed = 0;
                testReadyToUse = true;
            }
        }
    }

    public void showUI()
    {
        cameraMovement.enabled = false;
        Time.timeScale = 0;
        bQuarentena.gameObject.SetActive(true);
        maskText.text = $"Equipar Máscara ({maskNum})"; 
        bMascara.gameObject.SetActive(true);
        if (playerSelected.gameObject.GetComponent<Movement>().hasMask || maskNum <= 0)
        {
            bMascara.GetComponent<Button>().interactable = false;
        }
        else
        {
            bMascara.GetComponent<Button>().interactable = true;
        }
        bTeste.gameObject.SetActive(true);
        bCancelar.gameObject.SetActive(true);
        visibleUI = true;
        
    }

    public void hideUI()
    {
        cameraMovement.enabled = true;
        Time.timeScale = 1;
        bQuarentena.gameObject.SetActive(false);
        bMascara.gameObject.SetActive(false);
        bTeste.gameObject.SetActive(false);
        bCancelar.gameObject.SetActive(false);
        visibleUI = false;

    }

    public void useMask()
    {
        maskNum--;
        bMascara.GetComponent<Button>().interactable = false;
        maskText.text = $"Equipar Máscara ({maskNum})";
        playerSelected.GetComponent<Movement>().wearMask();
    }

    public void makeTest()
    {
        testReadyToUse = false;
        testText.text = $"Fazer o teste ({testTimer}s)";
        bTeste.GetComponent<Button>().interactable = false;
        bool result = playerSelected.GetComponent<Movement>().isInfected();
        Debug.Log(result.ToString());
    }

    private void updateScore()
    {
        score.text = nInfectedPlayers.ToString() + " / " + nPlayers.ToString();
    }

    public void quarantine()
    {
        var playerSelectedScript = playerSelected.GetComponent<Movement>();
        nPlayers--;
        if (playerSelectedScript.infected)
        {
            nInfectedPlayers--;
            noOneInfected();
        }
        updateScore();
        playerSelectedScript.enabled = false;
        playerSelected.transform.position = new Vector3(100, 1, 100);
    }

    private void noOneInfected()
    {
        if(nInfectedPlayers <= 0)
        {
            Debug.Log("You Win");
        }
    }

    private void allInfected()
    {
        if(nInfectedPlayers == nPlayers)
        {
            Debug.Log("You Lose");
        }
    }

    public void playerIsNowInfected()
    {
        nInfectedPlayers += 1;
        allInfected();
        updateScore();
    }

}
