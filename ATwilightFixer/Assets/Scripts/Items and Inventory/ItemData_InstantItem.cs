using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/InstantItem")]
public class ItemData_InstantItem : ItemData
{
    public int instanceItemID;
    public int healValue = 50;

    public void ExcuteItemEffect()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerStats.TakeDamage(healValue);
    }
}
