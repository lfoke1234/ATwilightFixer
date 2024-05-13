using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private int defaultFontSize = 40;

    private string GetItemTypeText(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Material:
                return "재료";
            case ItemType.Equipment:
                return "장비";
            case ItemType.Useable:
                return "소모품";
            default:
                return " ";
        }
    }

    public void ShowToolTip(ItemData item)
    {
        if (item == null)
            return;

        itemNameText.text = item.itemName;
        itemTypeText.text = GetItemTypeText(item.itemType);
        itemDescription.text = item.GetDescription();

        if (itemNameText.text.Length > 12)
            itemNameText.fontSize = itemNameText.fontSize * 0.7f;
        else
            itemNameText.fontSize = defaultFontSize;

        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
