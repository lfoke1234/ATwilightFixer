using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    // �������� ���� ����
    private void SetupVisual()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.itemName;
    }

    // ������ ���� �� �ʱ� �ӵ� ����
    public void SetUpItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisual();
    }

    // �������� �÷��̾ ȹ���� �� ȣ��Ǵ� �Լ�
    public void PickupItem()
    {
        if (itemData is ItemData_InstantItem instantItem)
        {
            instantItem.ExcuteItemEffect();
            Destroy(gameObject);
            return;
        }

        Inventory.Instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
