using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    private float timer;

    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = 0.25f;

        player.hasJump = true;
        //player.SetVelocity(6 * -player.facingDir, player.jumoForce);
        dontFreeze = true;
        rb.velocity = new Vector2(6 * -player.facingDir, player.jumoForce);
        //player.Flip();
    }

    public override void Exit()
    {
        base.Exit();
        dontFreeze = false;
    }

    public override void Update()
    {
        base.Update();

        timer -= Time.deltaTime;

        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }

        if (movementInput.x != 0)
        {
            player.SetVelocity(movementInput.x * player.moveSpeed, rb.velocity.y);
        }

        if (timer <= 0 && player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }

}
