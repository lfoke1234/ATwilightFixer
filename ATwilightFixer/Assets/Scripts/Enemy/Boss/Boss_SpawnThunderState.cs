using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SpawnThunderState : EnemyState
{
    Enemy_Boss enemy;
    private bool isTrigged;

    public Boss_SpawnThunderState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        isTrigged = false;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled && isTrigged == false)
        {
            enemy.UseSpawnThunder();
            triggerCalled = false;
            isTrigged = true;
        }

        if (triggerCalled && isTrigged)
        {
            enemy.stateMachine.ChangeState(enemy.battle);
        }
    }
}
