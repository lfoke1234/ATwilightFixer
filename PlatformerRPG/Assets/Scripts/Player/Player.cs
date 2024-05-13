using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public static Player instance;

    [Header("Player Collision info")]
    public Transform slashCheck;
    public Vector2 slashBoxSize;
    public Transform checkLand;
    [SerializeField] private Transform topCheck;
    [SerializeField] private float topCheckDistane;

    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuaration = 0.2f;

    public bool isBusy {  get; private set; }
    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumoForce = 12f;
    public float wallMoveSpeed;
    public bool hasJump;
    public bool hasSecondJump;
    public bool isDash;
    public bool isLanding;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;
    public float landdis;
    public float decreseVelocityX;

    [Header("Dash info")]
    public float defaultDashSpeed;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }

    [SerializeField] private float climbCheckDistance;
    public Transform climbCheck;

    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }


    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerLandState landState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallMoveState wallMove { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerClimbState climbState { get; private set; }
    public PlayerSecondJumpState secondJump { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    public PlayerSlashState slashState { get; private set; }

    public PlayerCrouchState crouchState { get; private set; }
    public PlayerCrouchIdleState crouchIdleState { get; private set; }
    public PlayerStandUpState standupState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        landState = new PlayerLandState(this, stateMachine, "Land");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallMove = new PlayerWallMoveState(this, stateMachine, "WallMove");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        climbState = new PlayerClimbState(this, stateMachine, "Climb");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        secondJump = new PlayerSecondJumpState(this, stateMachine, "Jump");
        slashState = new PlayerSlashState(this, stateMachine, "Slash");

        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");

        deadState = new PlayerDeadState(this, stateMachine, "Die");

        crouchState = new PlayerCrouchState(this, stateMachine, "Crouch");
        crouchIdleState = new PlayerCrouchIdleState(this, stateMachine, "CIdle");
        standupState = new PlayerStandUpState(this, stateMachine, "StandUp");

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

    }

    protected override void Start()
    {
        base.Start();
        DontDestroyOnLoad(this);

        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumoForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        base.Update();

        if (GameManager.Instance.isPlayCutScene)
            return;

        if (Time.timeScale == 0)
            return;

        stateMachine.currentState.Update();
        CheckForDashInput();

        #region Quick Slot
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.Instance.UseQuickSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Inventory.Instance.UseQuickSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Inventory.Instance.UseQuickSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Inventory.Instance.UseQuickSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Inventory.Instance.UseQuickSlot(5);
        }
        #endregion
    }

    #region ChangeSpeed
    //public override void EntitySpeedChangeBy(float _slowPercentage, float _slowDuration)
    //{
    //    moveSpeed = moveSpeed * (1 - _slowPercentage);
    //    jumoForce = jumoForce * (1 - _slowPercentage);
    //    dashSpeed = dashSpeed * (1 - _slowPercentage);
    //    anim.speed = anim.speed * (1 - _slowPercentage);
    //
    //    Invoke("ReturnDefaultSpeed", _slowDuration);
    //}
    //
    //protected override void ReturnDefaultSpeed()
    //{
    //    base.ReturnDefaultSpeed();
    //    moveSpeed = defaultMoveSpeed;
    //    jumoForce = defaultJumpForce;
    //    dashSpeed = defaultDashSpeed;
    //}
    #endregion

    #region Swrod Skill
    public void AssignNewSwrod(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void ClearTheSwrod()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }
    #endregion

    public IEnumerator BusyFor(float _second)
    {
        isBusy = true;

        yield return new WaitForSeconds(_second);

        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    #region Collision

    // public bool SlashCheck()
    // {
    //     Collider2D[] hitColliders = Physics2D.OverlapBoxAll(slashCheck.position, slashBoxSize, 0);
    //     foreach (var hitCollider in hitColliders)
    //     {
    //         if (hitCollider.CompareTag("Enemy"))
    //         {
    //             return true;
    //         }
    //     }
    //     return false;
    // }

    public bool WallClimbCheck() => Physics2D.Raycast(climbCheck.position, Vector2.right * facingDir, climbCheckDistance, isGround);

    public bool CheckLand() => Physics2D.Raycast(checkLand.position, Vector2.down, groundCheckDistance, isGround);

    public virtual bool IsTopDected() => Physics2D.Raycast(topCheck.position, Vector2.up, topCheckDistane, isGround);
    #endregion

    private void CheckForDashInput()
    {
        if (IsWallDected())
            return;

        if (Input.GetKeyDown(KeyCode.Z) && SkillManager.instance.dash.CanUseSkill()) 
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    public void ResetPlayer()
    {
        SpawnManager.Instance.MovePlayerToSpawnPoint();
        stateMachine.ChangeState(idleState);
        stats.currentHealth = stats.GetMaxHealthValue();
        stats.currentStamina = stats.GetMaxStaminaValue();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(climbCheck.position, new Vector3(climbCheck.position.x + climbCheckDistance, climbCheck.position.y));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(slashCheck.position, slashBoxSize);
    }
}
