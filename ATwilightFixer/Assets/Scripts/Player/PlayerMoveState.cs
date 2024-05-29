using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(8, null);
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX(8);
    }

    public override void Update()
    {
        base.Update();

        CheckSlope();

        float maxAngle = 50f;
        if (isSlope && player.IsGroundDetected() && slopeAngle < maxAngle)
        {
            player.SetVelocity(perp * player.moveSpeed * movementInput.x * -1f);
        }
        else if (!isSlope && player.IsGroundDetected())
        {
            player.SetVelocity(movementInput.x * player.moveSpeed, 0);
        }
        else
        {
            player.SetVelocity(movementInput.x * player.moveSpeed, rb.velocity.y);
        }

        //player.SetVelocity(movementInput.x * player.moveSpeed, rb.velocity.y);

        if (movementInput.x == 0 || player.IsWallDected())
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (IsActionTriggered("Jump") && player.IsGroundDetected())
        {
            if (!player.hasJump && !player.hasSecondJump)
                stateMachine.ChangeState(player.jumpState);
        }
    }
}
