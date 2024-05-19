using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_KeyBinding : MonoBehaviour
{
    public Button jumpButton;
    public Text jumpButtonText;

    private void Start()
    {
        jumpButton.onClick.AddListener(() => StartRebinding("Jump", jumpButtonText));
        UpdateButtonText("Jump", jumpButtonText);
    }

    private void StartRebinding(string actionName, Text buttonText)
    {
        var action = PlayerInputHandler.instance.GetAction(actionName);
        if (action == null) return;

        action.Disable();
        action.PerformInteractiveRebinding()
              .WithControlsExcluding("<Mouse>/position")
              .WithControlsExcluding("<Mouse>/delta")
              .OnComplete(operation =>
              {
                  action.Enable();
                  buttonText.text = action.controls[0].path;
                  operation.Dispose();
              })
              .Start();
    }

    private void UpdateButtonText(string actionName, Text buttonText)
    {
        var action = PlayerInputHandler.instance.GetAction(actionName);
        if (action != null)
        {
            buttonText.text = action.controls[0].path;
        }
    }
}
