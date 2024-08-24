using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public static Player instance;

    [Header("Player Collision info")]
    [SerializeField] private Vector2 groundCheckSize;
    public Transform slashCheck;
    public Vector2 slashBoxSize;
    public Transform checkLand;
    [SerializeField] private Transform topCheck;
    [SerializeField] private float topCheckDistane;
    public Vector2 defaultColOffset;
    public Vector2 defaultColSize;

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
    public float slidingSpeed;
    public float slidingDuration;
    public float dashDir { get; private set; }

    [Header("Slope info")]
    public Transform slopeCheckPosition;
    public float slopeCheckDistance;

    [Header("Skill info")]
    public float flashDistance;

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
    public PlayerSlidingState slidingState { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }

    public PlayerBlinkStrikeState blinkStrike { get; private set; }
    public PlayerSlashState slashState { get; private set; }
    public PlayerFlashCutState flashCut { get; private set; }

    public PlayerDeadState deadState { get; private set; }

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
        slidingState = new PlayerSlidingState(this, stateMachine, "Sliding");
        secondJump = new PlayerSecondJumpState(this, stateMachine, "Jump");


        blinkStrike = new PlayerBlinkStrikeState(this, stateMachine, "BlinkStrike");
        slashState = new PlayerSlashState(this, stateMachine, "Slash");
        flashCut = new PlayerFlashCutState(this, stateMachine, "FlashCut");

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
        defaultColOffset = cd.offset;
        defaultColSize = cd.size;
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
        if (IsActionTriggered("QuickSlot01"))
        {
            Inventory.Instance.UseQuickSlot(1);
        }
        else if (IsActionTriggered("QuickSlot02"))
        {
            Inventory.Instance.UseQuickSlot(2);
        }
        else if (IsActionTriggered("QuickSlot03"))
        {
            Inventory.Instance.UseQuickSlot(3);
        }
        else if (IsActionTriggered("QuickSlot04"))
        {
            Inventory.Instance.UseQuickSlot(4);
        }
        else if (IsActionTriggered("QuickSlot05"))
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

    public override bool IsGroundDetected()
    {
        Vector2 boxCenter = (Vector2)groundCheck.position + Vector2.down * (groundCheckDistance / 2);
        return Physics2D.BoxCast(boxCenter, groundCheckSize, 0f, Vector2.down, groundCheckDistance / 2, isGround);
    }
    #endregion

    private void CheckForDashInput()
    {
        if (IsWallDected())
            return;

        if (IsActionTriggered("Dash") && SkillManager.instance.dash.CanUseSkill()) 
        {
            Vector2 dashMovement = PlayerInputHandler.instance.GetMovementInput();
            dashDir = dashMovement.x;

            if (dashDir == 0)
                dashDir = facingDir;
            
            if (skill.dash.dash1Unlocked == true ||
                skill.dash.dash2Unlocked == true ||
                skill.dash.dash3Unlocked == true)
                stateMachine.ChangeState(dashState);
            else if (skill.sliding.sliding1Unlocked == true ||
                     skill.sliding.sliding2Unlocked == true ||
                     skill.sliding.sliding3Unlocked == true)
                stateMachine.ChangeState(slidingState);
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

    private bool IsActionTriggered(string actionName)
    {
        var action = PlayerInputHandler.instance.GetAction(actionName);
        return action != null && action.triggered;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Vector2 boxCenter = (Vector2)groundCheck.position + Vector2.down * (groundCheckDistance / 2);
        Gizmos.DrawWireCube(boxCenter, groundCheckSize);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(climbCheck.position, new Vector3(climbCheck.position.x + climbCheckDistance, climbCheck.position.y));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(slashCheck.position, slashBoxSize);

        Gizmos.DrawLine(slopeCheckPosition.position, new Vector2(slopeCheckPosition.position.x, slopeCheckPosition.position.y - slopeCheckDistance));
    }
}
