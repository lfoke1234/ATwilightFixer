using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float coolDown;
    protected float coolDownTimer;
    public float amount;
    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if(coolDownTimer < 0 && player.stats.currentStamina > amount)
        {
            UseSkill();
            coolDownTimer = coolDown;
            return true;
        }

        player.fx.CreatePopUpText("ÄðÅ¸ÀÓ Áß");
        return false;
    }

    public virtual void UseSkill()
    {

    }
}
