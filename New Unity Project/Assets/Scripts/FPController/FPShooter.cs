using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPShooter : MonoBehaviour
{
    public Transform shootPoint;
    public GameObject bullet;
    public int gunDamage;
    public float bulletSpeed;
    public float muzzleTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
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
        }
    }
}
