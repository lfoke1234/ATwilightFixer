using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_EnemyAnimationTrigger : MonoBehaviour
{
    private Enemy enemy => GetComponentInParent<Enemy>();
    private void AnimationTrigger()
    {
        enemy.AniamtionFinishedTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] collider = Physics2D.OverlapBoxAll(enemy.entityHitBox.position, enemy.entityHitBoxSize, 0);

        foreach (var hit in collider)
        {
            if (hit.GetComponent<Player>() != null && hit.GetComponent<Player>().gameObject.layer != LayerMask.NameToLayer("PlayerDashing"))
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
            }
        }
    }

    private void UniqueAttackTrigger()
    {
        enemy.AnimationUniqueAttackTrigger();
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
