using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.hasJump = false;
        if (player.IsGroundDetected())
            player.hasSecondJump = false;
        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        CheckSlope();

        if (movementInput.x == player.facingDir && player.IsWallDected())
            return;

        if (movementInput.x != 0 && !player.isBusy)
            stateMachine.ChangeState(player.moveState);

        if (IsActionTriggered("Jump") && player.IsGroundDetected())
        {
            if (!player.hasJump && !player.hasSecondJump)
                stateMachine.ChangeState(player.jumpState);
        }
    }
}
