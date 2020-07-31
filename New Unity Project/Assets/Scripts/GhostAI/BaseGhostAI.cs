using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum GhostStatus
{
    CHASING,
    ALERTCHASING,
    IDLE
}

public class BaseGhostAI : BaseEntity
{
    public GameObject ghostVisuals;
    bool 

    public float visibleRange;
    public float alertRange;
    public float detectedPlayerAlertTime;
    public float impactShotAlertTime;
    public float missedShotAlertTime;

    public float chaseSpeed;
    public float alertSpeed;

    public GhostStatus currentStatus;
    public NavMeshAgent agent;
    Transform playerTransform;

    Coroutine AlertGhostCoroutine;



    public void Start()
    {
        playerTransform = FindObjectOfType<FPMover>().transform;
    }

    public void Update()
    {
        //This loop mainly controls whether the ghost should be visible or not and if the player is close enough to alert the ghost
        if (playerTransform != null && (playerTransform.position - transform.position).magnitude < visibleRange)
        {
            SetGhostVisible();

            //If the player is within the alert radius of the ghost, make the ghost chase the player
            if((playerTransform.position - transform.position).magnitude < alertRange)
            {
                //If the ghost wasn't chasing the player already then alert the ghost to the player's presence
                if(currentStatus != GhostStatus.CHASING)
                {
                    AlertGhost(playerTransform, detectedPlayerAlertTime, GhostStatus.CHASING);
                }
                else
                {
                    //If the player was already being chased, we reset the coroutine
                    if(AlertGhostCoroutine != null) { StopCoroutine(AlertGhostCoroutine); }
                    AlertGhost(playerTransform, detectedPlayerAlertTime, GhostStatus.CHASING);
                }

                currentStatus = GhostStatus.CHASING;
            }
        }
        else
        {
            SetGhostInvisible();
        }

        if(currentStatus == GhostStatus.CHASING)
        {
            agent.SetDestination(playerTransform.position);
        }
    }

    public void SetGhostInvisible()
    {
        ghostVisuals.SetActive(false);
    }

    public void SetGhostVisible()
    {
        ghostVisuals.SetActive(true);
    }

    //If we want to do something akin to shooting the gun and having the ghost move for a brief second towards the player, we need
    //to alert the ghost with the state ALERTCHASING and give it a time that the ghost will move in
    //This function is used to alert the ghost of the player's presence, the behaviour depends on what status we give the ghost
    public void AlertGhost(Transform alertPosition, float alertTime, GhostStatus gStatus)
    {
        switch(gStatus)
        {
            case GhostStatus.ALERTCHASING:
                //If the ghost was already chasing the player then we don't do anything, this is to ensure that firing the gun
                //while a ghost is already chasing us doesn't make the ghost switch to the ALERTCHASING status, which has a much shorter time
                if(currentStatus != GhostStatus.CHASING) 
                { 
                    currentStatus = gStatus; 
                    agent.speed = alertSpeed;
                }
                else
                {
                    //If the player was already being chased, we reset the coroutine
                    if (AlertGhostCoroutine != null) { StopCoroutine(AlertGhostCoroutine); }
                    agent.speed = chaseSpeed;
                    alertTime = detectedPlayerAlertTime;
                }
                break;
            case GhostStatus.CHASING:
                agent.speed = chaseSpeed;
                currentStatus = GhostStatus.CHASING;
                break;
            case GhostStatus.IDLE:
                //I DUNNO DO NOTHING I GUESS?
                agent.speed = chaseSpeed;
                currentStatus = GhostStatus.IDLE;
                break;
        }

        agent.SetDestination(alertPosition.position);
        AlertGhostCoroutine = StartCoroutine(AlertGhostMove(alertTime));
    }

    //Function that modifies the health of the ghost, activated when it gets hit by the player with boolets
    public override void ModHealth(int modValue, BaseEntity damagerBase)
    {
        if ((playerTransform.position - transform.position).magnitude < visibleRange)
        {
            hp += modValue;
            AlertGhost(damagerBase.transform, impactShotAlertTime, GhostStatus.CHASING);
        }
        else
        {
            AlertGhost(damagerBase.transform, missedShotAlertTime, GhostStatus.ALERTCHASING);
        }

        if(hp <= 0)
        {
            Die();
        }
    }

    //Function that activates when ghost dies, we'll put in a bunch of sfx here
    public void Die()
    {

        //This should be executed last
        Destroy(gameObject);
    }

    public IEnumerator AlertGhostMove(float gAlertTime)
    {
        yield return new WaitForSeconds(gAlertTime);
        currentStatus = GhostStatus.IDLE;
        agent.SetDestination(agent.transform.position);
    }


}
