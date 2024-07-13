using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashCutState : PlayerState
{
    private List<Enemy> hitEnemies;
    private HashSet<Enemy> clonesCreated;
    private bool nextTrigger;
    private bool cloneFlip;

    public PlayerFlashCutState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
        hitEnemies = new List<Enemy>();
        clonesCreated = new HashSet<Enemy>();
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
        clonesCreated.Clear();
        nextTrigger = false;
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        if (triggerCalled && nextTrigger == false)
        {
            foreach (Enemy enemy in hitEnemies)
            {
                if (!clonesCreated.Contains(enemy))
                {
                    float posX;
                    Transform enemyPos = enemy.transform;

                    if (player.facingDir == -1)
                    {
                        posX = enemy.transform.position.x + enemy.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2f;
                        cloneFlip = true;
                    }
                    else if (player.facingDir == 1)
                    {
                        posX = enemy.transform.position.x - enemy.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2f;
                        cloneFlip = false;
                    }
                    else
                    {
                        posX = enemy.transform.position.x;
                    }

                    Vector2 newPosition = new Vector2(posX, enemy.transform.position.y);
                    player.skill.clone.CreateClone(newPosition, cloneFlip);
                    clonesCreated.Add(enemy);
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

        float maxDistance = player.flashDistance;

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, maxDistance);
        float travelDistance = maxDistance;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                if (((1 << hit.collider.gameObject.layer) & groundLayer) != 0)
                {
                    travelDistance = hit.distance;
                    break;
                }
                else if (((1 << hit.collider.gameObject.layer) & enemyLayer) != 0)
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    hitEnemies.Add(enemy);
                    enemy.stunDuration = 3f;
                    enemy.CanBeStunned();
                }
            }
        }

        Vector2 targetPosition = origin + direction * travelDistance;
        player.transform.position = targetPosition;
    }
}
