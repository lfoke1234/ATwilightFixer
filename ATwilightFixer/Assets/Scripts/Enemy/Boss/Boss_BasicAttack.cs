using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_BasicAttack : EnemyState
{
    Enemy_Boss enemy;
    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2f;

    public Boss_BasicAttack(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 1 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }

        enemy.anim.SetInteger("ComboCounter", comboCounter);

        float attackDir = enemy.facingDir;

        // if (movementInput.x != 0)
        // {
        //     attackDir = movementInput.x;
        // }

        // player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();
        // enemy.StartCoroutine("BusyFor", 0.15f);
        comboCounter++;
        Debug.Log(comboCounter);
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            enemy.SetZeroVelocity();

        if (triggerCalled)
        {
            enemy.stateMachine.ChangeState(enemy.idle);
        }
    }
}
