using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Passive_Skill : Skill
{
    private bool atkUnlock;
    private bool defUnlock;
    private bool speedUnlock;
    private bool allUnlock;
    private bool atkdefUnlock;
    private bool defatkUnlock;

    [SerializeField] private UI_SkillTreeSlot atkButton;
    [SerializeField] private UI_SkillTreeSlot defButton;
    [SerializeField] private UI_SkillTreeSlot speedButton;
    [SerializeField] private UI_SkillTreeSlot allButton;
    [SerializeField] private UI_SkillTreeSlot atkdefButton;
    [SerializeField] private UI_SkillTreeSlot defatkButton;

    private int atkBuff;
    private int defBuff;

    private int allAtk; 
    private int allDef;

    private int atkdef1;
    private int atkdef2;

    private int defatk1;
    private int defatk2;

    protected override void Start()
    {
        base.Start();

        atkButton.GetComponent<Button>().onClick.AddListener(AtkBuff);
        defButton.GetComponent<Button>().onClick.AddListener(DefBuff);
        speedButton.GetComponent<Button>().onClick.AddListener(SpeedBuff);
        allButton.GetComponent<Button>().onClick.AddListener(AllBuff);
        atkdefButton.GetComponent<Button>().onClick.AddListener(AtkDefBuff);
        defatkButton.GetComponent<Button>().onClick.AddListener(DefAtkBuff);
    }


    public override void UseSkill()
    {
        base.UseSkill();
    }

    private void CheckPassive()
    {
        AtkBuff();
        DefBuff();
        SpeedBuff();
        AllBuff();
        AtkDefBuff();
        DefAtkBuff();
    }

    private void AtkBuff()
    {

        if (atkUnlock)
        {
            atkBuff = (int)(player.stats.damage.GetValue() * 0.15f);
            player.stats.damage.AddModifiers(atkBuff);
        }
        else
        {
            player.stats.damage.RemoveModifiers(atkBuff);
        }
    }

    private void DefBuff()
    {
        if (defatkUnlock)
        {
            defBuff = (int)(player.stats.armor.GetValue() * 0.15f);
            player.stats.armor.AddModifiers(defBuff);
        }
        else
        {
            player.stats.armor.RemoveModifiers(defBuff);
        }
    }

    private void SpeedBuff()
    {
        if (speedUnlock)
            player.moveSpeed = 10f;
        else
            player.moveSpeed = 8f;
    }

    private void AllBuff()
    {
        if (allUnlock)
        {
            allAtk = (int)(player.stats.damage.GetValue() * 0.05f);
            allDef = (int)(player.stats.armor.GetValue() * 0.05f);
            player.stats.damage.AddModifiers(allAtk);
            player.stats.armor.AddModifiers(allDef);
        }
        else
        {
            player.stats.damage.RemoveModifiers(allAtk);
            player.stats.armor.RemoveModifiers(allDef);
        }
    }

    private void AtkDefBuff()
    {
        if (atkdefUnlock)
        { 
            allAtk = (int)(player.stats.damage.GetValue() * 0.25f);
            allDef = (int)(player.stats.armor.GetValue() * 0.10f);
            player.stats.damage.AddModifiers(allAtk);
            player.stats.armor.AddModifiers(allDef);
        }
        else
        {
            player.stats.damage.RemoveModifiers(allAtk);
            player.stats.armor.RemoveModifiers(allDef);
        }
    }

    private void DefAtkBuff()
    {
        if(defatkUnlock)
        {

        }
        else
        {

        }
    }
}
