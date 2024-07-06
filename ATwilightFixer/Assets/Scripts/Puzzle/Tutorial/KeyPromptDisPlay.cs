using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyPromptDisPlay : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    [Header("Text")]
    [SerializeField] private TextMeshPro[] promptText;

    private Dictionary<string, string> keyToSymbol = new Dictionary<string, string>
    {
        { "Left Arrow", "��" },
        { "Right Arrow", "��" },
        { "Up Arrow", "��" },
        { "Down Arrow", "��" },
    };


    private void Start()
    {
        UpdatePromptText();
    }

    private void UpdatePromptText()
    {
        string leftKey = GetBoundKey("Character", "Left");
        string rightKey = GetBoundKey("Character", "Right");

        promptText[0].text = $"�������� �����̷��� '{leftKey}'Ű�� ��������, ���������� �����̷��� '{rightKey}'Ű�� ��������";
    }

    private string GetBoundKey(string actionMapName, string actionName)
    {
        var actionMap = inputActions.FindActionMap(actionMapName);
        if (actionMap == null)
        {
            Debug.LogError($"ActionMap '{actionMapName}'�� ã�� �� �����ϴ�.");
            return "Unbound";
        }

        var action = actionMap.FindAction(actionName);
        if (action == null)
        {
            Debug.LogError($"Action '{actionName}'�� ã�� �� �����ϴ�.");
            return "Unbound";
        }

        var binding = action.bindings[0];
        var key = InputControlPath.ToHumanReadableString(binding.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);

        if (keyToSymbol.TryGetValue(key, out string symbol))
        {
            return symbol;
        }

        return key.ToUpper();
    }
}
