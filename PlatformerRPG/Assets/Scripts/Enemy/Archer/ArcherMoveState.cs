using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherMoveState : ArcherGroundState
{
    private int moveDir;

    public ArcherMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SetRandomMoveValue();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        stateTimer -= Time.deltaTime;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, enemy.rb.velocity.y);

        if (stateTimer <= 0)
            stateMachine.ChangeState(enemy.idleState);
    }


    private void SetRandomMoveValue()
    {
        int dir = Random.Range(0, 2);
        int sec = Random.Range(2, 4);
        
        moveDir = (dir == 0) ? -1 : 1;
        stateTimer = sec;
    }
}
