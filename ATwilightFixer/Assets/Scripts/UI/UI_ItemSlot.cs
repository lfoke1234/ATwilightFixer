using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    protected UI ui;
    public InventoryItem item;

    // 툴팁의 위치 조정과 관련된 변수
    [SerializeField] private float xLimit = 960;
    [SerializeField] private float yLimit = 540;
    [SerializeField] private float xOffset = 150; 
    [SerializeField] private float yOffset = 150; 

    private void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    // 슬롯을 업데이트하여 새로운 아이템을 배치
    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.icon; 

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    // 슬롯 초기화
    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    // 슬롯을 클릭했을 때 호출되는 메서드
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        // 마우스 우클릭으로 아이템 제거
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Inventory.Instance.RemoveItem(item.data);
            return;
        }

        // 장비 아이템의 경우 장착
        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.Instance.EquipItem(item.data);
            ui.itemTooltip.HideToolTip(); 
        }
        // 소모품일 경우 퀵슬롯에 장착
        else if (item.data.itemType == ItemType.Useable)
        {
            Inventory.Instance.EquipUsableItem(item.data);
            ui.itemTooltip.HideToolTip();
        }
    }

    // 마우스를 슬롯 위에 올렸을 때 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;

        Vector2 mousePosition = Input.mousePosition;

        float newXoffset = 0;
        float newYoffset = 0;

        // 마우스 위치에 따라 툴팁의 위치를 조정하여 화면을 벗어나지 않도록 조정
        if (mousePosition.x > xLimit)
            newXoffset = -xOffset;
        else
            newXoffset = xOffset;

        if (mousePosition.y > yLimit)
            newYoffset = -yOffset;
        else
            newYoffset = yOffset;

        ui.itemTooltip.ShowToolTip(item.data);
        ui.itemTooltip.transform.position = new Vector2(mousePosition.x + newXoffset, mousePosition.y + newYoffset);
    }

    // 마우스가 슬롯을 벗어났을 때 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
            return;

        ui.itemTooltip.HideToolTip();
    }
}

