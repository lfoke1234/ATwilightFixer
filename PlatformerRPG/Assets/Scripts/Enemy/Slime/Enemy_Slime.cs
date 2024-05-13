using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum SlimeType
{
    big,
    medium,
    small,
}
public class Enemy_Slime : Enemy
{
    [Header("Slime spsific")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int childAmount;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private Vector2 minCreateVelocity;
    [SerializeField] private Vector2 maxCreateVelocity;

    #region State

    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunnedState stunnedState { get; private set; }
    public SlimeDeadState deadState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        SetupDefaultFacingDir(-1);

        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBattleState(this, stateMachine, "Move", this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);

        stunnedState = new SlimeStunnedState(this, stateMachine, "Stunned", this);
        deadState = new SlimeDeadState(this, stateMachine, "Dead", this);
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
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);

        //if (slimeType == SlimeType.small)
            StartCoroutine(DestroyAfterDelay(2f));
    }

    public override void AnimationUniqueAttackTrigger()
    {
        CreateSliem(childAmount, slimePrefab);
    }

    private void CreateSliem(int _amountSlimes, GameObject _slimePrefab)
    {
        for (int i = 0; i < _amountSlimes; i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab, transform.position, Quaternion.identity);

            newSlime.GetComponent<Enemy_Slime>().SetUpSlime(facingDir);
        }
    }

    public void SetUpSlime(int _facingDir)
    {
        if (facingDir != facingDir)
            Flip();

        float xVelocity = Random.Range(minCreateVelocity.x, maxCreateVelocity.x);
        float yVelocity = Random.Range(minCreateVelocity.y, maxCreateVelocity.y);

        isKnocked = true;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * facingDir, yVelocity);

        Invoke("CancelKnockback", 1.5f);
    }

    private void CancelKnockback() => isKnocked = false;

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
