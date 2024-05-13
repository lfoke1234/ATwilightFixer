using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreAttack2State : EnemyState
{
    Enemy_Ogre enemy;
    Transform player;

    public OgreAttack2State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Ogre _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }

   
}
