using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ShopItem : MonoBehaviour
{
    [Header("Item data")]
    [SerializeField] private ItemData item;
    [SerializeField] private Sprite itemSprite;

    [Header("Content data")]
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private TextMeshProUGUI itemDescription;

    private void Awake()
    {
        SetItem();
    }

    private void OnValidate()
    {
        gameObject.name = "ShopItem_" + item.name;
    }

    public void BuyItem()
    {
        if (PlayerManager.instance.currency < item.itemPrice)
        {
            Debug.Log("You dont have Gold");
            AudioManager.instance.PlaySFX(5, null);
            return;
        }
        else
        {
            PlayerManager.instance.currency -= item.itemPrice;
            Inventory.Instance.AddItem(item);
            AudioManager.instance.PlaySFX(5, null);
        }
    }

    public void SetItem()
    {
        itemImage.sprite = itemSprite;
        //itemImage.sprite = item.icon;
        itemName.text = item.itemName;
        itemPrice.text = item.itemPrice.ToString() + "g";
        itemDescription.text = item.itemDescription;

    }
}
