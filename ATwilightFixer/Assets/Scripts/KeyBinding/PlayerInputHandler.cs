using System.Collections.Generic;
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

    // 플레이어의 이동 입력을 반환하는 메서드
    public Vector2 GetMovementInput()
    {
        float x = 0f;
        float y = 0f;

        // 각 방향에 대한 입력 상태에 따라 x, y 값 업데이트
        if (upPressed) y += 1f;
        if (downPressed) y -= 1f;
        if (leftPressed) x -= 1f;
        if (rightPressed) x += 1f;

        return new Vector2(x, y);
    }

    // 특정 액션을 이름으로 찾아서 반환하는 메서드
    public InputAction GetAction(string actionName)
    {
        // PlayerController의 asset에서 액션을 검색하여 반환
        return controls.asset.FindAction(actionName);
    }

    // 모든 액션을 리스트로 반환하는 메서드
    public List<InputAction> GetAllActions()
    {
        List<InputAction> actions = new List<InputAction>();
        foreach (var map in controls.asset.actionMaps)
        {
            // 모든 액션 맵을 순회하며 액션들을 리스트에 추가
            actions.AddRange(map.actions);
        }
        return actions;
    }
}
