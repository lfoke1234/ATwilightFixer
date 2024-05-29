using Unity.VisualScripting;
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
        Debug.Log(dontFreeze);

        stateTimer -= Time.deltaTime;
        movementInput = PlayerInputHandler.instance.GetMovementInput();
        //xInput = Input.GetAxisRaw("Horizontal");
        //yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.velocity.y);

        if (movementInput.x == 0 && dontFreeze == false)
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else if (movementInput.x != 0 || dontFreeze == true)
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        //CheckSlope();

        //float maxAngle = 50f;
        //if (isSlope && player.IsGroundDetected() && slopeAngle < maxAngle)
        //    rb.velocity = perp * player.moveSpeed * movementInput.x * -1f;
        //else if (!isSlope && player.IsGroundDetected())
        //    rb.velocity = new Vector2(movementInput.x, 0);
        //else
        //rb.velocity = new Vector2(movementInput.x, rb.velocity.y);
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
        var action = PlayerInputHandler.instance.GetAction(actionName);
        return action != null && action.triggered;
    }
}
