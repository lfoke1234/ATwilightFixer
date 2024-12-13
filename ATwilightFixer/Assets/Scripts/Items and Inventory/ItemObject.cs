using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    // 아이템의 외형 설정
    private void SetupVisual()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.itemName;
    }

    // 아이템 설정 및 초기 속도 지정
    public void SetUpItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisual();
    }

    // 아이템을 플레이어가 획득할 때 호출되는 함수
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
