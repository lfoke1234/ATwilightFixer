using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item Effect/Instant/Instant Heal effect")]
public class ItemEffect_InstantHeal : ItemEffect
{
    public int healPersent;

    public override void ExecuteEffect()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        int healAmount = (playerStats.GetMaxHealthValue() / 100) * healPersent;
        playerStats.IncreaseHealthBy(healAmount);
    }
}
