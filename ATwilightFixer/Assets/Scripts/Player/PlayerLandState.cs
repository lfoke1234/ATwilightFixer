using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerState
{
    public PlayerLandState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.isLanding = true;
        AudioManager.instance.PlaySFX(2, null);
        player.fx.ScreenShake(new Vector3(2.0f, 2.0f));
    }

    public override void Exit()
    {
        base.Exit();
        player.isLanding = false;
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
