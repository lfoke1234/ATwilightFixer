using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum StatType
{
    health,
    stamina,
    staminaRecoverSpeed,
    damage,
    armor,
    TrueDamage,
}


public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major Stat")]
    public Stat strength; // 피해량, 치명타 피해 증가
    public Stat agility;  // 회피, 치명타 확률 증가
    public Stat intelligence; // 마법뎀, 저항 증가
    public Stat vitality; // 체력 증가

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat trueDamage;
    public Stat recoveryStaminaSpeed;
    public Stat critChacne;
    public Stat critPower; // default 150

    [Header("Defencive stats")]
    public Stat maxHealth;
    public Stat maxHiddenHealth;
    public Stat maxStamina;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat posionDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isPoisoned; // 틱뎀
    public bool isChilled; // 적 빙결, 방감
    public bool isShocked; // 명중 감소

    public float poisonedTimer;
    public float chilledTimer;
    public float shockedTimer;

    private float poisonDamageCoolDown = 0.3f;
    private float posionDamageTimer;
    private int poisonDamage;

    public int currentHealth;
    public int currentHiddenHealth;
    public float currentStamina;

    public System.Action onHealthChanged;
    public System.Action onStaminaChanged;
    public bool isDead;
    public bool returnDamage;

    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();
        currentHiddenHealth = GetHiddenHealthValue();
        currentStamina = GetMaxStaminaValue();

        fx = GetComponent<EntityFX>();
    }
    
    protected virtual void Update()
    {
        poisonedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        posionDamageTimer -= Time.deltaTime;

        if (poisonedTimer < 0)
            isPoisoned = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        if (isPoisoned)
            ApplyPoisonDamage();

        RecoveryStamina();
    }

    public void RecoveryStamina()
    {
        if(currentStamina < GetMaxStaminaValue())
            currentStamina += (Time.deltaTime * recoveryStaminaSpeed.GetValue());

        if(currentStamina > GetMaxStaminaValue())
        {
            currentStamina = GetMaxStaminaValue();
        }

        if (onStaminaChanged != null)
            onStaminaChanged();
    }

    public void RecoveryStaminaBy(int _value)
    {
        if (currentStamina < GetMaxStaminaValue())
            currentStamina += _value;

        if (currentStamina > GetMaxStaminaValue())
        {
            currentStamina = GetMaxStaminaValue();
        }

        if (onStaminaChanged != null)
            onStaminaChanged();
    }

    public void DecreaseStamianBy(float _stamina)
    {
        currentStamina -= _stamina;

        if(onStaminaChanged != null)
        {
            onStaminaChanged();
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (_targetStats.isDead)
            return;

        // 회피할경우 return
        //if (TargetCanAvoidAttack(_targetStats)) return;
        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        // 시전자의 공격력 적용
        int totalDamage = damage.GetValue();

        // 크리티컬일 경우 크리티컬 피해 적용
        // if (CanCrit())
        // {
        //     totalDamage = CalculateCriticalDamage(totalDamage);
        // }

        // 타겟의 아머 체크후 피해 경감
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        // 시각효과
        fx.CreatHitFX(_targetStats.transform, false);

        _targetStats.TakeDamage(totalDamage);

        // 마법 데미지일 경우 상태이상 추가
        DoMagicalDamage(_targetStats);
    }

    public virtual void DoDamageWithValue(CharacterStats _targetStats, int plusValue)
    {
        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue() + plusValue;

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(_targetStats);
    }

    public virtual void DoTrueDamage(CharacterStats _targetStats)
    {
        int totalDamage = trueDamage.GetValue();

        _targetStats.TakeTrueDamage(totalDamage);
    }

    public virtual void DoTrueDamage(CharacterStats _targetStats, int value)
    {
        _targetStats.TakeTrueDamage(value);
    }

    #region MagicalDamage
    // 마뎀 to ApplyAilments
    public virtual void DoMagicalDamage(CharacterStats _targetStatas)
    {
        // 대상에게 마법 피해를 적용하는 함수

        // 각 마법 피해량을 가져옴 (독, 얼음, 번개)
        int _poisonDamage = posionDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        // 총 마법 피해량 계산 (독 피해 + 얼음 피해 + 번개 피해 + 지능 스탯의 추가 피해)
        int totalMagicalDamage = _poisonDamage + _iceDamage + _lightingDamage + intelligence.GetValue();

        // 대상의 마법 저항을 체크하여 최종 피해량을 결정
        totalMagicalDamage = CheckTargetResustance(_targetStatas, totalMagicalDamage);
        _targetStatas.TakeDamage(totalMagicalDamage);

        // 모든 마법 피해가 0 이하인 경우 상태 이상을 적용하지 않음
        if (Mathf.Max(_poisonDamage, _iceDamage, _lightingDamage) <= 0)
        {
            return;
        }

        // 각 상태 이상(독, 빙결, 감전) 적용 여부를 확인
        bool canApplyPoison = _poisonDamage > _iceDamage && _poisonDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _poisonDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _poisonDamage && _lightingDamage > _iceDamage;

        // 상태 이상 적용 시도
        AttemptyToApplyAilments(_targetStatas, _poisonDamage, _iceDamage, _lightingDamage,
                                 ref canApplyPoison, ref canApplyChill, ref canApplyShock);
    }

    private void AttemptyToApplyAilments(CharacterStats _targetStatas, int _poisonDamage, int _iceDamage, int _lightingDamage, ref bool canApplyPoison, ref bool canApplyChill, ref bool canApplyShock)
    {
        // 대상이 이미 죽어있는 경우 상태 이상 적용을 중지
        if (_targetStatas.isDead)
            return;

        // 세 가지 상태 이상 모두 적용되지 않은 경우에만 반복
        while (!canApplyPoison && !canApplyChill && !canApplyShock)
        {
            // 50% 확률로 독 상태 적용
            if (Random.value < 0.5f && _poisonDamage > 0)
            {
                canApplyPoison = true;
                _targetStatas.ApplyAilments(canApplyPoison, canApplyChill, canApplyShock);
                return;
            }

            // 50% 확률로 빙결 상태 적용
            if (Random.value < 0.5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStatas.ApplyAilments(canApplyPoison, canApplyChill, canApplyShock);
                return;
            }

            // 50% 확률로 감전 상태 적용
            if (Random.value < 0.5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStatas.ApplyAilments(canApplyPoison, canApplyChill, canApplyShock);
                return;
            }
        }

        // 독 상태가 적용된 경우, 추가적인 독 피해 설정
        if (canApplyPoison)
            _targetStatas.SetupPoisonDamage(Mathf.RoundToInt(_poisonDamage * 0.2f));

        // 상태 이상 적용
        _targetStatas.ApplyAilments(canApplyPoison, canApplyChill, canApplyShock);
    }

    public void ApplyAilments(bool _poison, bool _chill, bool _shock)
    {
        // 이미 상태 이상이 적용된 경우 중복 적용 방지
        if (isPoisoned || isChilled || isShocked)
            return;

        // 독 상태 적용
        if (_poison)
        {
            poisonedTimer = 2f;
            isPoisoned = _poison;
            fx.poisonFxFor(poisonedTimer);
        }

        // 빙결 상태 적용
        if (_chill)
        {
            chilledTimer = 2f;
            isChilled = _chill;

            // 적이 빙결된 동안 시간 동결
            Enemy enemy = GetComponent<Enemy>();
            enemy.StartCoroutine("FreezeTimeFor", chilledTimer);
            fx.ChillFxFor(chilledTimer);
        }

        // 감전 상태 적용
        if (_shock)
        {
            shockedTimer = 2f;
            isShocked = _shock;
            fx.ShockFxFor(shockedTimer);
        }
    }

    private void ApplyPoisonDamage()
    {
        if (posionDamageTimer < 0)
        {
            DecreaseHealthBy(poisonDamage);

            if (currentHealth <= 0 && !isDead)
                Die();

            posionDamageTimer = poisonDamageCoolDown;
        }
    }

    public void SetupPoisonDamage(int _damage) => poisonDamage = _damage;
    #endregion


    public virtual void TakeDamage(int _damage)
    {
        if (returnDamage)
            return;

        // DoDamage에서 적용된 최종 데미지 적용
        DecreaseHealthBy(_damage);
        
        Entity target = GetComponent<Entity>();
        Enemy enemy = GetComponent<Enemy>();

        // 적이 죽어있는경유 return
        if (target.stats.isDead == false)
        {
            if (target.GetComponent<Enemy>() == true)
                enemy.CanBeStunned();

            target.DamageEffect();
            target.fx.StartCoroutine("FlashFX");
        }

        // 체력이 없을경우 각 Entity에 Die() 호출
        if (currentHealth <= 0 && !isDead)
            Die();
    }


    protected virtual void TakeTrueDamage(int _damage)
    {
        DecreaseHiddenHealthBy(_damage);

        Entity target = GetComponent<Entity>();

        if (currentHiddenHealth <= 0 && currentHealth <= 0 && !isDead)
            Die();
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        if(onHealthChanged != null)
            onHealthChanged();
    }

    public virtual void DecreaseHiddenHealthBy(int _damage)
    {
        currentHiddenHealth -= _damage;

        if (currentHiddenHealth > GetHiddenHealthValue())
            currentHiddenHealth = GetHiddenHealthValue();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        // 데미지 팝업 이펙트 생성
        if (_damage > 0)
            fx.CreateDamagePopUpText(_damage.ToString());

        // 구독 메서드에게 알림 전달
        if (onHealthChanged != null)
            onHealthChanged();
    }

    #region Defend Check
    // 총 체력 반환
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    public float GetMaxStaminaValue()
    {
        return maxStamina.GetValue();
    }

    public int GetHiddenHealthValue()
    {
        return maxHiddenHealth.GetValue();
    }

    // 회피기능
    private bool TargetCanAvoidAttack(CharacterStats _targetStats) 
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    // 총데미지 - 방어력, 최솟값 0
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if(_targetStats == null)
        {
            return 0;
        }

        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        else
            totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    // 마뎀감
    private int CheckTargetResustance(CharacterStats _targetStatas, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStatas.magicResistance.GetValue() + (_targetStatas.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    
    #endregion


    #region Check Critical
    // 치확
    private bool CanCrit()
    {
        int totalCriticalChance = critChacne.GetValue() + agility.GetValue();

        if(Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    // 치피 계산
    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }
    #endregion


    protected virtual void Die()
    {
        isDead = true;
    }

    public void KillEntity()
    {
        if (!isDead)
            Die();
    }

    public void ReturnDamage(bool _returnDamage) => returnDamage = _returnDamage;

    public Stat GetStat(StatType statType)
    {
        if (statType == StatType.health) return maxHealth;
        else if (statType == StatType.stamina) return maxStamina;
        else if (statType == StatType.staminaRecoverSpeed) return recoveryStaminaSpeed;
        else if (statType == StatType.damage) return damage;
        else if (statType == StatType.armor) return armor;
        else if (statType == StatType.TrueDamage) return trueDamage;

        return null;
    }
}
