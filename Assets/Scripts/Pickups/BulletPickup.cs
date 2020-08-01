using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPickup : MonoBehaviour
{
    public GameObject visuals;
    bool touched = false;
    private void OnTriggerEnter(Collider collision)
    {
        if (!touched)
        {
            touched = true;
            FindObjectOfType<FPShooter>().AddAmmo(1);
            gameObject.GetComponent<AudioSource>().Play();
            visuals.SetActive(false);
            Destroy(gameObject, 2f);
        }
    }
}
