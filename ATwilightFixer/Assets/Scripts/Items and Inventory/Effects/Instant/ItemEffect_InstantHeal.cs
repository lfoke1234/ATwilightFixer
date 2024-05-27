using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item Effect/Instant/Instant Heal effect")]
public class ItemEffect_InstantHeal : ItemEffect
{
    public int healValue = 50;

    public override void ExecuteEffect()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerStats.IncreaseHealthBy(healValue);
    }
}
