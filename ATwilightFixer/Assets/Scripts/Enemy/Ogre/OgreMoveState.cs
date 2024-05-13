using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreMoveState : OgreGroundState
{
    public OgreMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Ogre _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.battleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);

        if (!enemy.IsGroundDetected() || enemy.IsWallDected())
        {
            enemy.Flip();
        }

        if (stateTimer <= 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    
}
