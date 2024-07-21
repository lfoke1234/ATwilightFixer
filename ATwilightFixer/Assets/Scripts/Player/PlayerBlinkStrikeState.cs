using UnityEngine;

public class PlayerBlinkStrikeState : PlayerState
{

    public PlayerBlinkStrikeState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Enemy closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            SpriteRenderer enemySprite = closestEnemy.GetComponentInChildren<SpriteRenderer>();
            if (enemySprite != null)
            {
                Vector2 teleportPos = new Vector2(
                    closestEnemy.transform.position.x + (enemySprite.bounds.size.x / 2f) * (closestEnemy.facingDir * -1),
                    closestEnemy.transform.position.y
                );

                player.transform.position = teleportPos;

                if (player.facingDir != closestEnemy.facingDir)
                {
                    player.Flip();
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();

        if (triggerCalled)
        {
            player.stateMachine.ChangeState(player.idleState);
        }

    }

    private Enemy FindClosestEnemy()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        Enemy closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 playerPosition = player.transform.position;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
