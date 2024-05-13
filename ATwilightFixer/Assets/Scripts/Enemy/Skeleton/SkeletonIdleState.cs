using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 2f;
        enemy.SetVelocity(0, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();
        
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0 && enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
