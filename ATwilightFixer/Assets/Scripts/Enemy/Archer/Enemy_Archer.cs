using System.Collections;
using UnityEngine;

public class Enemy_Archer : Enemy
{
    [Header("Prefab info")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private float arrowSpeed;
    [Header("Archer Jump info")]
    public Vector2 jumpVelocity;
    public float lastTimeJumped;
    public float jumpCooldown;
    public float jumpTimer;
    [Header("Archer Check info")]
    [SerializeField] private Transform atkpos;
    [SerializeField] private Transform closepos;
    [SerializeField] private Transform groundBehindCheck;
    [SerializeField] private Vector2 groundBehindCheckSize;
    #region State
    public ArcherIdleState idleState { get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherJumpState jumpState { get; private set; }
    public ArcherBattleState battleState { get; private set; }
    public ArcherAttackState attackState { get; private set; }

    public ArcherDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        SetupDefaultFacingDir(1);

        idleState = new ArcherIdleState(this, stateMachine, "Idle", this);
        moveState = new ArcherMoveState(this, stateMachine, "Move", this);
        jumpState = new ArcherJumpState(this, stateMachine, "Jump", this);
        battleState = new ArcherBattleState(this, stateMachine, "Idle", this);
        attackState = new ArcherAttackState(this, stateMachine, "Attack", this);

        deadState = new ArcherDeadState(this, stateMachine, "Dead", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStunned()
    {
        // if (base.CanBeStunned())
        // {
        //     stateMachine.ChangeState(stunnedState);
        //     return true;
        // }

        return false;
    }

    public override void AnimationUniqueAttackTrigger()
    {
        GameObject newArrow = Instantiate(prefab, atkpos.position, Quaternion.identity);

        float arrowRotation = facingDir > 0 ? 0f : 180f;
        newArrow.transform.eulerAngles = new Vector3(0, arrowRotation, 0);

        newArrow.GetComponent<Arrow_Controller>().SetArrow(arrowSpeed * facingDir, stats);
    }

    public bool IsPlayerClosed() => Physics2D.Raycast(closepos.position, new Vector2(facingDir, 0), attackDistance, isPlayer);
    public bool GroundBehind() => Physics2D.BoxCast(groundBehindCheck.position, groundBehindCheckSize, 0, Vector2.zero, 0, isGround);

    public override Collider2D IsPlayerDetected()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(playerChecker.position, boxSize, 0, isPlayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                RaycastHit2D hit = Physics2D.Linecast(transform.position, hitCollider.transform.position, isGround);
                if (hit.collider == null)
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
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(closepos.position, new Vector2(closepos.position.x + facingDir * attackDistance, closepos.position.y));

        Gizmos.DrawWireCube(groundBehindCheck.position, groundBehindCheckSize);

    }
}
