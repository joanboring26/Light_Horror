using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorRot : MonoBehaviour
{
    public Animator anim;
    public Transform pivot;
    public bool open;
    public bool locked;
    public bool creaky;
    public AudioSource sndSrc;
    public AudioClip lockedSnd;
    public AudioClip openSnd;
    public AudioClip c_openSnd;
    public AudioClip closeSnd;
    public AudioClip c_closeSnd;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        StopAllCoroutines();
        StartCoroutine(sndDisable());
        if (creaky)
        {
            sndSrc.PlayOneShot(c_closeSnd);
        }
        else
        {
            sndSrc.PlayOneShot(closeSnd);
        }
        anim.SetBool("open", true);
    }

    public void CloseDoor()
    {
        StopAllCoroutines();
        StartCoroutine(sndDisable());
        if (creaky)
        {
            sndSrc.PlayOneShot(c_closeSnd);
        }
        else
        {
            sndSrc.PlayOneShot(closeSnd);
        }
        anim.SetBool("open", false);
    }

    public void InteractDoor()
    {
        if (locked)
        {
            sndSrc.PlayOneShot(lockedSnd);
            StopAllCoroutines();
            StartCoroutine(sndDisable());
        }
        else
        {
            if (anim.GetBool("open"))
            {
                OpenDoor();
            }
            else
            {
                CloseDoor();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.E) && !anim.GetBool("open"))
        {
            anim.SetBool("open", true);
        }
        else if ((Input.GetKeyDown(KeyCode.E) && anim.GetBool("open")))
        {
            anim.SetBool("open", false);
        }
        */
    }

    public IEnumerator sndDisable()
    {
        yield return new WaitForSeconds(3.5f);
        sndSrc.enabled = false;
    }
}



