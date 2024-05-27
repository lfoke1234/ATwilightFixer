using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;


    private void OnValidate()
    {
        SetupVisual();
    }

    private void SetupVisual()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.itemName;
    }

    public void SetUpItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisual();
    }

    public void PickupItem()
    {
        if (itemData is ItemData_InstantItem instantItem)
        {
            instantItem.ExcuteItemEffect();
            Destroy(gameObject);
            return;
        }

        if (Inventory.Instance.CanAddtoInventory() == false && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 7);
            return;
        }

        AudioManager.instance.PlaySFX(6, null);
        Inventory.Instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
