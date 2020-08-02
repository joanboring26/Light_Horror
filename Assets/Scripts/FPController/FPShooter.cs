using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class FPShooter : MonoBehaviour
{
    public Light shotLight;
    public AudioSource gunSrc;
    public AudioClip gunSnd;
    public AudioClip openCylinder;
    public AudioClip closeCylinder;
    public AudioClip insertBullet;
    public Animator gunAnim;

    public Transform shootPoint;

    public GameObject bullet;
    public int gunDamage;

    public FPMover playerMover;
    public FPLooker playerLooker;
    public GameObject HUDCylinder;
    public GameObject[] HUDBullets;
    public bool isOpenCylinder = false;
    public int bulletsInMag;//Bullets that are left in the magazine also tracks the current bullet slot selected for the revolver HUD
    public int maxBullets = 6; //ITS A REVOLVER YOU NUMBNUTS OF COURSE IT HAS 6 BULLETS!
    public int bulletReserve = 0;
    public Text bulletReserveText;

    public float gunRecoil;
    public float bulletSpeed;
    public float alertRange;
    public float visibleRadius;
    public float muzzleTime;
    public float fireRate;
    public float loadBulletDelay;

    Vector3 initPos;
    float defaultMove;

    // Start is called before the first frame update
    void Start()
    {
        defaultMove = playerMover.speed;
        initPos = shootPoint.localPosition;
        gunAnim = GetComponentInChildren<Animator>();
    }

    public void AddAmmo(int ammo)
    {
        bulletReserve += ammo;
        bulletReserveText.text = "x" + bulletReserve.ToString();
    }

    float bulletLoadNextTime = 0;
    float fireNextTime = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse1))
        {
            shootPoint.transform.localPosition = new Vector3(0, -0.07f, initPos.z);
            //Reduce the movement speed when the player aims
            playerMover.speed = 2.5f;
        }
        else
        {
            shootPoint.transform.localPosition = initPos;
            //Restore the movement speed when the player stops aiming
            playerMover.speed = defaultMove;
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(isOpenCylinder)
            {
                isOpenCylinder = false;
                HUDCylinder.SetActive(false);
                gunSrc.PlayOneShot(closeCylinder);
            }
            else
            {
                isOpenCylinder = true;
                HUDCylinder.SetActive(true);
                gunSrc.PlayOneShot(closeCylinder);
            }
        }

        if(Input.GetButtonDown("Fire1"))
        {
            if (isOpenCylinder)
            {
                if(bulletReserve > 0 && bulletsInMag < maxBullets && Time.time > bulletLoadNextTime)
                {
                    bulletLoadNextTime = Time.time + loadBulletDelay;
                    bulletsInMag++;
                    bulletReserve--;
                    bulletReserveText.text = "x" + bulletReserve.ToString();
                    HUDBullets[bulletsInMag - 1].SetActive(true);
                    gunSrc.PlayOneShot(insertBullet);
                }
            }
            else
            {
                if (bulletsInMag > 0 && Time.time > fireNextTime)
                {
                    shotLight.enabled = true;
                    StartCoroutine(flashDelay());
                    //Make the camera recoil
                    playerLooker.rotVal = gunRecoil;
                    //HIde the bullet that was shot in the hud
                    fireNextTime = Time.time + fireRate;
                    HUDBullets[bulletsInMag - 1].SetActive(false);
                    bulletsInMag--;

                    gunAnim.SetTrigger("fire");
                    gunSrc.PlayOneShot(gunSnd);
                    GameObject newBullet = Instantiate(bullet, shootPoint.transform.position, transform.rotation);
                    Rigidbody brb = newBullet.GetComponent<Rigidbody>();
                    brb.AddRelativeForce(0f, 0f, bulletSpeed, ForceMode.VelocityChange);
                    Destroy(newBullet, 0.5f);

                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, shootPoint.forward, out hit, 30f, 1 << 8, QueryTriggerInteraction.Collide))
                    {
                        Debug.DrawLine(transform.position, hit.point, Color.yellow, 10f);
                        BaseEntity health = hit.transform.gameObject.GetComponent<BaseEntity>();
                        if (health != null)
                        {
                            health.ModHealth(gunDamage, health);
                        }
                    }

                    Collider[] ghosts = Physics.OverlapSphere(transform.position, visibleRadius, 1 << 8);
                    for (int i = 0; i < ghosts.Length; i++)
                    {
                        BaseGhostAI ghostBase = ghosts[i].GetComponent<BaseGhostAI>();
                        if (ghostBase != null)
                        {
                            ghostBase.SetGhostVisible();
                            ghostBase.beingRevealed = true;
                            if ((ghostBase.transform.position - transform.position).magnitude < alertRange)
                            {
                                ghostBase.AlertGhost(transform, muzzleTime, GhostStatus.ALERTCHASING);
                            }
                        }
                    }
                }
            }
        }
    }

    public IEnumerator flashDelay()
    {
        yield return new WaitForSeconds(muzzleTime);
        shotLight.enabled = false;
    }
}
