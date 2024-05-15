using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Boss : Enemy
{
    protected override void Start()
    {
        base.Start();
        GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
    }

    public override void Die()
    {
        DeadEvent();
    }

    private void DeadEvent()
    {
        stats.isDead = true;
        anim.SetBool("Dead", true);
        FindObjectOfType<StrengthPatten>().deadCount++;
        Debug.Log(FindObjectOfType<StrengthPatten>().deadCount);
    }

}
