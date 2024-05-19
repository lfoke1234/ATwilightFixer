using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlashState : PlayerState
{
    private float attackDir;

    public PlayerSlashState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();

        player.skill.slash.FirstSlash();
        player.skill.slash.SecondSlash();
        player.skill.slash.ThirdSlash();
    }

    public override void Exit()
    {
        base.Exit();
        //player.StartCoroutine("Busyfor", 0.15f);
    }

    public override void Update()
    {
        base.Update();
        if (movementInput.x != 0)
            attackDir = movementInput.x;

        player.SetVelocity(player.attackMovement[3].x * attackDir, player.attackMovement[3].y);

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
