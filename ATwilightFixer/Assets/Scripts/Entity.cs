using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Entity : MonoBehaviour
{

    [Header("Collision info")]
    public Transform entityHitBox;
    public Vector2 entityHitBoxSize;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask isGround;
    [SerializeField] protected Transform entityFeet;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public Action onFliped;

    public int knockbackDir { get; private set; }

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackPower;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;


    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        fx = GetComponent<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {

    }

    public virtual void EntitySpeedChangeBy(float _slowPercentage, float _slowDuration)
    {

    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    public virtual void SetupKnockbackDir(Transform _damageDir)
    {
        if (_damageDir.position.x > transform.position.x)
        {
            knockbackDir = -1;
        }
        else if (_damageDir.position.x < transform.position.x)
        {
            knockbackDir = 1;
        }
    }

    public virtual void DamageEffect()
    {
        StartCoroutine("HitKnockback");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        if (knockbackPower.x > 0 || knockbackPower.y > 0)
            rb.velocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);

        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }

    #region Velocity
    public void SetZeroVelocity()
    {
        if (isKnocked) return;

        rb.velocity = new Vector2(0, 0);
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        //if (isKnocked) return;

        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public void SetVelocity(Vector2 velocity)
    {
        rb.velocity = new Vector2(velocity.x, velocity.y);
        FlipController(velocity.x);
    }
    #endregion

    #region Collision

    // public virtual void SlopesHandle()
    // {
    //     RaycastHit2D Hit2D = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, isGround);
    // 
    //     if (Hit2D != false)
    //     {
    //         Vector2 temp = entityFeet.position;
    //         temp.y = Hit2D.point.y;
    //         entityFeet.position = temp;
    //     }
    // }

    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, isGround);
    public virtual bool IsWallDected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, isGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(entityHitBox.position, entityHitBoxSize);
        
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if(onFliped != null)
            onFliped();
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }

    public virtual void SetupDefaultFacingDir(int _direction)
    {
        facingDir = _direction;

        if (facingDir == -1)
            facingRight = false;
    }

    #endregion

    public virtual void Die()
    {

    }

    
}
