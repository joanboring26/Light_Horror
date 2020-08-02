using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorRot : MonoBehaviour
{
    public Transform pivot;
    public bool isOpen;
    public bool inDoorPos;
    public bool slide;
    public bool stationary;
    public Animator anim;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && anim.GetBool("open") == false && inDoorPos)
        {
            if (!locked)
            {
                Open();
                StopAllCoroutines();
                StartCoroutine(sndDisable());
                sndSrc.enabled = true;
                if (creaky)
                {
                    sndSrc.PlayOneShot(c_openSnd);
                }
                else
                {
                    sndSrc.PlayOneShot(openSnd);
                }
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(sndDisable());
                sndSrc.enabled = true;
                sndSrc.PlayOneShot(lockedSnd);
            }
        }
        else if ((Input.GetKeyDown(KeyCode.E) && anim.GetBool("open") == true && inDoorPos))
        {
            if (!locked)
            {
                Close();
                StopAllCoroutines();
                StartCoroutine(sndDisable());
                sndSrc.enabled = true;
                if (creaky)
                {
                    sndSrc.PlayOneShot(c_closeSnd);
                }
                else
                {
                    sndSrc.PlayOneShot(closeSnd);
                }
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(sndDisable());
                sndSrc.enabled = true;
                sndSrc.PlayOneShot(lockedSnd);
            }
        }

        if (!inDoorPos && isOpen)
        {
            Close();
            StopAllCoroutines();
            StartCoroutine(sndDisable());
            sndSrc.enabled = true;
            if (creaky)
            {
                sndSrc.PlayOneShot(c_closeSnd);
            }
            else
            {
                sndSrc.PlayOneShot(closeSnd);
            }
        }
    }

    void Open()
    {

        if (stationary)
            anim.SetBool("stationary", true);
        if (slide)
            anim.SetBool("slide", true);

        anim.SetBool("open", true);
        isOpen = true;
        isOpen = true;
    }

    void Close()
    {
        anim.SetBool("open", false);
        isOpen = false;

    }

    public IEnumerator sndDisable()
    {
        yield return new WaitForSeconds(3.5f);
        sndSrc.enabled = false;
    }
}



