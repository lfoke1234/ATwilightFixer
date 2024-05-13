using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OgreBattleState : EnemyState
{
    private Enemy_Ogre enemy;
    private Transform player;
    private float moveDir;
    private float battleTimer = 2f;

    public OgreBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Ogre _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        stateTimer = battleTimer;
        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected() && enemy.IsPlayerInAttackRange())
        {
            if (CanAttack1())
            {
                stateMachine.ChangeState(enemy.attack1State);
            }
        }
        else if (enemy.IsPlayerDetected2() && enemy.IsPlayerInAttackRange() == false)
        {
            if (CanAttack2())
            {
                stateMachine.ChangeState(enemy.attack2State);
            }
        }

        if (enemy.IsPlayerDetected() == false)
        {
            stateTimer -= Time.deltaTime;
            if (stateTimer <= 0)
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


    private bool CanAttack1()
    {
        if (Time.time >= enemy.attackCooldown + enemy.lastTimeAttacked)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }

    private bool CanAttack2()
    {
        if (Time.time >= enemy.attackCooldown2 + enemy.lastTimeAttacked2)
        {
            enemy.lastTimeAttacked2 = Time.time;
            return true;
        }

        return false;
    }
}
