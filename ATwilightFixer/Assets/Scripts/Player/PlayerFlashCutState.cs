using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashCutState : PlayerState
{
    private List<Enemy> hitEnemies;
    private bool nextTrigger;

    public PlayerFlashCutState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
        hitEnemies = new List<Enemy>();
    }

    public override void Enter()
    {
        base.Enter();
        ExecuteFlashCut();
        nextTrigger = false;
    }

    public override void Exit()
    {
        base.Exit();
        hitEnemies.Clear();
        nextTrigger = false;
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        if (triggerCalled && !nextTrigger)
        {
            foreach (Enemy enemy in hitEnemies)
            {
                if (enemy != null)
                {
                    player.StartCoroutine(ApplyRepeatedDamage(enemy, player.cutCount, 0.2f));
                }
            }

            triggerCalled = false;
            nextTrigger = true;
        }

        if (nextTrigger && triggerCalled)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }

    private void ExecuteFlashCut()
    {
        Vector2 origin = player.transform.position;
        Vector2 direction = new Vector2(player.facingDir, 0);

        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        LayerMask groundLayer = LayerMask.GetMask("Ground");
        LayerMask brokenWall = LayerMask.GetMask("BrokenWall");

        float maxDistance = player.flashDistance;

        Vector2 boxSize = new Vector2(player.transform.localScale.x, player.transform.localScale.y);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, boxSize, 0, direction, maxDistance);
        float travelDistance = maxDistance;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if ((((1 << hit.collider.gameObject.layer) & groundLayer) != 0)
                    || ((1 << hit.collider.gameObject.layer) & brokenWall) != 0)
                {
                    travelDistance = hit.distance;
                }
                else if (((1 << hit.collider.gameObject.layer) & enemyLayer) != 0)
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        hitEnemies.Add(enemy);
                        enemy.stunDuration = 3f;
                        enemy.CanBeStunned();
                    }
                    else
                    {
                        Debug.LogWarning("Hit object does not have an Enemy component.");
                    }
                }
            }
        }

        Vector2 targetPosition = origin + direction * travelDistance;
        player.transform.position = targetPosition;
    }

    private IEnumerator ApplyRepeatedDamage(Enemy enemy, int hitCount, float interval)
    {
        for (int i = 0; i < hitCount; i++)
        {
            if (enemy != null)
            {
                enemy.stats.TakeDamage((int)(player.stats.damage.GetValue() * 0.3f));
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
