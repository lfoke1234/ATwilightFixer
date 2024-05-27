using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum ItemType
{
    Material,
    Equipment,
    Useable,
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public int itemPrice;
    public string itemDescription;
    public string itemID;

    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemID = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public virtual string GetDescription()
    {
        // if (itemType == ItemType.Material)
        // {
        //     sb.Clear();
        //     sb.Append(itemDescription);
        //     return sb.ToString();
        // }
        // else
            return "";  
    }
}
