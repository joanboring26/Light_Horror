using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public doorRot door;
    private void OnTriggerEnter(Collider collision)
    {
        gameObject.GetComponent<AudioSource>().enabled = true;
        gameObject.GetComponent<AudioSource>().Play();
        door.locked = false;
        Destroy(gameObject, 1f);
    }
}
