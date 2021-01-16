﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    public Animator animator;

    public GameObject gameManager;
    public GameObject particles;


    public bool infected;
    public bool hasMask;

    public GameObject[] players;

    private int timer;
    private float timePassed;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
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
    

    
    //Get infected, start the timer to cough and notify game manager  
    public void GetInfected()
    {
        infected = true;
        timer = Random.Range(15, 30);
        gameManager.GetComponent<GameManager>().PlayerIsNowInfected();
    }

    //Play Cough animation and show particles
    public IEnumerator Cough()
    {
        animator.Play("Cough");
        var inst = Instantiate(particles, new Vector3(transform.position.x, transform.position.y + 1.9f, transform.position.z), Quaternion.identity);
        yield return new WaitForSeconds(1);
        Destroy(inst);

    }
    //Wear mask and turn player blue
    public void WearMask()
    {
        hasMask = true;
        var child = transform.GetChild(1);
        child.GetComponent<SkinnedMeshRenderer>().material.color = Color.blue;
    }

    public bool IsInfected()
    {
        return infected;
    }

}
