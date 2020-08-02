﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorRot : MonoBehaviour
{
    public Transform pivot;
    public bool isOpen;
    public bool inDoorPos;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetKey(KeyCode.E) && !open)
    transform.RotateAround(pivot.position, Vector3.up, -30 * Time.deltaTime);
*/
        if (Input.GetKeyDown(KeyCode.E) && anim.GetBool("open") == false && inDoorPos)
        {
            Open();
        }
        else if ((Input.GetKeyDown(KeyCode.E) && anim.GetBool("open") == true && inDoorPos))
        {
            Close();
        }

        if(!inDoorPos && isOpen)
        {
            Close();

        }
    }

    void Open()
    {
        anim.SetBool("open", true);
        isOpen = true;
    }

    void Close()
    {
        anim.SetBool("open", false);
        isOpen = false;

    }

    /*private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("stop"))
        {
            open = true;
        }
    }*/
}



