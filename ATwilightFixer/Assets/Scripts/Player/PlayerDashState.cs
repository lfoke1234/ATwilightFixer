using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.dashDuration;
        dontFreeze = true;
        player.gameObject.layer = LayerMask.NameToLayer("PlayerDashing");
        //player.cd.enabled = false;
        //player.stats.ReturnDamage(true);
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
        //player.stats.ReturnDamage(false);
        //player.cd.enabled = true;

    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected() && player.IsWallDected())
            stateMachine.ChangeState(player.wallSlide);

        if (player.IsGroundDetected() && player.IsWallDected())
            stateMachine.ChangeState(player.idleState);

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }
}
