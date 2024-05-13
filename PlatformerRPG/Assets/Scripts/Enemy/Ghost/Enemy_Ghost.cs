using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy_Ghost : Enemy
{
    public Vector2 ghostDefaultPosition;
    public Vector2 lastFindPosition;


    #region
    public GhostIdleState idleState { get; private set; }
    public GhostMoveState moveState { get; private set; }
    public GhostBattleState battleState { get; private set; }
    public GhostAttackState attackState { get; private set; }
    public GhostDeadState deadState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        ghostDefaultPosition = this.transform.position;

        idleState = new GhostIdleState(this, stateMachine, "Idle", this);
        moveState = new GhostMoveState(this, stateMachine, "Move", this);
        battleState = new GhostBattleState(this, stateMachine, "Move", this);
        attackState = new GhostAttackState(this, stateMachine, "Attack", this);

        deadState = new GhostDeadState(this, stateMachine, "Die", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        SetupDefaultFacingDir(-1);
    }

    protected override void Update()
    {
        base.Update();
    }

    // public override bool CanBeStunned()
    // {
    //     if (base.CanBeStunned())
    //     {
    //         stateMachine.ChangeState(stunnedState);
    //         return true;
    //     }
    // 
    //     return false;
    // }

    public override Collider2D IsPlayerDetected()
    {
        Vector2 detectionCenter = (Vector2)playerChecker.position + new Vector2(facingDir * boxSize.x / 2, 0);
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(detectionCenter, boxSize, 0, isPlayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                RaycastHit2D hit = Physics2D.Linecast(transform.position, hitCollider.transform.position, isGround);

                if (!hit.collider)
                {
                    return hitCollider;
                }
            }
        }

        return null;
    }


    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);

        StartCoroutine(DestroyAfterDelay(1f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(entityHitBox.position, entityHitBoxSize);

        Gizmos.color = Color.yellow; // Attack Distance
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + attackDistance * facingDir, transform.position.y));

        Gizmos.color = Color.yellow; // HitBox
        if (attackChecker != null)
        {
            Vector2 attackBoxCenter = attackChecker.position;
            Gizmos.DrawWireCube(attackBoxCenter, attackCheckerSize);
        }

        if (playerChecker != null)
        {
            Vector2 detectionCenter = (Vector2)playerChecker.position + new Vector2(facingDir * boxSize.x / 2, 0);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(detectionCenter, boxSize);
        }

    }
}
