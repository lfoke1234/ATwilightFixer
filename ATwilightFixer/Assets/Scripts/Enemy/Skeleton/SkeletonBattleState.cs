using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    Transform player;
    Enemy_Skeleton enemy;
    private int moveDir;

    private float moveTimer;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        InitializeMovement();

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);
    }
    public override void Update()
    {
        base.Update();
        enemy.canBeStunned = true;

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            // float distanceToPlayer = Vector2.Distance(enemy.IsPlayerDetected().transform.position, enemy.transform.position);
            // if (distanceToPlayer < enemy.attackDistance)
            if (enemy.IsPlayerInAttackRange())
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        //else
        //{
        //    if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
        //        stateMachine.ChangeState(enemy.idleState);
        //}

        #region Old Move
        // if (player.position.x > enemy.transform.position.x)
        // {
        //     moveDir = 1;
        // }
        // else if (player.position.x < enemy.transform.position.x)
        // {
        //     moveDir = -1;   
        // }
        // 
        // enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
        #endregion

        if (moveTimer > 0)
        {
            moveTimer -= Time.deltaTime;
            enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
            if (moveTimer <= 0)
            {
                enemy.SetVelocity(0, rb.velocity.y);
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        if (enemy.IsWallDected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }

    private void InitializeMovement()
    {
        int randomDirection = Random.Range(0, 2);
        moveDir = (randomDirection == 0) ? -1 : 1;
        moveTimer = Random.Range(2f, 4f);
    }

    private bool CanAttack()
    {
        if(Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
