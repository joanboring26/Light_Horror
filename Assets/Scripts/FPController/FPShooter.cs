using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FPShooter : MonoBehaviour
{
    public AudioSource gunSrc;
    public AudioClip gunSnd;
    public AudioClip gunReload;

    public Transform shootPoint;

    public GameObject bullet;
    public int gunDamage;

    public GameObject HUDCylinder;
    public GameObject[] HUDBullets;
    public int bulletsInMag;//Bullets that are left in the magazine also tracks the current bullet slot selected for the revolver HUD
    public int maxBullets = 6; //ITS A REVOLVER YOU NUMBNUTS OF COURSE IT HAS 6 BULLETS!
    public int bulletReserve = 0;

    public float bulletSpeed;
    public float alertRadius;
    public float muzzleTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddAmmo(int ammo)
    {
        bulletReserve += ammo;
    }



    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {

        }

        if(Input.GetButtonDown("Fire1"))
        {
            gunSrc.PlayOneShot(gunSnd);
            GameObject newBullet = Instantiate(bullet, shootPoint.transform.position, transform.rotation);
            Rigidbody brb = newBullet.GetComponent<Rigidbody>();
            brb.AddRelativeForce(0f, 0f, bulletSpeed, ForceMode.VelocityChange);
            Destroy(newBullet, 0.5f);

            RaycastHit hit;
            if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit))
            {
                BaseEntity health = hit.transform.gameObject.GetComponent<BaseEntity>();
                if (health != null)
                {
                    health.ModHealth(gunDamage, health);
                }
            }

            Collider[] ghosts = Physics.OverlapSphere(transform.position, alertRadius, 1 << 8);
            for(int i = 0; i < ghosts.Length; i++)
            {
                BaseGhostAI ghostBase = ghosts[i].GetComponent<BaseGhostAI>();
                if(ghostBase != null)
                {
                    ghostBase.SetGhostVisible();
                    ghostBase.beingRevealed = true;
                    ghostBase.AlertGhost(transform, muzzleTime, GhostStatus.ALERTCHASING);
                }
            }
        }
    }
}
