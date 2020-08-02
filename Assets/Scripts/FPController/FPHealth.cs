using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPHealth : MonoBehaviour
{
    public int maxHealth;
    public float recoverValue;
    public float currHealth;
    public bool touching;
    public float nextTime = 0;
    public float healthDelay;
    public float damageDelay;

    public Light mainLight;

  

    private void Start()
    {
        currHealth = maxHealth;
    }
    private void FixedUpdate()
    {
        if(!touching)
        {
            if (Time.time > nextTime)
            {
                nextTime = Time.time + healthDelay;
                RecoverHealth();
               
            }
        }

        if(touching)
        {
            if (Time.time > nextTime)
            {
                nextTime = Time.time + damageDelay;
                LoseHealth();

            }
        }

       if (currHealth < 1)
        {
            Destroy(gameObject);
        }

        //mainLight.intensity = currHealth / 1000;
    }

    void LoseHealth()
    {
        if(currHealth != 0)
        {
            currHealth-=5;
        }
    }

    void RecoverHealth()
    {
        if(currHealth <= maxHealth)
        {
            currHealth += recoverValue;
        }

    }

    void Touch()
    {
        touching = true;
    }

    void UnTouch()
    {
        touching = false;
    }




}
