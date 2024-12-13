using System.Text;
using UnityEngine;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

// 아이템의 유형을 정의하는 열거형
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
    // 아이템의 기본 정보들
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public int itemPrice;
    public string itemDescription;
    public string itemID;

    // 아이템 드롭 확률
    [Range(0, 100)]
    public float dropChance;

    // 아이템 설명을 관리하기 위한 StringBuilder 객체
    protected StringBuilder sb = new StringBuilder();

    // 에디터에서 ScriptableObject의 변경 사항이 발생할 때 호출되는 메서드
    private void OnValidate()
    {
#if UNITY_EDITOR
        // 현재 ScriptableObject의 파일 경로를 가져와서 고유 ID로 설정
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
