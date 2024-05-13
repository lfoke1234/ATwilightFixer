using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats, ISaveManager
{
    private Player player;

    [Header("Level details")]
    [SerializeField] private int level;
    [SerializeField] private int damageStatsModifireWithLevel;
    [SerializeField] private int maxHealthStatsModifireWithLevel;
    [SerializeField] private int armorStatsModifireWithLevel;

    private void Awake()
    {
        ApplyLevelModifires();
    }

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    #region Add modifire with level

    private void DamageModify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            int modifire = damageStatsModifireWithLevel;

            _stat.AddModifiers(modifire);
        }
    }

    private void HealthModify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            int modifire = maxHealthStatsModifireWithLevel;

            _stat.AddModifiers(modifire);
        }
    }

    private void ArmorModify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            int modifire = armorStatsModifireWithLevel;

            _stat.AddModifiers(modifire);
        }
    }
    private void ApplyLevelModifires()
    {
        DamageModify(damage);
        HealthModify(maxHealth);
        ArmorModify(armor);
    }

    #endregion

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        // player.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();

        player.Die();
    }

    public void ReturnDamage()
    {

    }

    public void LoadData(GameData _data)
    {

    }

    public void SaveData(ref GameData _data)
    {

    }
}
