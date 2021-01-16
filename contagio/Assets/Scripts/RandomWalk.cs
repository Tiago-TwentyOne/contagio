using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RandomWalk : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent navMeshAgent;

    private bool walk = false;
    public bool isQuarantined = false;

    private Vector3 newTarget;

    public GameObject gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        GenerateRandomTarget(0, 100, 0, -100);
    }

    void FixedUpdate()
    {
        
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            walk = false;
            //If is in quarantined define target inside quarentine
            if (isQuarantined)
            {
                GenerateRandomTarget(85, 100, -85, -100);
            }
            else
            {
                GenerateRandomTarget(0, 100, 0, -100);
            }
            
        }
        else
        {
            walk = true;
        }
        //Walk animation
        animator.SetBool("Walk", walk);

    }
   
    public void GenerateRandomTarget(float minX, float maxX, float minZ, float maxZ)
    {
        newTarget = new Vector3(Random.Range(minX, maxX), 1f, Random.Range(minZ, maxZ));
        navMeshAgent.SetDestination(newTarget);
    }

    public IEnumerator TestResultAction()
    {
        //Get player scripts
        var playerManagerScript = gameObject.GetComponent<PlayerManager>();
        var playerWalkScript = gameObject.GetComponent<RandomWalk>();
        //Check if player is infected
        var testResult = playerManagerScript.IsInfected();
        //Wait 5 seconds before show result
        yield return new WaitForSeconds(5);
        if (testResult)
        {
            //Show toast with test result
            StartCoroutine(gameManager.GetComponent<GameManager>().ShowToast("TESTE POSITIVO"));
            //Disable player scripts
            playerManagerScript.enabled = false;
            playerWalkScript.enabled = false;
            //Wait until toast finish to destroy player
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
            //Notify the gameManager that the player has been destroyed
            gameManager.GetComponent<GameManager>().PlayerInfectedDestroyed();
            
            
        }
        else
        {
            //Show toast with test result
            StartCoroutine(gameManager.GetComponent<GameManager>().ShowToast("TESTE NEGATIVO"));
            //Remove player from quarentine room
            navMeshAgent.Warp(new Vector3(83, 1, -95));
            GenerateRandomTarget(0, 100, 0, -100);
            isQuarantined = false;
        }
    }

    public void StartQuarentine()
    {
        isQuarantined = true;
        navMeshAgent.Warp(new Vector3(90, 1, -90));
        GenerateRandomTarget(85, 100, -85, -100);
        StartCoroutine("TestResultAction");
    }

    
}
