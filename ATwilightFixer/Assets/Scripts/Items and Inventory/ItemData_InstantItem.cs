using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Instant")]
public class ItemData_InstantItem : ItemData
{
    public ItemEffect[] itemEffects;

    public void ExcuteItemEffect()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect();
        }
    }
}
