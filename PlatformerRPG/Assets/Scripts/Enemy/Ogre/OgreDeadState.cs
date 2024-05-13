using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreDeadState : EnemyState
{
    Enemy_Ogre enemy;

    public OgreDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Ogre _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(0f, rb.velocity.y);
    }
}
