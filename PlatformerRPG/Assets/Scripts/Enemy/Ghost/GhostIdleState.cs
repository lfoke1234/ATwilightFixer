using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostIdleState : EnemyState
{
    private float returnDistance = 20f;
    protected Transform player;
    protected Enemy_Ghost enemy;

    public GhostIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Ghost _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }

        if (enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.battleState);
        }

        if (Vector2.Distance(enemy.transform.position, player.position) < 2)
        {
            if (player.position.x < enemy.transform.position.x && enemy.facingDir == 1)
            {
                enemy.Flip();
            }
            else if (player.position.x > enemy.transform.position.x && enemy.facingDir == -1)
            {
                enemy.Flip();
            }
        }


        // º¹±Í
        float distanceToDefaultPosition = Vector2.Distance(enemy.transform.position, enemy.ghostDefaultPosition);
        Vector2 direction = ((Vector3)enemy.ghostDefaultPosition - enemy.transform.position).normalized;
        if (distanceToDefaultPosition > returnDistance)
        {
            enemy.transform.position += (Vector3)direction * enemy.moveSpeed * Time.deltaTime;
            
            if (enemy.ghostDefaultPosition.x < enemy.transform.position.x && enemy.facingDir == 1)
            {
                enemy.Flip();
            }
            else if (enemy.ghostDefaultPosition.x > enemy.transform.position.x && enemy.facingDir == -1)
            {
                enemy.Flip();
            }
        }
    }
}
