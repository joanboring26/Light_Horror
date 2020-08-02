using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorLock : MonoBehaviour
{
    public doorScript drot;
    // Start is called before the first frame update
    void Start()
    {
        //drot = GetComponentInParent<doorRot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            drot.inDoorPos = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            drot.inDoorPos = false;
        }
    }

    
}
