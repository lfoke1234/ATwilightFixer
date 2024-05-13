using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GhostBattleState : EnemyState
{
    Transform player;
    Enemy_Ghost enemy;
    bool isCoroutineRunning;

    public GhostBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Ghost _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        stateTimer -= Time.deltaTime;

        Vector2 currentPosition = enemy.transform.position;
        Vector2 direction = (enemy.lastFindPosition - currentPosition).normalized;
        float distance = Vector2.Distance(currentPosition, enemy.lastFindPosition);

        float distanceToPlayer = Vector2.Distance(player.position, enemy.transform.position);
        if (distanceToPlayer > 1f && stateTimer > 0)
        {
            if (player.position.x > enemy.transform.position.x && enemy.facingDir == -1)
             {
                 enemy.Flip();
             }
             else if ((player.position.x < enemy.transform.position.x || enemy.lastFindPosition.x < enemy.transform.position.x )&& enemy.facingDir == 1)
             {
                 enemy.Flip();
             }
        }

        // 플레이어 위치 기반 시야
        if (enemy.IsPlayerDetected())
        {
            stateTimer = 1.5f;
            enemy.lastFindPosition = player.position;

            if (enemy.IsPlayerInAttackRange())
            {
                enemy.SetZeroVelocity();
            }
            else
            {
                enemy.transform.position += (Vector3)(direction * enemy.moveSpeed * Time.deltaTime);
            }

            if(enemy.IsPlayerInAttackRange())
            {
                if(CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }

        }
        else if(!enemy.IsPlayerDetected())
        {
            if (distance > 0.1f)
            {
                enemy.transform.position += (Vector3)(direction * enemy.moveSpeed * Time.deltaTime);
            }
            else
            {
                enemy.StartCoroutine(WaitAndChangeState(enemy.idleState));
            }
        }
    }

    // 두리번 거리는 표현
    private IEnumerator WaitAndChangeState(EnemyState newState)
    {
        if (isCoroutineRunning) yield break;

        isCoroutineRunning = true;

        yield return new WaitForSeconds(1f);
        enemy.Flip();
        yield return new WaitForSeconds(1f);
        enemy.Flip();
        yield return new WaitForSeconds(0.5f);
        stateMachine.ChangeState(newState);

        isCoroutineRunning = false;
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
