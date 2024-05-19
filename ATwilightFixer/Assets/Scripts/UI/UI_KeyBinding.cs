using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_KeyBinding : MonoBehaviour
{
    #region Button
    [SerializeField] private Button rightButton;
    [SerializeField] private TextMeshProUGUI rightText;

    [SerializeField] private Button leftButton;
    [SerializeField] private TextMeshProUGUI leftText;

    [SerializeField] private Button upButton;
    [SerializeField] private TextMeshProUGUI upText;

    [SerializeField] private Button downButton;
    [SerializeField] private TextMeshProUGUI downText;

    [SerializeField] private Button jumpButton;
    [SerializeField] private TextMeshProUGUI jumpText;

    [SerializeField] private Button dashButton;
    [SerializeField] private TextMeshProUGUI dashText;

    [SerializeField] private Button attackButton;
    [SerializeField] private TextMeshProUGUI attackText;

    [SerializeField] private Button slashButton;
    [SerializeField] private TextMeshProUGUI slashText;

    [SerializeField] private Button inventoryButton;
    [SerializeField] private TextMeshProUGUI inventoryText;

    [SerializeField] private Button skillButton;
    [SerializeField] private TextMeshProUGUI skillText;

    [SerializeField] private Button optionButton;
    [SerializeField] private TextMeshProUGUI optionText;
    #endregion

    private void Start()
    {
        #region AddListener and Update Text
        rightButton.onClick.AddListener(() => StartRebinding("Right", rightText));
        UpdateButtonText("Right", rightText);

        leftButton.onClick.AddListener(() => StartRebinding("Left", leftText));
        UpdateButtonText("Left", leftText);

        upButton.onClick.AddListener(() => StartRebinding("Up", upText));
        UpdateButtonText("Up", upText);

        downButton.onClick.AddListener(() => StartRebinding("Down", downText));
        UpdateButtonText("Down", downText);

        jumpButton.onClick.AddListener(() => StartRebinding("Jump", jumpText));
        UpdateButtonText("Jump", jumpText);

        dashButton.onClick.AddListener(() => StartRebinding("Dash", dashText));
        UpdateButtonText("Dash", dashText);

        attackButton.onClick.AddListener(() => StartRebinding("Attack", attackText));
        UpdateButtonText("Attack", attackText);

        slashButton.onClick.AddListener(() => StartRebinding("Slash", slashText));
        UpdateButtonText("Slash", slashText);

        inventoryButton.onClick.AddListener(() => StartRebinding("UI_Inventory", inventoryText));
        UpdateButtonText("UI_Inventory", inventoryText);

        skillButton.onClick.AddListener(() => StartRebinding("UI_Skilltree", skillText));
        UpdateButtonText("UI_Skilltree", skillText);

        optionButton.onClick.AddListener(() => StartRebinding("UI_Option", optionText));
        UpdateButtonText("UI_Option", optionText);
        #endregion
    }

    private void StartRebinding(string actionName, TextMeshProUGUI buttonText)
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
                  UpdateButtonTextWithSize(action.controls[0].path, buttonText);
                  operation.Dispose();
              })
              .Start();
    }

    private void UpdateButtonText(string actionName, TextMeshProUGUI buttonText)
    {
        var action = PlayerInputHandler.instance.GetAction(actionName);
        if (action != null)
        {
            UpdateButtonTextWithSize(action.controls[0].path, buttonText);
        }
    }

    private void UpdateButtonTextWithSize(string controlPath, TextMeshProUGUI buttonText)
    {
        string readableName;
        int fontSize = 36;

        switch (InputControlPath.ToHumanReadableString(controlPath, InputControlPath.HumanReadableStringOptions.OmitDevice))
        {
            case "upArrow":
                readableName = "ก่";
                fontSize = 48;
                break;
            case "downArrow":
                readableName = "ก้";
                fontSize = 48;
                break;
            case "leftArrow":
                readableName = "ก็";
                fontSize = 48;
                break;
            case "rightArrow":
                readableName = "กๆ";
                fontSize = 48;
                break;
            default:
                readableName = InputControlPath.ToHumanReadableString(controlPath, InputControlPath.HumanReadableStringOptions.OmitDevice).ToUpper();
                break;
        }

        buttonText.text = readableName;
        buttonText.fontSize = fontSize;
    }
}
