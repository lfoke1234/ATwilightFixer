using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSecondJumpState : PlayerState
{
    public PlayerSecondJumpState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(rb.velocity.x, player.jumoForce);
        player.hasSecondJump = true;
        AudioManager.instance.PlaySFX(1, null);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }

        if (movementInput.x != 0)
        {
            player.SetVelocity(movementInput.x * player.moveSpeed, rb.velocity.y);
        }
    }
}
