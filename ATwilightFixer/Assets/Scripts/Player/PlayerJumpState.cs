using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    private float jumptimer;

    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(rb.velocity.x, player.jumoForce);
        player.hasJump = true;
        AudioManager.instance.PlaySFX(1, null);
        jumptimer = 0.2f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        jumptimer -= Time.deltaTime;

        if (IsActionTriggered("Jump") && player.hasJump && !player.hasSecondJump)
        {
            stateMachine.ChangeState(player.secondJump);
        }

        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }

        if (movementInput.x != 0)
        {
            player.SetVelocity(movementInput.x * player.moveSpeed, rb.velocity.y);
        }

        if (jumptimer <= 0 && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
