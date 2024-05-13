using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAnimationTrigger : MonoBehaviour
{
    private bool hasDamaged = false;
    private BoxCollider2D cd;
    public float percent = 0.1f;

    private void Start()
    {
        cd = GetComponent<BoxCollider2D>();
    }

    public void ActiveCollision()
    {
        cd.enabled = true;
    }

    public void SetPercent(float value) => percent = value;

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.GetComponent<PlayerStats>() != null)
        {
            float damage = target.GetComponent<PlayerStats>().GetMaxHealthValue() * percent;

            if (hasDamaged == false)
            {
                target.GetComponent<PlayerStats>().TakeDamage((int)damage);
                hasDamaged = true;
            }
            else
                return;

        }
    }
}
