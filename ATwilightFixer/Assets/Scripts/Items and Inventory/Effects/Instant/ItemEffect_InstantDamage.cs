using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item Effect/Instant/Instant Damage effect")]
public class ItemEffect_InstantDamage : ItemEffect
{
    public int damageValue = 50;

    public override void ExecuteEffect()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerStats.TakeDamage(damageValue);
    }
}
