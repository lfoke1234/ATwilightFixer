using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInputHandler instance;

    private PlayerController controls;
    private bool upPressed;
    private bool downPressed;
    private bool leftPressed;
    private bool rightPressed;

    private void Awake()
    {
        controls = new PlayerController();

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

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        controls.Character.Up.performed += ctx => upPressed = ctx.ReadValue<float>() > 0;
        controls.Character.Up.canceled += ctx => upPressed = false;

        controls.Character.Down.performed += ctx => downPressed = ctx.ReadValue<float>() > 0;
        controls.Character.Down.canceled += ctx => downPressed = false;

        controls.Character.Left.performed += ctx => leftPressed = ctx.ReadValue<float>() > 0;
        controls.Character.Left.canceled += ctx => leftPressed = false;

        controls.Character.Right.performed += ctx => rightPressed = ctx.ReadValue<float>() > 0;
        controls.Character.Right.canceled += ctx => rightPressed = false;
    }

    public Vector2 GetMovementInput()
    {
        float x = 0f;
        float y = 0f;

        if (upPressed) y += 1f;
        if (downPressed) y -= 1f;
        if (leftPressed) x -= 1f;
        if (rightPressed) x += 1f;

        return new Vector2(x, y);
    }

    public InputAction GetAction(string actionName)
    {
        return controls.asset.FindAction(actionName);
    }
}
