using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHead : BaseEntity
{
    public BaseGhostAI ghostScript;
    //This should only activate on a headshot, so the ghost instantly dies
    public override void ModHealth(int modValue, BaseEntity damagerBase)
    {
        ghostScript.Die();
    }
}
