public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        //gameObject.name = "Equipment slot - " + slotType.ToString();
    }

    // public override void OnPointerDown(PointerEventData eventData)
    // {
    //     ItemData_Equipment equipment = item.data as ItemData_Equipment;
    // 
    //     if (equipment != null)
    //     {
    //         Inventory.Instance.UnequipItem(item.data as ItemData_Equipment);
    //         Inventory.Instance.AddItem(item.data as ItemData_Equipment);
    //         CleanUpSlot();
    //     }
    // }
}
