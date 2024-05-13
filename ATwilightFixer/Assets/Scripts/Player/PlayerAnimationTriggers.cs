using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>() ;

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(0, null);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(player.entityHitBox.position, player.entityHitBoxSize, 0);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Arrow_Controller>() != null)
            {
                hit.GetComponent<Arrow_Controller>().FlipArrow();
            }

            if(hit.GetComponent<Enemy>() != null || hit.GetComponent<WorldObject>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                ObjectStats _targetObject = hit.GetComponent<ObjectStats>();

                if (_target != null)
                {

                    player.stats.DoDamage(_target);
                }
                else if (_targetObject != null)
                {
                    player.stats.DoTrueDamage(_targetObject);
                }

                ItemData_Equipment weaponData = Inventory.Instance.GetEquipment(EquipmentType.Weapon);

                if (weaponData != null)
                {
                    weaponData.ExcuteItemEffect();
                }
            }
        }
    }

    private void SlashTrigger()
    {
        Collider2D[] col = Physics2D.OverlapBoxAll(player.slashCheck.position, player.slashBoxSize, 0);
        player.fx.ScreenShake(new Vector3(1.5f, 1.0f));

        foreach (var hit in col)
        {
            if (hit.GetComponent<Enemy>() != null || hit.GetComponent<WorldObject>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                ObjectStats _targetObject = hit.GetComponent<ObjectStats>();

                if (_target != null)
                {
                    _target.TakeDamage(player.skill.slash.slashDoDamage);
                    player.fx.CreatHitFX(_target.transform, true);
                }
                else if (_targetObject != null)
                {
                    _targetObject.TakeDamage(player.skill.slash.slashDoDamage);
                }
            }
        }
    }

    private void WeaopnEffect()
    {
        
    }

    private void ThorwSword()
    {
        SkillManager.instance.sword.CreateSwrod();
    }

}
