using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAttack : MonoBehaviour
{
    public int damage;
    public float attackDelay;
    float nextTime = 0;
    private void OnTriggerEnter(Collider collision)
    {
        /* if (Time.time > nextTime)
         {
             nextTime = Time.time + attackDelay;
             BaseEntity dam = new BaseEntity();
             collision.GetComponent<BaseEntity>().ModHealth(damage, dam);
         } */
        collision.SendMessage("Touch");
    }

    private void OnTriggerExit(Collider other)
    {
        other.SendMessage("UnTouch");

    }
}
