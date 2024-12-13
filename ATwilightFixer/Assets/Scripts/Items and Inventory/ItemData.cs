using System.Text;
using UnityEngine;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

// �������� ������ �����ϴ� ������
public enum ItemType
{
    Material,
    Equipment,
    Useable,
    Instant,
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    // �������� �⺻ ������
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public int itemPrice;
    public string itemDescription;
    public string itemID;

    // ������ ��� Ȯ��
    [Range(0, 100)]
    public float dropChance;

    // ������ ������ �����ϱ� ���� StringBuilder ��ü
    protected StringBuilder sb = new StringBuilder();

    // �����Ϳ��� ScriptableObject�� ���� ������ �߻��� �� ȣ��Ǵ� �޼���
    private void OnValidate()
    {
#if UNITY_EDITOR
        // ���� ScriptableObject�� ���� ��θ� �����ͼ� ���� ID�� ����
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
