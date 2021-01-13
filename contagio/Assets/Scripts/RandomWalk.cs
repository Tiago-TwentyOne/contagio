using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomWalk : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent navMeshAgent;

    private bool walk = false;
    public bool isQuarantined = false;

    private Vector3 newTarget;

    private void Start()
    {

       GenerateRandomTarget(0, 100, 0, -100);
    }

    void FixedUpdate()
    {
        
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
}
