using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlidingState : PlayerState
{
    public PlayerSlidingState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.slidingDuration;
        dontFreeze = true;
        player.gameObject.layer = LayerMask.NameToLayer("PlayerDashing");
        AudioManager.instance.PlaySFX(7, null);

        player.skill.dash.Dash1();
        player.skill.dash.Dash2();
        player.skill.dash.Dash3();
    }

    public override void Exit()
    {
        base.Exit();
        dontFreeze = false;
        player.SetVelocity(0, rb.velocity.y);
        dontFreeze = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public override void Update()
    {
        base.Update();
        if (!player.IsGroundDetected() && player.IsWallDected())
            stateMachine.ChangeState(player.wallSlide);

        if (player.IsGroundDetected() && player.IsWallDected())
            stateMachine.ChangeState(player.idleState);

        player.SetVelocity(player.slidingSpeed * player.dashDir, player.rb.velocity.y);

        AnimatorStateInfo stateInfo = player.anim.GetCurrentAnimatorStateInfo(0);

        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }
}
