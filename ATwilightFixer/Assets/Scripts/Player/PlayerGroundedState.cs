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




        if (IsActionTriggered("Attack") && player.stats.currentStamina > 0)
        {
            stateMachine.ChangeState(player.primaryAttack);
        }

        if (player.IsGroundDetected() == false)
            stateMachine.ChangeState(player.airState);

        

        if (IsActionTriggered("Slash") && SkillManager.instance.slash.CanUseSkill())
        {
            stateMachine.ChangeState(player.slashState);
        }

        // if (Input.GetKeyDown(KeyCode.DownArrow))
        // {
        //     stateMachine.ChangeState(player.crouchState);
        // }


    }

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
