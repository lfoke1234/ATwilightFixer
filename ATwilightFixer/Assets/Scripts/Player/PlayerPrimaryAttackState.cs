using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2f;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        dontFreeze = true;

        if (comboCounter > 1 || Time.time >= lastTimeAttacked + comboWindow) 
        {
            comboCounter = 0;
        }

        // 현재 카운터에 따른 애니메이션 세팅
        player.anim.SetInteger("ComboCounter", comboCounter);

        // 키 입력에 따른 플레이어 forward 설정
        float attackDir = player.facingDir;
        if (movementInput.x != 0)
        {
            attackDir = movementInput.x;
        }

        // 공격마다 역동감을 주기위한 속도값 추가
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = 0.1f;
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.15f);

        comboCounter++;
        lastTimeAttacked = Time.time;
        dontFreeze = false;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
