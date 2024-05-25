using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/InstantItem")]
public class ItemData_InstantItem : ItemData
{
    public int instanceItemID;
    public ItemEffect[] itemEffects;

    public void ExcuteItemEffect()
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect();
        }
    }
}
