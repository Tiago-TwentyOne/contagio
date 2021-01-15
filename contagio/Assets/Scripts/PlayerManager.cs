using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    public Animator animator;
    //public NavMeshAgent navMeshAgent;
    //public Collider collider;

    public GameObject gameManager;
    public GameObject particles;


    public bool infected;
    public bool hasMask;

    public GameObject[] players;

    private int timer;
    private float timePassed;


    // Update is called once per frame
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        players = GameObject.FindGameObjectsWithTag("Player");
        timePassed = 0;
}
    private void Update()
    {
        if (infected)
        {
            
            timePassed += Time.deltaTime; 
            if(timePassed >= timer)
            {
                Debug.Log("Tosse");

                StartCoroutine(Cough());
                GetComponent<AudioSource>().Play();

                timer = Random.Range(15, 30);
                timePassed = 0;
            }

        }
    }
    

    

    public void GetInfected()
    {
        infected = true;
        timer = Random.Range(15, 30);
        gameManager.GetComponent<GameManager>().playerIsNowInfected();
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
