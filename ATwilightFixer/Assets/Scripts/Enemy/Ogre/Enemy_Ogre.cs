using System.Collections;
using UnityEngine;

public class Enemy_Ogre : Enemy
{
    [Header("Projectile info")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDamage;

    public float attackCooldown2;
    public float lastTimeAttacked2;

    [SerializeField] private Transform projectilePosition;
    [SerializeField] private Transform playerChecker2;
    [SerializeField] private Vector2 playerCheckBoxSize2;

    #region State

    public OgreIdleState idleState { get; private set; }
    public OgreMoveState moveState { get; private set; }
    public OgreBattleState battleState { get; private set; }
    public OgreAttack1State attack1State { get; private set; }
    public OgreAttack2State attack2State { get; private set; }


    public OgreDeadState deadState { get; private set; }

    #endregion


    protected override void Awake()
    {
        base.Awake();
        SetupDefaultFacingDir(-1);

        idleState = new OgreIdleState(this, stateMachine, "Idle", this);
        moveState = new OgreMoveState(this, stateMachine, "Move", this);
        battleState = new OgreBattleState(this, stateMachine, "Move", this);
        attack1State = new OgreAttack1State(this, stateMachine, "Attack1", this);
        attack2State = new OgreAttack2State(this, stateMachine, "Attack2", this);

        deadState = new OgreDeadState(this, stateMachine, "Dead", this);
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
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(null);
            return true;
        }

        return false;
    }

    public Collider2D IsPlayerDetected2()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(playerChecker2.position, playerCheckBoxSize2, 0, isPlayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                return hitCollider;
            }
        }

        return null;
    }


    public override void AnimationUniqueAttackTrigger()
    {
        GameObject newProjectile = Instantiate(prefab, projectilePosition.position, Quaternion.identity);
        Vector3 targetPosition = PlayerManager.instance.player.transform.position;

        newProjectile.GetComponent<Projectile_Controller>().SetupParabolicMotionToTarget(targetPosition, newProjectile.GetComponent<Rigidbody2D>().gravityScale, stats);
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

        Gizmos.color = new Color(0.7f, 0.5f, 1f);
        Gizmos.DrawWireCube(playerChecker2.position, playerCheckBoxSize2);
    }
}
