using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallMoveState : PlayerState
{
    public PlayerWallMoveState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.isKinematic = true;

    }

    public override void Exit()
    {
        base.Exit();
        rb.isKinematic = false;

    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, movementInput.y * player.wallMoveSpeed);

        if (movementInput.y == 0)
        {
            stateMachine.ChangeState(player.wallSlide);
        }

        if (movementInput.x != 0 && player.facingDir != movementInput.x)
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (player.WallClimbCheck() == false && movementInput.y > 0)
        {
            stateMachine.ChangeState(player.climbState);
        }

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        if (!player.IsWallDected())
            stateMachine.ChangeState(player.airState);

        if (player.IsTopDected())
            stateMachine.ChangeState(player.wallSlide);

    }
}
