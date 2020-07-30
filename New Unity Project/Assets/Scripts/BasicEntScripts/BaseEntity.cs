using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    // Contains basic monobehaviour data an ID for the entity and hp with death system
    public int ID = 0;
    public int hp;
    public virtual void ModHealth(int modValue, BaseEntity damagerBase)
    {

    }

    public virtual void Death()
    {

    }
}