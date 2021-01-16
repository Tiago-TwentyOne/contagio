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


    private float nInfectedPlayers;
    private float nPlayers;

    public int maskNum = 3;


    private GameObject playerSelected;

    public CameraMovement cameraMovement;

    public int prefabTimer = 10;
    private float timePassed;

    public GameObject toast;
    public Text toastText;

    public GameObject myPrefab;

    public GameObject resultUI;
    public Text resultText;
    void Start()
    {
        //Find players
        players = GameObject.FindGameObjectsWithTag("Player");
        //Make a player infected
        var randomPlayer = Random.Range(0, players.Length - 1);
        players[randomPlayer].GetComponent<PlayerManager>().GetInfected();
        nPlayers = players.Length;
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        //Check distance between player and infect 
        foreach (var p in players)
        {
            if(p != null) {
                var pScript = p.GetComponent<PlayerManager>();
                bool pIsInfected = pScript.infected;
                bool pHasMask = pScript.hasMask;
                if (pIsInfected && !pHasMask)
                {
                    foreach (var player in players)
                    {

                        if (player != null && player.GetInstanceID() != p.GetInstanceID())
                        {
                            var dist = Vector3.Distance(player.transform.position, p.transform.position);
                            if (dist < 3 && dist > 0) 
                            {
                                var playerScript = player.GetComponent<PlayerManager>();
                                if (!playerScript.infected && !playerScript.hasMask)
                                {
                                    playerScript.GetInfected();
                                }
                            }
                        }
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(0) && !PauseMenu.gameIsPaused)
        {
            //Check if the player was clicked and show player options
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, layerPlayers))
            {
                playerSelected = hit.transform.gameObject;
                ShowUI();
            }
        }

        //Timer to istanciate prefab
        timePassed += Time.deltaTime;

        if (timePassed >= prefabTimer)
        {
            Instantiate(myPrefab, new Vector3(70, 1, -95), Quaternion.identity);
            players = GameObject.FindGameObjectsWithTag("Player");
            nPlayers = players.Length;
            UpdateScore();
            timePassed = 0;
        }
        
    }
    //Pause game and show UI
    public void ShowUI()
    {
        PauseMenu.gameIsPaused = true;
        Time.timeScale = 0;

        bQuarentena.gameObject.SetActive(true);
        if (playerSelected.gameObject.GetComponent<RandomWalk>().isQuarantined)
        {
            bQuarentena.GetComponent<Button>().interactable = false;
        }
        else
        {
            bQuarentena.GetComponent<Button>().interactable = true;
        }
        maskText.text = $"Equipar Máscara ({maskNum})"; 
        bMascara.gameObject.SetActive(true);
        if (playerSelected.gameObject.GetComponent<PlayerManager>().hasMask || maskNum <= 0)
        {
            bMascara.GetComponent<Button>().interactable = false;
        }
        else
        {
            bMascara.GetComponent<Button>().interactable = true;
        }
        //bTeste.gameObject.SetActive(true);
        bCancelar.gameObject.SetActive(true);
    }
    //Unpause game and hide UI
    public void HideUI()
    {
        cameraMovement.enabled = true;
        Time.timeScale = 1;
        bQuarentena.gameObject.SetActive(false);
        bMascara.gameObject.SetActive(false);
        //bTeste.gameObject.SetActive(false);
        bCancelar.gameObject.SetActive(false);
        PauseMenu.gameIsPaused = false;
    }
    //Equip mask
    public void UseMask()
    {
        maskNum--;
        playerSelected.GetComponent<PlayerManager>().WearMask();
        HideUI();
    }

    //public void makeTest()
    //{
    //    testReadyToUse = false;
    //    testText.text = $"Fazer o teste ({testTimer}s)";
    //    bTeste.GetComponent<Button>().interactable = false;
    //    bool result = playerSelected.GetComponent<PlayerManager>().isInfected();
    //    Debug.Log(result.ToString());
    //}

    //Update  infected/players
    private void UpdateScore()
    {
        score.text = nInfectedPlayers.ToString() + " / " + nPlayers.ToString();
    }

    //Start quarentine
    public void Quarantine()
    {
        var playerWalkScript = playerSelected.GetComponent<RandomWalk>();
        playerWalkScript.StartQuarentine();
        HideUI();
    }

    //Check if no one is infected
    private void NoOneInfected()
    {
        if(nInfectedPlayers <= 0)
        {
            Time.timeScale = 0;
            resultText.text = "YOU WIN";
            PauseMenu.gameIsPaused = true;
            resultUI.SetActive(true);
        }
    }

    //Check if 65% of the population is infected
    private void CheckLoseCondition()
    {
        
        if(nInfectedPlayers > 1)
        {
            if (nInfectedPlayers / nPlayers >= 0.65f)
            {
                Time.timeScale = 0;
                resultText.text = "YOU LOSE";
                PauseMenu.gameIsPaused = true;
                resultUI.SetActive(true);
            }
        }
        
    }

    //When the player infected is destroyed update score and check if no one is infected
    public void PlayerInfectedDestroyed()
    {
        nInfectedPlayers--;
        nPlayers--;
        UpdateScore();
        NoOneInfected();
    }

    //When the player is infected update score and check lose condition
    public void PlayerIsNowInfected()
    {
        nInfectedPlayers += 1;
        CheckLoseCondition();
        UpdateScore();
    }

    
    public IEnumerator ShowToast(string message)
    {
        toastText.text = message;
        toast.SetActive(true);
        yield return new WaitForSeconds(2);
        toast.SetActive(false);
    }
}
