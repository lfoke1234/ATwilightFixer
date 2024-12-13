using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
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

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    stateMachine.ChangeState(player.aimSword);
        //}

        //if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSwrod())
        //{
        //    stateMachine.ChangeState(player.aimSword);
        //}


        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     stateMachine.ChangeState(player.counterAttack);
        // }
        // 
        // if(Input.GetKeyDown(KeyCode.X) && player.stats.currentStamina > 0)
        // {
        //     stateMachine.ChangeState(player.primaryAttack);
        // }


        // 충돌 체크에 따른 상태전환
        if (player.IsGroundDetected() == false)
            stateMachine.ChangeState(player.airState);

        // 키 입력에 따른 상태 전환
        if (IsActionTriggered("Attack") && player.stats.currentStamina > 0)
        {
            stateMachine.ChangeState(player.primaryAttack);
        }

        if (IsActionTriggered("Slash") && SkillManager.instance.slash.CanUseSkill())
        {
            stateMachine.ChangeState(player.flashCut);
        }

        // if (Input.GetKeyDown(KeyCode.DownArrow))
        // {
        //     stateMachine.ChangeState(player.crouchState);
        // }


    }

    // 경사로 체크 메서드
    protected void CheckSlope()
    {

        RaycastHit2D hit = Physics2D.Raycast(player.slopeCheckPosition.position, Vector2.down, player.slopeCheckDistance, ground);

        if (hit)
        {
            perp = Vector2.Perpendicular(hit.normal);
            slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeAngle != 0)
                isSlope = true;
            else
                isSlope = false;

            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.blue);
        }
    }

    private bool HasNoSwrod()
    {
        if(!player.sword)
        {
            return true;
        }

        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
