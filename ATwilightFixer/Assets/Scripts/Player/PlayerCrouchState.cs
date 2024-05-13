using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouchState : PlayerState
{
    private float currentVelocity;
    private float targetVelocity;

    public PlayerCrouchState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        currentVelocity = rb.velocity.x;
        targetVelocity = 0;

        float newVelocity = Mathf.Lerp(currentVelocity, targetVelocity, Time.deltaTime * player.decreseVelocityX);

        rb.velocity = new Vector2 (newVelocity, rb.velocity.y);

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            stateMachine.ChangeState(player.standupState);
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.crouchIdleState);
        }
    }
}
