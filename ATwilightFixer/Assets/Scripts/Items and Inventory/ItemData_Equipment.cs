using System.Collections.Generic;
using UnityEngine;

// 장비의 부위를 정의하는 열거형
public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Shose,
    Flask,
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    // 아이템의 타입
    public EquipmentType equipmentType;

    // 아이템의 쿨타임과 효과
    public float itemCooldown;
    public ItemEffect[] itemEffects;

    // 추가적인 능력치들
    #region Stat
    [Header("Major Stat")]
    public int strength; 
    public int agility;
    public int intelligence;
    public int vitality;
    public int recoveryStaminaSpeed;

    [Header("Offensive Stats")]
    public int damage;
    public int trueDamage;
    public int critChacne;
    public int critPower;

    [Header("Defencive stats")]
    public int health;
    public int stamina;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;
    #endregion

    // 아이템 제작에 필요한 재료들
    [Header("Creaft requrements")]
    public List<InventoryItem> craftingMaterial;

    //
    private int descriptionLength;

    // 장비의 고유한 효과
    public void ExcuteItemEffect()
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect();
        }
    }

    // 장비 아이템 장착 시 추가적인 능력치를 플레이어에게 부여
    public void AddModifire()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifiers(strength);
        playerStats.agility.AddModifiers(agility);
        playerStats.intelligence.AddModifiers(intelligence);
        playerStats.vitality.AddModifiers(vitality);
        playerStats.recoveryStaminaSpeed.AddModifiers(recoveryStaminaSpeed);

        playerStats.damage.AddModifiers(damage);
        playerStats.trueDamage.AddModifiers(trueDamage);
        playerStats.critChacne.AddModifiers(critChacne);
        playerStats.critPower.AddModifiers(critPower);

        playerStats.maxHealth.AddModifiers(health);
        playerStats.maxStamina.AddModifiers(stamina);
        playerStats.armor.AddModifiers(armor);
        playerStats.evasion.AddModifiers(evasion);
        playerStats.magicResistance.AddModifiers(magicResistance);

        playerStats.posionDamage.AddModifiers(fireDamage);
        playerStats.iceDamage.AddModifiers(iceDamage);
        playerStats.lightingDamage.AddModifiers(lightingDamage);
    }

    // 장비 아이템 해제 시 부여한 능력치를 제거
    public void RemoveModifire() 
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifiers(strength);
        playerStats.agility.RemoveModifiers(agility);
        playerStats.intelligence.RemoveModifiers(intelligence);
        playerStats.vitality.RemoveModifiers(vitality);
        playerStats.recoveryStaminaSpeed.RemoveModifiers(recoveryStaminaSpeed);

        playerStats.damage.RemoveModifiers(damage);
        playerStats.trueDamage.RemoveModifiers(trueDamage);
        playerStats.critChacne.RemoveModifiers(critChacne);
        playerStats.critPower.RemoveModifiers(critPower);

        playerStats.maxHealth.RemoveModifiers(health);
        playerStats.maxStamina.RemoveModifiers(stamina);
        playerStats.armor.RemoveModifiers(armor);
        playerStats.evasion.RemoveModifiers(evasion);
        playerStats.magicResistance.RemoveModifiers(magicResistance);

        playerStats.posionDamage.RemoveModifiers(fireDamage);
        playerStats.iceDamage.RemoveModifiers(iceDamage);
        playerStats.lightingDamage.RemoveModifiers(lightingDamage);
    }

    // 아이템 설명을 생성하는 메서드 (기존 메서드 덮어쓰기)
    public override string GetDescription()
    {
        // StringBuilder 초기화 (기존 내용 삭제)
        sb.Length = 0;
        descriptionLength = 0;

        // 아이템의 각 속성을 설명에 추가
        AddItemDescription(damage, "공격력");
        AddItemDescription(trueDamage, "공격력");
        AddItemDescription(health, "체력");
        AddItemDescription(recoveryStaminaSpeed, "스테미나 회복");
        AddItemDescription(armor, "방어력");

        // 설명이 5줄 미만인 경우 빈 줄을 추가하여 설명 길이를 맞춤
        if (descriptionLength < 5)
        {
            for (int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        // 최종적으로 생성된 설명 반환
        return sb.ToString();
    }

    // 아이템 속성을 설명에 추가하는 메서드
    private void AddItemDescription(int _value, string _name)
    {
        // 값이 0이 아닌 경우에만 설명 추가
        if (_value != 0)
        {
            // StringBuilder에 이미 내용이 있는 경우 줄 바꿈 추가
            if (sb.Length > 0)
                sb.AppendLine();

            // 값이 양수일 경우 속성 설명 추가
            if (_value > 0)
                sb.Append("+ " + _value + " " + _name);

            // 설명에 추가된 속성의 개수 증가
            descriptionLength++;
        }
    }
}
