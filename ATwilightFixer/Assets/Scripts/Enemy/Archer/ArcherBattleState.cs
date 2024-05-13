using UnityEngine;

public class ArcherBattleState : EnemyState
{
    Enemy_Archer enemy;
    Transform player;

    public ArcherBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;

        if (player.GetComponent<PlayerStats>().isDead)
            stateMachine.ChangeState(enemy.moveState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            if (enemy.IsPlayerInAttackRange())
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
            }

            if (enemy.IsPlayerClosed() && CanJump())
            {
                enemy.lastTimeJumped = Time.time;

                if (enemy.transform.position.x < player.position.x && enemy.facingDir == -1)
                {
                    enemy.Flip();
                }
                else if (enemy.transform.position.x > player.position.x && enemy.facingDir == 1)
                {
                    enemy.Flip();
                }

                stateMachine.ChangeState(enemy.jumpState);
            }
        }

        float distance = Mathf.Abs(enemy.transform.position.y - player.position.y);

        if (distance < 5f)
        {
            if (enemy.transform.position.x < player.position.x && enemy.facingDir == -1)
            {
                enemy.Flip();
            }
            else if (enemy.transform.position.x > player.position.x && enemy.facingDir == 1)
            {
                enemy.Flip();
            }
        }

    }

    private bool CanAttack()
    {
        if (Time.time > enemy.attackCooldown + enemy.lastTimeAttacked)
        {
            return true;
        }

        return false;
    }

    private bool CanJump()
    {
        // if (enemy.GroundBehind() == false || enemy.WallBehind() == true)
        //     return false;

        if (Time.time >= enemy.lastTimeJumped + enemy.jumpCooldown)
        {
            enemy.lastTimeJumped = Time.time;
            return true;
        }

        return false;
    }
}
