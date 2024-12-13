using System.Collections.Generic;
using UnityEngine;

// ����� ������ �����ϴ� ������
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
    // �������� Ÿ��
    public EquipmentType equipmentType;

    // �������� ��Ÿ�Ӱ� ȿ��
    public float itemCooldown;
    public ItemEffect[] itemEffects;

    // �߰����� �ɷ�ġ��
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

    // ������ ���ۿ� �ʿ��� ����
    [Header("Creaft requrements")]
    public List<InventoryItem> craftingMaterial;

    //
    private int descriptionLength;

    // ����� ������ ȿ��
    public void ExcuteItemEffect()
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect();
        }
    }

    // ��� ������ ���� �� �߰����� �ɷ�ġ�� �÷��̾�� �ο�
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

    // ��� ������ ���� �� �ο��� �ɷ�ġ�� ����
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

    // ������ ������ �����ϴ� �޼��� (���� �޼��� �����)
    public override string GetDescription()
    {
        // StringBuilder �ʱ�ȭ (���� ���� ����)
        sb.Length = 0;
        descriptionLength = 0;

        // �������� �� �Ӽ��� ���� �߰�
        AddItemDescription(damage, "���ݷ�");
        AddItemDescription(trueDamage, "���ݷ�");
        AddItemDescription(health, "ü��");
        AddItemDescription(recoveryStaminaSpeed, "���׹̳� ȸ��");
        AddItemDescription(armor, "����");

        // ������ 5�� �̸��� ��� �� ���� �߰��Ͽ� ���� ���̸� ����
        if (descriptionLength < 5)
        {
            for (int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        // ���������� ������ ���� ��ȯ
        return sb.ToString();
    }

    // ������ �Ӽ��� ���� �߰��ϴ� �޼���
    private void AddItemDescription(int _value, string _name)
    {
        // ���� 0�� �ƴ� ��쿡�� ���� �߰�
        if (_value != 0)
        {
            // StringBuilder�� �̹� ������ �ִ� ��� �� �ٲ� �߰�
            if (sb.Length > 0)
                sb.AppendLine();

            // ���� ����� ��� �Ӽ� ���� �߰�
            if (_value > 0)
                sb.Append("+ " + _value + " " + _name);

            // ���� �߰��� �Ӽ��� ���� ����
            descriptionLength++;
        }
    }
}
