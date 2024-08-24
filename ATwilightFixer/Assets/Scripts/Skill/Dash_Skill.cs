using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    Sliding_Skill sliding;

    private Coroutine boost1Coroutine;
    private Coroutine boost2Coroutine;
    private Coroutine boost3Coroutine;

    [Header("Dash1")]
    public bool dash1Unlocked;
    [SerializeField] private UI_SkillTreeSlot dash1UnlockButton;
    [SerializeField] private int damage1;
    [SerializeField] private int timer1;

    [Header("Dash2")]
    public bool dash2Unlocked;
    [SerializeField] private UI_SkillTreeSlot dash2UnlockButton;
    [SerializeField] private int damage2;
    [SerializeField] private int timer2;

    [Header("Dash3")]
    public bool dash3Unlocked;
    [SerializeField] private UI_SkillTreeSlot dash3UnlockButton;
    [SerializeField] private int damage3;
    [SerializeField] private int timer3;

    public override void UseSkill()
    {
        base.UseSkill();

        if (dash1Unlocked)
        {
            amount = 0;
        }
        else if (dash2Unlocked)
        {
            amount = 20;
        }
        else if (dash3Unlocked)
        {
            amount = 40;
        }

        player.stats.DecreaseStamianBy(amount);
    }

    protected override void Start()
    {
        base.Start();
        sliding = GetComponent<Sliding_Skill>();

        dash1UnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash1);
        dash2UnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash2);
        dash3UnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash3);
    }

    #region Unlock 
    public int CheckCurrentDash()
    {
        if(dash1Unlocked)
        {
            return 1;
        }
        else if(dash2Unlocked)
        {
            return 2;
        }
        else if(!dash3Unlocked)
        {
            return 3;
        }

        return 0;
    }

    private void UnlockDash1()
    {
        sliding.sliding1Unlocked = false;
        sliding.sliding2Unlocked = false;
        sliding.sliding3Unlocked = false;

        dash1Unlocked = true;
        dash2Unlocked = false;
        dash3Unlocked = false;
    }

    private void UnlockDash2()
    {
        sliding.sliding1Unlocked = false;
        sliding.sliding2Unlocked = false;
        sliding.sliding3Unlocked = false;

        dash1Unlocked = false;
        dash2Unlocked = true;
        dash3Unlocked = false;
    }

    private void UnlockDash3()
    {
        sliding.sliding1Unlocked = false;
        sliding.sliding2Unlocked = false;
        sliding.sliding3Unlocked = false;

        dash1Unlocked = false;
        dash2Unlocked = false;
        dash3Unlocked = true;
    }
    #endregion


    public void Dash1()
    {
        if (dash1Unlocked)
        {
            if (boost1Coroutine != null)
            {
                StopCoroutine(boost1Coroutine);
                player.stats.damage.RemoveModifiers(damage1);
            }
            StartCoroutine(Boost1Corutine(damage1, timer1));
        }
    }

    public void Dash2()
    {
        if (dash2Unlocked)
        {
            if (boost2Coroutine != null)
            {
                StopCoroutine(boost2Coroutine);
                player.stats.damage.RemoveModifiers(damage2);
            }
            StartCoroutine(Boost2Corutine(damage2, timer2));
        }
    }

    public void Dash3()
    {
        if (dash3Unlocked)
        {
            if (boost3Coroutine != null)
            {
                StopCoroutine(boost3Coroutine);
                player.stats.damage.RemoveModifiers(damage3);
            }
            StartCoroutine(Boost3Corutine(damage3, timer3));
        }
    }


    private IEnumerator Boost1Corutine(int damage, float timer)
    {
        player.stats.damage.AddModifiers(damage);
        yield return new WaitForSeconds(timer);
        player.stats.damage.RemoveModifiers(damage);
        boost1Coroutine = null;
    }

    private IEnumerator Boost2Corutine(int damage, float timer)
    {
        player.stats.damage.AddModifiers(damage);
        yield return new WaitForSeconds(timer);
        player.stats.damage.RemoveModifiers(damage);
        boost2Coroutine = null;
    }

    private IEnumerator Boost3Corutine(int damage, float timer)
    {
        player.stats.damage.AddModifiers(damage);
        yield return new WaitForSeconds(timer);
        player.stats.damage.RemoveModifiers(damage);
        boost3Coroutine = null;
    }
}
