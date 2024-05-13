using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDeadState : EnemyState
{
    private Enemy_Slime enemy;

    public SlimeDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        #region Mario Dead Effect
        // enemy.anim.SetBool(enemy.lastAnimBoolName, true);
        // enemy.anim.speed = 0;
        // enemy.cd.enabled = false;
        // 
        // stateTimer = 0.1f;
        #endregion

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();
    }
}
