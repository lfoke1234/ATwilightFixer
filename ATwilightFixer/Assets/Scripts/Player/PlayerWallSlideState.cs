using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.hasJump = false;
        player.hasSecondJump = false;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
        rb.isKinematic = false;
    }

    public override void Update()
    {
        base.Update();
        if (IsActionTriggered("Jump"))
        {
            stateMachine.ChangeState(player.wallJump);
            return;
        }

        if (movementInput.y != 0 && player.IsTopDected() == false)
        {
            stateMachine.ChangeState(player.wallMove);
        }

        if(movementInput.x != 0 && player.facingDir != movementInput.x)
        {
            stateMachine.ChangeState(player.idleState);
        }

        //if (yInput < 0)
            rb.velocity = new Vector2 (0, rb.velocity.y);
        //else
        //    rb.velocity = new Vector2 (0, rb.velocity.y * 0.7f);

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
        if (!player.IsWallDected())
            stateMachine.ChangeState(player.airState);

    }
}
