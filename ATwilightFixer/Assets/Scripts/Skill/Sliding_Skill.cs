using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Sliding_Skill : Skill
{
    Dash_Skill dash;

    private Coroutine boost1Coroutine;
    private Coroutine boost2Coroutine;
    private Coroutine boost3Coroutine;

    [Header("Sliding1")]
    public bool sliding1Unlocked;
    [SerializeField] private UI_SkillTreeSlot sliding1UnlockButton;
    [SerializeField] private int damage1;
    [SerializeField] private int timer1;

    [Header("Sliding2")]
    public bool sliding2Unlocked;
    [SerializeField] private UI_SkillTreeSlot sliding2UnlockButton;
    [SerializeField] private int damage2;
    [SerializeField] private int timer2;

    [Header("Sliding3")]
    public bool sliding3Unlocked;
    [SerializeField] private UI_SkillTreeSlot sliding3UnlockButton;
    [SerializeField] private int damage3;
    [SerializeField] private int timer3;

    public override void UseSkill()
    {
        base.UseSkill();

        if (sliding1Unlocked)
        {
            amount = 0;
        }
        else if (sliding2Unlocked)
        {
            amount = 20;
        }
        else if (sliding3Unlocked)
        {
            amount = 40;
        }

        player.stats.DecreaseStamianBy(amount);
    }

    protected override void Start()
    {
        base.Start();
        dash = GetComponent<Dash_Skill>();

        sliding1UnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSliding1);
        sliding2UnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSliding2);
        sliding3UnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSliding3);
    }

    #region Unlock 
    public int CheckCurrentDash()
    {
        if (sliding1Unlocked)
        {
            return 1;
        }
        else if (sliding2Unlocked)
        {
            return 2;
        }
        else if (sliding3Unlocked)
        {
            return 3;
        }

        return 0;
    }

    private void UnlockSliding1()
    {
        dash.dash1Unlocked = false;
        dash.dash2Unlocked = false;
        dash.dash3Unlocked = false;

        sliding1Unlocked = true;
        sliding2Unlocked = false;
        sliding3Unlocked = false;
    }

    private void UnlockSliding2()
    {
        dash.dash1Unlocked = false;
        dash.dash2Unlocked = false;
        dash.dash3Unlocked = false;

        sliding1Unlocked = false;
        sliding2Unlocked = true;
        sliding3Unlocked = false;
    }

    private void UnlockSliding3()
    {
        dash.dash1Unlocked = false;
        dash.dash2Unlocked = false;
        dash.dash3Unlocked = false;

        sliding1Unlocked = false;
        sliding2Unlocked = false;
        sliding3Unlocked = true;
    }
    #endregion

    public void Sliding1()
    {
        if (sliding1Unlocked)
        {
            if (boost1Coroutine != null)
            {
                StopCoroutine(boost1Coroutine);
                player.stats.damage.RemoveModifiers(damage1);
            }
            StartCoroutine(Boost1Corutine(damage1, timer1));
        }
    }

    public void Sliding2()
    {
        if (sliding2Unlocked)
        {
            if (boost2Coroutine != null)
            {
                StopCoroutine(boost2Coroutine);
                player.stats.damage.RemoveModifiers(damage2);
            }
            StartCoroutine(Boost2Corutine(damage2, timer2));
        }
    }

    public void Sliding3()
    {
        if (sliding3Unlocked)
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
