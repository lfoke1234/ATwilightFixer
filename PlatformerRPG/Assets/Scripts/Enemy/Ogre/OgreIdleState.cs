using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreIdleState : OgreGroundState
{
    public OgreIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Ogre _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        enemy.SetZeroVelocity();
        stateTimer = enemy.idleTime;
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
            stateMachine.ChangeState(enemy.moveState);
    }

    
}
