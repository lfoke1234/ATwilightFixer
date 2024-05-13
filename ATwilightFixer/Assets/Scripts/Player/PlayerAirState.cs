using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    private float aa;
    private bool bb;
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        bb = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsWallDected())
        {
            stateMachine.ChangeState(player.wallSlide);
        }

        if (Input.GetKeyDown(KeyCode.C) && !player.hasSecondJump)
        {
            stateMachine.ChangeState(player.secondJump);
        }

        if (player.CheckLand() && bb)
        {
            aa = player.rb.velocity.y;
            bb = false;
        }

        if (player.IsGroundDetected())
        {
            //if (aa >= player.landdis)
                stateMachine.ChangeState(player.idleState);
            //else if (aa < player.landdis)
            //    stateMachine.ChangeState(player.landState);

            player.SetVelocity(0.0f, 0.0f);
        }

        if (xInput!= 0)
        {
            player.SetVelocity(xInput * player.moveSpeed * 0.8f, rb.velocity.y);
        }
    }
}
