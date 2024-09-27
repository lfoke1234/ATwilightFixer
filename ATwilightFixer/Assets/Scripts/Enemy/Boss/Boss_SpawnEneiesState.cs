using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SpawnEneiesState : EnemyState
{
    Enemy_Boss enemy;
    private bool isTrigged;
    public Boss_SpawnEneiesState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        isTrigged = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled && isTrigged == false)
        {
            enemy.UseSpawnEnemies();
            triggerCalled = false;
            isTrigged = true;
        }

        if (triggerCalled && isTrigged)
        {
            enemy.stateMachine.ChangeState(enemy.battle);
        }
    }
}
