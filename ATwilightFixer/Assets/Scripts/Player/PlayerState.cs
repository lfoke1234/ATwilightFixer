using UnityEngine;

public class PlayerState
{
    private PlayerController controls;

    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;

    protected Vector2 movementInput;
    protected float xInput;
    protected float yInput;
    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    protected Vector2 perp;
    protected LayerMask ground = 1 << 6;
    public float slopeAngle;
    public bool isSlope;
    public bool dontFreeze;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.player = _player;
        this.animBoolName = animBoolName;
    }


    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        movementInput = PlayerInputHandler.instance.GetMovementInput();
        player.anim.SetFloat("yVelocity", rb.velocity.y);

        // x 입력값에 따른 플레이어 이동 제어
        if (movementInput.x == 0 && dontFreeze == false)
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else if (movementInput.x != 0 || dontFreeze == true)
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    protected bool IsActionTriggered(string actionName)
    {
        // New Input System의 액션네임을 통한 키입력 감지
        var action = PlayerInputHandler.instance.GetAction(actionName);
        return action != null && action.triggered;
    }
}
