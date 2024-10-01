using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_IdleState : EnemyState
{
    Enemy_Boss enemy;

    public Boss_IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
        stateTimer = 5f;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();

        if (stateTimer <= 0 && enemy.startBattle)
        {
            enemy.stateMachine.ChangeState(enemy.battle);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
