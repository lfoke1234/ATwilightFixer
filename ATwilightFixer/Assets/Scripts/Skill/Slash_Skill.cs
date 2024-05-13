using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slash_Skill : Skill
{
    public Slash_Skill_Controller slashController;

    public int slashDoDamage;

    [Header("First Slash")]
    public bool firstSlashUnlocked;
    [SerializeField] private UI_SkillTreeSlot firstSlashButton;
    [SerializeField] private int damage1 = 10;

    [Header("Second Slash")]
    public bool secondSlashUnlocked;
    [SerializeField] private UI_SkillTreeSlot secondSlashButton;
    [SerializeField] private int damage2 = 20;

    [Header("Third Slash")]
    public bool thirdSlashUnlocked;
    [SerializeField] private UI_SkillTreeSlot thirdSlashButton;
    [SerializeField] private int damage3 = 30;

    protected override void Start()
    {
        base.Start();

        firstSlashButton.GetComponent<Button>().onClick.AddListener(unlockFirstSlash);
        secondSlashButton.GetComponent<Button>().onClick.AddListener(unlockSecondSlash);
        thirdSlashButton.GetComponent<Button>().onClick.AddListener(unlockThirdSlash);
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (firstSlashUnlocked)
        {
            amount = 10;
            slashDoDamage = damage1;
        }
        else if (secondSlashUnlocked)
        {
            amount = 20;
            slashDoDamage = damage2;
        }
        else if (thirdSlashUnlocked)
        {
            amount = 40;
            slashDoDamage = damage3;
        }

        player.stats.DecreaseStamianBy(amount);
    }

    #region unlock
    public int CheckCurrentSlash()
    {
        if (firstSlashUnlocked)
        {
            return 1;
        }
        else if (secondSlashUnlocked)
        {
            return 2;
        }
        else if (thirdSlashUnlocked)
        {
            return 3;
        }

        return 0;
    }

    private void unlockFirstSlash()
    {
        firstSlashUnlocked = true;
        secondSlashUnlocked = false;
        thirdSlashUnlocked = false;
    }

    private void unlockSecondSlash()
    {
        firstSlashUnlocked = false;
        secondSlashUnlocked = true;
        thirdSlashUnlocked = false;
    }

    private void unlockThirdSlash()
    {
        firstSlashUnlocked = false;
        secondSlashUnlocked = false;
        thirdSlashUnlocked = true;
    }
    #endregion

    public void FirstSlash()
    {
        if (firstSlashUnlocked)
        {
            //slashController.PerformSlash(1, slashDoDamage);

        }
    }

    public void SecondSlash()
    {
        if (secondSlashUnlocked)
        {
            //slashController.PerformSlash(2, slashDoDamage);

        }
    }

    public void ThirdSlash()
    {
        if (thirdSlashUnlocked)
        {
            //slashController.PerformSlash(3, slashDoDamage);

        }
    }


}
