using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMoveState : EnemyState
{
    private Transform player;
    private Enemy_Ghost enemy;
    private Vector2 targetPosition;
    float randomDistance;
    float randomAngle;

    public GhostMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Ghost _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        randomDistance = Random.Range(2f, 5f);
        randomAngle = Random.Range(0, 360f);

        Vector2 direction = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
        targetPosition = (Vector2)enemy.transform.position + direction * randomDistance;

        enemy.FlipController(direction.x);
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

       //if (Vector2.Distance(enemy.transform.position, player.position) < 2)
       //{
       //    if (player.position.x < enemy.transform.position.x && enemy.facingDir == 1)
       //    {
       //        enemy.Flip();
       //    }
       //    else if (player.position.x > enemy.transform.position.x && enemy.facingDir == -1)
       //    {
       //        enemy.Flip();
       //    }
       //}


        Vector2 currentPosition = enemy.transform.position;
        if ((targetPosition - currentPosition).magnitude > 0.1f)
        {
            Vector2 direction = (targetPosition - currentPosition).normalized;
            enemy.transform.position += (Vector3)(direction * enemy.moveSpeed * Time.deltaTime);
        }
        else
        {
            stateMachine.ChangeState(enemy.idleState);
        }

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 1f)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
