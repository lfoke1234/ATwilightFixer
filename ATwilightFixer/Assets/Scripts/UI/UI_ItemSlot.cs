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

    // ������ ��ġ ������ ���õ� ����
    [SerializeField] private float xLimit = 960;
    [SerializeField] private float yLimit = 540;
    [SerializeField] private float xOffset = 150; 
    [SerializeField] private float yOffset = 150; 

    private void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    // ������ ������Ʈ�Ͽ� ���ο� �������� ��ġ
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

    // ���� �ʱ�ȭ
    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;
        itemText.text = "";
    }

    // ������ Ŭ������ �� ȣ��Ǵ� �޼���
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null)
            return;

        // ���콺 ��Ŭ������ ������ ����
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Inventory.Instance.RemoveItem(item.data);
            return;
        }

        // ��� �������� ��� ����
        if (item.data.itemType == ItemType.Equipment)
        {
            Inventory.Instance.EquipItem(item.data);
            ui.itemTooltip.HideToolTip(); 
        }
        // �Ҹ�ǰ�� ��� �����Կ� ����
        else if (item.data.itemType == ItemType.Useable)
        {
            Inventory.Instance.EquipUsableItem(item.data);
            ui.itemTooltip.HideToolTip();
        }
    }

    // ���콺�� ���� ���� �÷��� �� ȣ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;

        Vector2 mousePosition = Input.mousePosition;

        float newXoffset = 0;
        float newYoffset = 0;

        // ���콺 ��ġ�� ���� ������ ��ġ�� �����Ͽ� ȭ���� ����� �ʵ��� ����
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

    // ���콺�� ������ ����� �� ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
            return;

        ui.itemTooltip.HideToolTip();
    }
}

