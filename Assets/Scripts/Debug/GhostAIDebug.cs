using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAIDebug : MonoBehaviour
{
    public BaseGhostAI ghost;

    public Transform target;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ghost.AlertGhost(target, 0.2f, GhostStatus.ALERTCHASING);
        }
    }
}
