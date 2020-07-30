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
    public float visibleRange;
    public float impactShotAlertTime;
    public float missedShotAlertTime;

    public float chaseSpeed;
    public float alertSpeed;

    public GhostStatus currentStatus;
    public NavMeshAgent agent;
    Transform playerTransform;

    public void Start()
    {
    }

    //
    public void AlertGhost(Transform alertPosition, float alertTime, GhostStatus gStatus)
    {
        currentStatus = gStatus;
        switch(currentStatus)
        {
            case GhostStatus.ALERTCHASING:
                agent.speed = alertSpeed;
                break;
            case GhostStatus.CHASING:
                agent.speed = chaseSpeed;
                break;
            case GhostStatus.IDLE:
                //I DUNNO DO NOTHING I GUESS?
                agent.speed = chaseSpeed;
                break;
        }

        agent.SetDestination(alertPosition.position);
        StartCoroutine(AlertGhostMove(alertTime));
    }

    // Update is called once per frame
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
    }

    public IEnumerator AlertGhostMove(float gAlertTime)
    {
        yield return new WaitForSeconds(gAlertTime);
        currentStatus = GhostStatus.IDLE;
        agent.SetDestination(agent.transform.position);
    }
}
