using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainTrigger : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private bool doDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats player = collision.GetComponent<PlayerStats>();

        if (player != null && !doDamage)
        {
            int playerArmor = player.armor.GetValue();
            player.TakeDamage(damage - playerArmor);
            doDamage = true;
        }
    }

    public void ReturnDamageTrigger() => doDamage = true;

    public void SetChainDamage(int value) => damage = value;
}
