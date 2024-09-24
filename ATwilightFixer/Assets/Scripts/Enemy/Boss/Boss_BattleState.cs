
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Boss_BattleState : EnemyState
{
    protected Transform player;
    protected Enemy_Boss enemy;

    private int moveDir;

    public Boss_BattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        #region Move
        float distanceToPlayerWithFlip = Vector2.Distance(player.position, enemy.transform.position);
        if (distanceToPlayerWithFlip > 1f)
        {
            if (player.position.x > enemy.transform.position.x)
            {
                moveDir = 1;
            }
            else if (player.position.x < enemy.transform.position.x)
            {
                moveDir = -1;
            }
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
        #endregion

        enemy.stateMachine.ChangeState(enemy.spawnThunder);

        if (Vector2.Distance(enemy.transform.position, player.transform.position) > enemy.trackPlayerDistance && enemy.CanUseTrack())
        {
            enemy.stateMachine.ChangeState(enemy.track);
        }

        if (enemy.IsPlayerInAttackRange())
        {
            int random = Random.Range(0, 3);

            if (random == 0)
            {
                enemy.stateMachine.ChangeState(enemy.attack);
            }
            else if (random == 1 && enemy.CanUseFlashCut())
            {
                enemy.stateMachine.ChangeState(enemy.flashCut);
            }
            else if (random == 2 && enemy.CanUseThunder())
            {
                enemy.stateMachine.ChangeState(enemy.spawnThunder);
            }
            else
                enemy.stateMachine.ChangeState(enemy.idle);
        }
    }
}
