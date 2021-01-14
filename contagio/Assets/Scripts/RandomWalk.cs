using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RandomWalk : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent navMeshAgent;

    private bool walk = false;
    public bool isQuarantined = false;
    public bool startedQuarentine = false;

    private Vector3 newTarget;

    public GameManager gameManager;

    private void Start()
    {

       GenerateRandomTarget(0, 100, 0, -100);
    }

    void FixedUpdate()
    {
        if (startedQuarentine)
        {
            navMeshAgent.Warp(new Vector3(90, 1, -90));
            GenerateRandomTarget(85, 100, -85, -100);
            StartCoroutine("testResultAction");
            startedQuarentine = false;    
        }
        
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            walk = false;
           
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

        animator.SetBool("Walk", walk);

    }

    public void GenerateRandomTarget(float minX, float maxX, float minZ, float maxZ)
    {
        newTarget = new Vector3(Random.Range(minX, maxX), 1f, Random.Range(minZ, maxZ));
        navMeshAgent.SetDestination(newTarget);
    }

    public IEnumerator testResultAction()
    {
        var playerManagerScript = gameObject.GetComponent<PlayerManager>();
        var playerWalkScript = gameObject.GetComponent<RandomWalk>();
        var testResult = playerManagerScript.isInfected();
        yield return new WaitForSeconds(5);
        if (testResult)
        {
            StartCoroutine(gameManager.showToast("Positivo"));
            playerManagerScript.enabled = false;
            playerWalkScript.enabled = false;
            Destroy(gameObject, 2.01f);
            
        }
        else
        {
            StartCoroutine(gameManager.showToast("Negativo"));
            navMeshAgent.Warp(new Vector3(83, 1, -95));
            GenerateRandomTarget(0, 100, 0, -100);
            isQuarantined = false;
        }
    }

    
}
