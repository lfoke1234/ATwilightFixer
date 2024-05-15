using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Boss : WorldObject
{
    protected override void Start()
    {
        base.Start();
        GetComponentInChildren<Animator>();
    }

    protected override void DeadEvent()
    {

    }

}
