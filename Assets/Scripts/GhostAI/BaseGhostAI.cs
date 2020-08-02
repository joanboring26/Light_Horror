using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum GhostStatus
{
    CHASING,
    ALERTCHASING,
    IDLE,
    STUNNED
}

public class BaseGhostAI : BaseEntity
{
    public bool chaseInDark;

    public AudioSource chaseSrc;
    public AudioSource sndSrc;
    public AudioClip[] deathSnd;
    public AudioClip[] alertSnd;
    public AudioClip damagedSnd;

    public GameObject ghostVisuals;
    public Animator ghostAnims;
    public bool beingRevealed = false;

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
        SetGhostInvisible();
        playerTransform = FindObjectOfType<FPMover>().transform;
        ghostAnims = GetComponentInParent<Animator>();
    }

    public void Update()
    {
        if (currentStatus != GhostStatus.STUNNED && chaseInDark)
        {
            //This loop mainly controls whether the ghost should be visible or not and if the player is close enough to alert the ghost
            if (playerTransform != null && (playerTransform.position - transform.position).magnitude < visibleRange && agent.enabled)
            {
                SetGhostVisible();

                //If the player is within the alert radius of the ghost, make the ghost chase the player
                if ((playerTransform.position - transform.position).magnitude < alertRange)
                {
                    //If the ghost wasn't chasing the player already then alert the ghost to the player's presence
                    if (currentStatus != GhostStatus.CHASING)
                    {
                        AlertGhost(playerTransform, detectedPlayerAlertTime, GhostStatus.CHASING);
                        sndSrc.PlayOneShot(alertSnd[Random.Range(0, alertSnd.Length)]);
                    }
                    else
                    {
                        //If the player was already being chased, we reset the coroutine
                        if (AlertGhostCoroutine != null) { StopCoroutine(AlertGhostCoroutine); }
                        AlertGhost(playerTransform, detectedPlayerAlertTime, GhostStatus.CHASING);
                    }

                    currentStatus = GhostStatus.CHASING;
                }
            }
            else if (!beingRevealed)
            {
                SetGhostInvisible();
            }

         

            if (currentStatus == GhostStatus.CHASING && agent.enabled)
            {
                agent.SetDestination(playerTransform.position);
            }
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
                    sndSrc.PlayOneShot(alertSnd[Random.Range(0, alertSnd.Length)]);
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
                if (currentStatus != GhostStatus.CHASING) { StartCoroutine(GhostChaseSnd()); }
                agent.speed = chaseSpeed;
                currentStatus = GhostStatus.CHASING;
                break;
            case GhostStatus.IDLE:
                //I DUNNO DO NOTHING I GUESS?
                agent.speed = chaseSpeed;
                currentStatus = GhostStatus.IDLE;
                break;
        }

        ghostAnims.SetBool("Running", true);
        agent.SetDestination(alertPosition.position);
        AlertGhostCoroutine = StartCoroutine(AlertGhostMove(alertTime));
    }

    //Function that modifies the health of the ghost, activated when it gets hit by the player with boolets
    public override void ModHealth(int modValue, BaseEntity damagerBase)
    {
        hp += modValue;
        sndSrc.PlayOneShot(damagedSnd);
        if (chaseInDark)
        {
            AlertGhost(damagerBase.transform, impactShotAlertTime, GhostStatus.CHASING);
        }

        if(hp <= 0)
        {
            Die();
        }
    }

    //Function that activates when ghost dies, we'll put in a bunch of sfx here
    public void Die()
    {
        sndSrc.PlayOneShot(deathSnd[Random.Range(0,deathSnd.Length)]);

        agent.enabled = false;
        agent.gameObject.GetComponent<AudioSource>().Stop();
        chaseSrc.Stop();
        ghostVisuals.SetActive(false);

        //This should be executed last
        Destroy(agent.gameObject,2.8f);
    }

    public IEnumerator AlertGhostMove(float gAlertTime)
    {
        yield return new WaitForSeconds(gAlertTime);
        ghostAnims.SetBool("Running", false);
        currentStatus = GhostStatus.IDLE;
        agent.SetDestination(agent.transform.position);
        ghostVisuals.SetActive(false);
        beingRevealed = false;
    }

    public IEnumerator GhostChaseSnd()
    {
        chaseSrc.volume = 0;
        chaseSrc.enabled = true;
        chaseSrc.Play();
        while(chaseSrc.volume < 0.9f && chaseSrc.volume != 1)
        {
            chaseSrc.volume = chaseSrc.volume + 3f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        while(currentStatus == GhostStatus.CHASING)
        {
            yield return new WaitForEndOfFrame();
        }

        while (chaseSrc.volume > 0f)
        {
            chaseSrc.volume = chaseSrc.volume - 0.4f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        chaseSrc.Stop();
        chaseSrc.enabled = false;
    }

    public IEnumerator StunGhost()
    {

        yield return new WaitForSeconds(2f);
    }
}
