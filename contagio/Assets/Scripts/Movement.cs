using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    //public Collider collider;

    public bool walk = false;


    public Transform point;
    public GameObject particles;

    private Vector3 newTarget;
    private float minX;
    private float maxX;
    private float minZ;
    private float maxZ;

    public bool infected;
    public bool hasMask;

    public GameObject[] players;

    private int timer;
    private float timePassed;


    // Update is called once per frame
    private void Start()
    {
        minX = 0f;
        maxX = 100f;
        minZ = 0f;
        maxZ = -100f;

        players = GameObject.FindGameObjectsWithTag("Player");


        GenerateRandomTarget();
        timePassed = 0;

}
    private void Update()
    {
        
        if (infected)
        {
            if (!hasMask)
            {
                foreach (var p in players)
                {
                    var dist = Vector3.Distance(transform.position, p.transform.position);
                    if (dist < 2 && dist > 0)
                    {
                        var pScript = p.GetComponent<Movement>();
                        if (!pScript.infected && !pScript.hasMask)
                        {
                            pScript.GetInfected();
                        }
                    }
                }
            }
            timePassed += Time.deltaTime; 
            if(timePassed >= timer)
            {
                Debug.Log("Tosse");

                StartCoroutine(Cough());

                timer = Random.Range(15, 30);
                timePassed = 0;
            }

        }
    }
    void FixedUpdate()
    {
        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            walk = false;
            GenerateRandomTarget();
        }
        else
        {
            walk = true;
        }

        animator.SetBool("Walk", walk);

    }

    void GenerateRandomTarget()
    {
        newTarget = new Vector3(Random.Range(minX, maxX), 1f, Random.Range(minZ, maxZ));
        navMeshAgent.SetDestination(newTarget);
        point.position = newTarget;
    }

    public void GetInfected()
    {
        infected = true;
        timer = Random.Range(15, 30);
        point.GetComponent<GameManager>().playerIsNowInfected();
        Debug.Log("Infected");
    }

    public IEnumerator Cough()
    {
        animator.Play("Cough");
        var inst = Instantiate(particles, new Vector3(transform.position.x, transform.position.y + 1.9f, transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(1);
        Destroy(inst);

    }

    public void wearMask()
    {
        hasMask = true;
        var child = transform.GetChild(1);
        child.GetComponent<SkinnedMeshRenderer>().material.color = Color.blue;
    }

    public bool isInfected()
    {
        return infected;
    }

}
