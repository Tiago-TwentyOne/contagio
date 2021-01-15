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
    private bool visibleUI = false;

    public int maskNum = 3;


    private GameObject playerSelected;

    public CameraMovement cameraMovement;

    public int prefabTimer = 10;
    private bool testReadyToUse = true;
    private float timePassed;

    public GameObject toast;
    public Text toastText;

    public GameObject myPrefab;
    void Start()
    {
       
        players = GameObject.FindGameObjectsWithTag("Player");
        var randomPlayer = Random.Range(0, players.Length - 1);
        players[randomPlayer].GetComponent<PlayerManager>().GetInfected();
        nPlayers = players.Length;
        updateScore();
    }

    // Update is called once per frame
    void Update()
    {
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
       
            timePassed += Time.deltaTime;

        if (timePassed >= prefabTimer)
        {
            Instantiate(myPrefab, new Vector3(70, 1, -95), Quaternion.identity);
            players = GameObject.FindGameObjectsWithTag("Player");
            nPlayers = players.Length;
            updateScore();
            timePassed = 0;

        }
        
    }

    public void showUI()
    {
        cameraMovement.enabled = false;
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
        visibleUI = true;
        
    }

    public void hideUI()
    {
        cameraMovement.enabled = true;
        Time.timeScale = 1;
        bQuarentena.gameObject.SetActive(false);
        bMascara.gameObject.SetActive(false);
        //bTeste.gameObject.SetActive(false);
        bCancelar.gameObject.SetActive(false);
        visibleUI = false;

    }

    public void useMask()
    {
        maskNum--;
        bMascara.GetComponent<Button>().interactable = false;
        maskText.text = $"Equipar Máscara ({maskNum})";
        playerSelected.GetComponent<PlayerManager>().wearMask();
    }

    //public void makeTest()
    //{
    //    testReadyToUse = false;
    //    testText.text = $"Fazer o teste ({testTimer}s)";
    //    bTeste.GetComponent<Button>().interactable = false;
    //    bool result = playerSelected.GetComponent<PlayerManager>().isInfected();
    //    Debug.Log(result.ToString());
    //}

    private void updateScore()
    {
        score.text = nInfectedPlayers.ToString() + " / " + nPlayers.ToString();
    }

    public void quarantine()
    {
        var playerWalkScript = playerSelected.GetComponent<RandomWalk>();
        playerWalkScript.startQuarentine();
        playerSelected.transform.position = new Vector3(90, 1, -90);
    }


    private void noOneInfected()
    {
        if(nInfectedPlayers <= 0)
        {
            Time.timeScale = 0;
            StartCoroutine(showToast("You win"));
        }
    }

    private void allInfected()
    {
        
        if(nInfectedPlayers > 1)
        {
            if (nInfectedPlayers / nPlayers >= 0.65f)
            {
                Time.timeScale = 0;
                StartCoroutine(showToast("You Lose"));
            }
        }
        
    }


    public void playerInfectedDestroyed()
    {
        nInfectedPlayers--;
        nPlayers--;
        updateScore();
        noOneInfected();
    }
    public void playerIsNowInfected()
    {
        nInfectedPlayers += 1;
        allInfected();
        updateScore();
    }

    public IEnumerator showToast(string message)
    {
        toastText.text = message;
        toast.SetActive(true);
        Debug.Log("Antes");
        yield return new WaitForSeconds(2);
        Debug.Log("Depois");
        toast.SetActive(false);
    }

}
