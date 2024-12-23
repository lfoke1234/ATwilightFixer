using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity
{
    [SerializeField] protected LayerMask isPlayer;
    [Header("Stunned info")]
    public float stunDuration;
    public Vector2 stunPower;
    public bool canBeStunned;
    [SerializeField] protected GameObject counterImage;


    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    private float defaultMoveSpeed;

    [Header("Attack info")]
    public float attackDistance;
    public float attackCooldown;
    [SerializeField] protected Transform attackChecker;
    [SerializeField] protected Vector2 attackCheckerSize;
    [HideInInspector] public float lastTimeAttacked;
    [SerializeField] private float playerDetectDistance;

    [SerializeField] protected Transform playerChecker;
    [SerializeField] protected Vector2 boxSize;

    public EnemyStateMachine stateMachine { get; private set; }
    public string lastAnimBoolName { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

    }

    #region Set Behavior Speed
    
    public virtual void FreezeTimer(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    protected virtual IEnumerator FreezeTimeFor(float _seconds)
    {
        FreezeTimer(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTimer(false);
    }

    public override void EntitySpeedChangeBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
    }
    #endregion

    public virtual void AssignLastAnimName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            //CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    #endregion

    public virtual void AniamtionFinishedTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual void AnimationUniqueAttackTrigger()
    {

    }

    public virtual Collider2D IsPlayerDetected()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(playerChecker.position, boxSize, 0, isPlayer);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                return hitCollider;
            }
        }

        return null;
    }

    public bool IsPlayerInAttackRange()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(attackChecker.position, attackCheckerSize, 0, isPlayer);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }



    public override void Die()
    {
        base.Die();
        GetComponentInChildren<UI_HealthBar>().DisableHPBar();
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        // Gizmos.color = Color.yellow; // Attack Distance
        // Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + attackDistance * facingDir, transform.position.y));

        // Gizmos.color = Color.green; // PlayerDitected 
        // Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + playerDetectDistance * facingDir, wallCheck.position.y));

        Gizmos.color = Color.yellow; // HitBox
        if (attackChecker != null)
        {
            Vector2 attackBoxCenter = attackChecker.position;
            Gizmos.DrawWireCube(attackBoxCenter, attackCheckerSize);
        }

        Gizmos.color = Color.red; // Player Distance
        Vector2 boxCenter = playerChecker.position;
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }

}
