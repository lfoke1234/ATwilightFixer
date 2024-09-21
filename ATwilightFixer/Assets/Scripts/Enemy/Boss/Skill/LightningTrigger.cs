using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class LightningTrigger : MonoBehaviour
{
    private BoxCollider2D cd;
    private bool doDamaged;

    private void Awake()
    {
        cd = GetComponent<BoxCollider2D>();
    }

    private void AttackStart()
    {
        cd.enabled = true;
    }

    private void AttackEnd()
    {
        cd.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && doDamaged == false)
        {
            CharacterStats player = collision.GetComponent<CharacterStats>();

            int damage = (int)(player.GetMaxHealthValue() * 0.15f);

            player.TakeDamage(damage);

            doDamaged = true;
        }
    }
}
