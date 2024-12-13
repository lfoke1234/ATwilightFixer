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
        // 슬래시 범위 안에 있는 모든 충돌체 탐지
        Collider2D[] col = Physics2D.OverlapBoxAll(player.slashCheck.position, player.slashBoxSize, 0);
        player.fx.ScreenShake(new Vector3(1.5f, 1.0f));

        foreach (var hit in col)
        {
            // Enemy나 WorldObject에 충돌했을 경우 처리
            if (hit.GetComponent<Enemy>() != null || hit.GetComponent<WorldObject>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                ObjectStats _targetObject = hit.GetComponent<ObjectStats>();

                if (_target != null)
                {
                    // 데미지 적용
                    _target.TakeDamage(player.skill.slash.slashDoDamage);

                    // 피격 효과 생성
                    player.fx.CreatHitFX(_target.transform, true);

                    // 타겟을 위로 띄우는 힘 적용
                    Rigidbody2D targetRb = _target.GetComponent<Rigidbody2D>();
                    if (targetRb != null)
                    {
                        float launchForce = 5f; // 위로 띄우는 힘의 크기
                        targetRb.velocity = new Vector2(targetRb.velocity.x, launchForce);
                    }
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
