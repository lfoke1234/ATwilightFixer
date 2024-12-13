using UnityEngine.EventSystems;

public class UI_QuickSlot : UI_ItemSlot
{
    public int quickSlotNumber;

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Useable usableItem = item.data as ItemData_Useable;

        if (usableItem != null)
        {
            Inventory.Instance.UnequipUsableItem(usableItem);
            Inventory.Instance.AddItemWithStack(usableItem, item.stackSize);
            ui.itemTooltip.HideToolTip();
        }
        else if (usableItem == null)
        {
            return;
        }
    }
}
