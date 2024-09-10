using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TrackState : EnemyState
{
    Enemy_Boss enemy;

    public Boss_TrackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.UseTrack();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (triggerCalled)
        {
            enemy.stateMachine.ChangeState(enemy.flashCut);
        }
    }
}
