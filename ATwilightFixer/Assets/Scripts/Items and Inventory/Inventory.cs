using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory Instance;

    public List<ItemData> startingItems;
    public List<ItemData> startEquipItemData;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem> usable;
    public Dictionary<ItemData_Useable, InventoryItem> usableDictionary;


    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform EquipmentSlotParnet;
    [SerializeField] private Transform statSlotParnet;
    [SerializeField] private Transform quickSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;
    private UI_QuickSlot[] quickSlot;

    [Header("Items Cooldown")]
    private float lastTimeUsedFalsk;
    private float flaskCooldown;

    private float lastTimeUseUsableItem;
    public float usableItemCooldown { get; private set; }


    [Header("Data base")]
    public List<ItemData> itemDataBase;
    public List<InventoryItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipment;
    public List<InventoryItem> loadUsable;
    #region EquipmentItem
    public ItemData_Equipment level1Sword;
    public ItemData_Equipment level2Sword;
    public ItemData_Equipment level3Sword;
    public ItemData_Equipment level4Sword;
    public ItemData_Equipment level1Armor;
    public ItemData_Equipment level2Armor;
    public ItemData_Equipment level3Armor;
    public ItemData_Equipment level4Armor;
    public ItemData_Equipment level1Shoose;
    public ItemData_Equipment level2Shoose;
    public ItemData_Equipment level3Shoose;
    public ItemData_Equipment level4Shoose;
    public ItemData_Equipment level1Gloves;
    public ItemData_Equipment level2Gloves;
    public ItemData_Equipment level3Gloves;
    public ItemData_Equipment level4Gloves;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this);

        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        usable = new List<InventoryItem>();
        usableDictionary = new Dictionary<ItemData_Useable, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = EquipmentSlotParnet.GetComponentsInChildren<UI_EquipmentSlot>();
        quickSlot = quickSlotParent.GetComponentsInChildren<UI_QuickSlot>();

        statSlot = statSlotParnet.GetComponentsInChildren<UI_StatSlot>();

        StartingItme();
        StartEquipItem();

    }

    private void StartingItme()
    {
        foreach (ItemData_Equipment item in loadedEquipment)
        {
            EquipItem(item);
        }

        if (loadedItems.Count > 0)
        {
            foreach (InventoryItem item in loadedItems)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
        }

        if (loadUsable.Count > 0)
        {
            foreach (InventoryItem item in loadUsable)
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddQuickSlot(item.data);
                }
            }
        }

        for (int i = 0; i < startingItems.Count; i++)
        {
            if (startingItems[i] != null)
                AddItem(startingItems[i]);
        }
    }

    private void StartEquipItem()
    {
        for (int i = 0; i < startEquipItemData.Count; i++)
        {
            EquipItem(startEquipItemData[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquiment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquiment);

        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquiment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            RemoveItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquiment, newItem);

        newEquiment.AddModifire();

        RemoveItem(_item);

        UpdateSlotUI();
    }

    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifire();
        }
    }

    public void EquipUsableItem(ItemData _item)
    {
        ItemData_Useable newUsable = _item as ItemData_Useable;

        if (usableDictionary.TryGetValue(newUsable, out InventoryItem existingItem))
        {
            existingItem.stackSize += stashDictionary[_item].stackSize;
        }
        else
        {
            InventoryItem newItem = new InventoryItem(newUsable);
            newItem.stackSize = stashDictionary[_item].stackSize;
            usable.Add(newItem);
            usableDictionary.Add(newUsable, newItem);
        }

        RemoveItem(_item);
        UpdateSlotUI();
    }
    
    #region EquipItem

    public void EquipLevel1Sword()
    {
        EquipItem(level1Sword);
    }
    public void EquipLevel2Sword()
    {
        EquipItem(level2Sword);
    }
    public void EquipLevel3Sword()
    {
        EquipItem(level3Sword);
    }
    public void EquipLevel4Sword()
    {
        EquipItem(level4Sword);
    }

    public void EquipLevel1Gloves()
    {
        EquipItem(level1Gloves);
    }
    public void EquipLevel2Gloves()
    {
        EquipItem(level2Gloves);
    }
    public void EquipLevel3Gloves()
    {
        EquipItem(level3Gloves);
    }
    public void EquipLevel4Gloves()
    {
        EquipItem(level4Gloves);
    }

    public void EquipLevel1Shoose()
    {
        EquipItem(level1Shoose);
    }
    public void EquipLevel2Shoose()
    {
        EquipItem(level2Shoose);
    }
    public void EquipLevel3Shoose()
    {
        EquipItem(level3Shoose);
    }
    public void EquipLevel4Shoose()
    {
        EquipItem(level4Shoose);
    }

    public void EquipLevel1Armor()
    {
        EquipItem(level1Armor);
    }
    public void EquipLevel2Armor()
    {
        EquipItem(level2Armor);
    }
    public void EquipLevel3Armor()
    {
        EquipItem(level3Armor);
    }
    public void EquipLevel4Armor()
    {
        EquipItem(level4Armor);
    }

    #endregion

    public void AddItemWithStack(ItemData _item, int stackSizeToAdd)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem existingItem))
        {
            existingItem.stackSize += stackSizeToAdd;
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            newItem.stackSize = stackSizeToAdd;
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }

        UpdateSlotUI();
    }

    public void UnequipUsableItem(ItemData_Useable itemToRemove)
    {
        if (usableDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            usable.Remove(value);
            usableDictionary.Remove(itemToRemove);
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);
                }
            }
        }

        for (int i = 0; i < quickSlot.Length; i++)
        {
            quickSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        for (int i = 0; i < usable.Count; i++)
        {
            quickSlot[i].UpdateSlot(usable[i]);
        }

        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddtoInventory())
        {
            AddToInventory(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            AddStash(_item);
        }
        else if (_item.itemType == ItemType.Useable)
        {
            if (usableDictionary.TryGetValue((ItemData_Useable)_item, out InventoryItem value))
            {
                value.AddStack();
            }
            else
            {
                AddStash(_item);
            }
        }

        UpdateSlotUI();
    }

    private void AddStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddQuickSlot(ItemData _item)
    {
        if (usableDictionary.TryGetValue((ItemData_Useable)_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            usable.Add(newItem);
            usableDictionary.Add((ItemData_Useable)_item, newItem);
        }

        UpdateSlotUI();
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    private void AddItemToStash(InventoryItem itemToLoad)
    {
        if (stashDictionary.TryGetValue(itemToLoad.data, out InventoryItem existingItem))
        {
            existingItem.stackSize += itemToLoad.stackSize;
        }
        else
        {
            stash.Add(itemToLoad);
            stashDictionary.Add(itemToLoad.data, itemToLoad);
        }

        UpdateSlotUI();
    }

    private void AddItemToUsable(InventoryItem itemToLoad)
    {
        if (usableDictionary.TryGetValue((ItemData_Useable)itemToLoad.data, out InventoryItem existingItem))
        {
            existingItem.stackSize += itemToLoad.stackSize;
        }
        else
        {
            usable.Add(itemToLoad);
            usableDictionary.Add((ItemData_Useable)itemToLoad.data, itemToLoad);
        }

        UpdateSlotUI();
    }


    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }


        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            stash.Remove(stashValue);
            stashDictionary.Remove(_item);
        }

        UpdateSlotUI();
    }

    public void RemoveUsableItem(ItemData_Useable _item)
    {
        if (usableDictionary.TryGetValue(_item, out InventoryItem usableValue))
        {
            if (usableValue.stackSize <= 1)
            {
                usable.Remove(usableValue);
                usableDictionary.Remove(_item);
            }
            else
            {
                usableValue.RemoveStack();
            }
        }

        UpdateSlotUI();
    }

    public bool CanAddtoInventory()
    {
        if (inventory.Count >= inventoryItemSlot.Length)
        {
            return false;
        }

        return true;
    }

    public bool CanCreaft(ItemData_Equipment _itemToCreaft, List<InventoryItem> _requiredMaterial)
    {
        List<InventoryItem> materialToRemove = new List<InventoryItem>();


        for (int i = 0; i < _requiredMaterial.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requiredMaterial[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _requiredMaterial[i].stackSize)
                {
                    Debug.Log("You dont have Materials");
                    return false;
                }
                else
                {
                    materialToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("You dont have Materials");
                return false;
            }
        }

        for (int i = 0; i < materialToRemove.Count; i++)
        {
            RemoveItem(materialToRemove[i].data);
        }

        AddItem(_itemToCreaft);
        Debug.Log("Success Craft Item : " + _itemToCreaft.name);
        return true;
    }

    

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
            {
                equipedItem = item.Key;
            }
        }

        return equipedItem;
    }

    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
            return;

        bool canUseFlaks = Time.time > lastTimeUsedFalsk + flaskCooldown;

        if (canUseFlaks)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.ExcuteItemEffect();
            lastTimeUsedFalsk = Time.time;
        }
    }

    public void UseQuickSlot(int quickSlotNumber)
    {
        UI_QuickSlot quickSlot = FindQuickSlotByNumber(quickSlotNumber);

        bool canUseItem = Time.time > lastTimeUseUsableItem + usableItemCooldown;

        if (canUseItem)
        {
            if (quickSlot != null && quickSlot.item != null)
            {
                ItemData_Useable usableItem = quickSlot.item.data as ItemData_Useable;

                if (usableItem != null)
                {
                    usableItemCooldown = usableItem.itemCooldown;
                    usableItem.ExcuteItemEffect();
                    RemoveUsableItem(usableItem);
                    lastTimeUseUsableItem = Time.time;
                }
            }
        }
    }

    private UI_QuickSlot FindQuickSlotByNumber(int quickSlotNumber)
    {
        UI_QuickSlot[] quickSlots = quickSlotParent.GetComponentsInChildren<UI_QuickSlot>();
        foreach (var quickSlot in quickSlots)
        {
            if (quickSlot.quickSlotNumber == quickSlotNumber)
            {
                return quickSlot;
            }
        }
        return null;
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && item.itemID == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (string loadedItemID in _data.equipmentID)
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && loadedItemID== item.itemID)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }

        foreach (KeyValuePair<string, int> pair in _data.quickSlot)
        {
            foreach (var item in itemDataBase)
            {
                if (item != null && item.itemID == pair.Key && item is ItemData_Useable)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadUsable.Add(itemToLoad);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentID.Clear();

        //foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        //{
        //    _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        //}

        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemID, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            _data.equipmentID.Add(item.Key.itemID);
        }

        foreach (InventoryItem item in usable)
        {
            if (_data.quickSlot.ContainsKey(item.data.itemID))
            {
                _data.quickSlot[item.data.itemID] = item.stackSize;
            }
            else
            {
                _data.quickSlot.Add(item.data.itemID, item.stackSize);
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Fill up item data base")]
    private void FillUpItemDataBase() => itemDataBase = new List<ItemData>(GetItemDataBase());

    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" }); 

        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
#endif
}
