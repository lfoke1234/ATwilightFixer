using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeBattleState : EnemyState
{
    private Transform player;
    private Enemy_Slime enemy;
    private int moveDir;
    public SlimeBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);
    }
    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            float distanceToPlayer = Vector2.Distance(enemy.IsPlayerDetected().transform.position, enemy.transform.position);
            if (enemy.IsPlayerInAttackRange())
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }

        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
                stateMachine.ChangeState(enemy.idleState);
        }

        float distanceToPlayerWithFlip = Vector2.Distance(player.position, enemy.transform.position);
        if (distanceToPlayerWithFlip > 1f)
        {
            if (player.position.x > enemy.transform.position.x)
            {
                moveDir = 1;
            }
            else if (player.position.x < enemy.transform.position.x)
            {
                moveDir = -1;
            }
        }

        if (!enemy.IsPlayerInAttackRange())
        {
            enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
        }
        else
        {
            enemy.SetZeroVelocity();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
