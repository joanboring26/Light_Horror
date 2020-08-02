using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Transform openPosition;
    Vector3 closedPosition;
    Quaternion closedRotation;
    public bool closed;
    public bool locked;
    public bool creaky;
    public AudioSource sndSrc;
    public AudioClip lockedSnd;
    public AudioClip openSnd;
    public AudioClip c_openSnd;
    public AudioClip closeSnd;
    public AudioClip c_closeSnd;

    public void Start()
    {
        closedPosition = transform.position;
        closedRotation = transform.rotation;
    }

    public void OpenDoor()
    {
        closed = false;
        transform.position = openPosition.position;
        transform.rotation = openPosition.rotation;
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

    public void CloseDoor()
    {
        closed = true;
        transform.position = closedPosition;
        transform.rotation = closedRotation;
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
            if (closed)
            {
                OpenDoor();
            }
            else
            {
                CloseDoor();
            }
        }
    }

    public IEnumerator sndDisable()
    {
        yield return new WaitForSeconds(3.5f);
        sndSrc.enabled = false;
    }
}
