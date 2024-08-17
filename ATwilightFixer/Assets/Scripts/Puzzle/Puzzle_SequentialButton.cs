using UnityEngine;

public class Puzzle_SequentialButton : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;  // ������� ������ �ϴ� ��ư��
    [SerializeField] private GameObject door;  // ������ �Ϸ�Ǹ� ������ ��
    [SerializeField] private float distanceToPlayer;  // �÷��̾�� ��ư ���� �Ÿ�
    [SerializeField] private Sprite leverOnSprite;  // Ȱ��ȭ�� ���� ��������Ʈ
    [SerializeField] private Sprite leverOffSprite; // ��Ȱ��ȭ�� ���� ��������Ʈ
    [SerializeField] private GameObject finalButton; // ���� Ȯ�� ��ư

    private int currentButtonIndex;
    private bool isCleard;
    private bool puzzleFailed;
    private SpriteRenderer[] buttonRenderers;

    private void Start()
    {
        buttonRenderers = new SpriteRenderer[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttonRenderers[i] = buttons[i].GetComponent<SpriteRenderer>();
            buttonRenderers[i].sprite = leverOffSprite;
        }
        finalButton.GetComponent<SpriteRenderer>().sprite = leverOffSprite;
    }

    private void Update()
    {
        CheckPlayerInput();
    }

    private void CheckPlayerInput()
    {
        if (!isCleard)
        {
            // currentButtonIndex�� buttons.Length���� ũ�� �ʵ��� Ȯ��
            if (currentButtonIndex < buttons.Length && IsPlayerNearby(buttons[currentButtonIndex]) && IsActionTriggered("Interaction"))
            {
                TriggerButton(currentButtonIndex);
            }
            else if (IsPlayerNearby(finalButton) && IsActionTriggered("Interaction"))
            {
                if (currentButtonIndex == buttons.Length && !puzzleFailed)
                {
                    OnPuzzleCompleted();
                }
                else
                {
                    ResetPuzzle();
                }
            }
            else
            {
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (i != currentButtonIndex && IsPlayerNearby(buttons[i]) && IsActionTriggered("Interaction"))
                    {
                        TriggerIncorrectButton(i);
                        break;
                    }
                }
            }
        }
    }

    private void TriggerButton(int buttonIndex)
    {
        if (buttonIndex == currentButtonIndex)
        {
            buttonRenderers[buttonIndex].sprite = leverOnSprite;
            currentButtonIndex++;
            Debug.Log("Button " + (buttonIndex + 1) + " activated.");
        }
        else
        {
            TriggerIncorrectButton(buttonIndex);
        }
    }

    private void TriggerIncorrectButton(int buttonIndex)
    {
        buttonRenderers[buttonIndex].sprite = leverOnSprite; // Ʋ�� ������ ��������Ʈ ����
        puzzleFailed = true;
        Debug.Log("Puzzle failed!");
    }

    private void OnPuzzleCompleted()
    {
        door.SetActive(false);
        isCleard = true;
        finalButton.GetComponent<SpriteRenderer>().sprite = leverOnSprite;
        Debug.Log("Puzzle completed!");
    }

    private void ResetPuzzle()
    {
        currentButtonIndex = 0;
        puzzleFailed = false;
        Debug.Log("Resetting puzzle.");

        // ��� ������ ��������Ʈ�� ������� ����
        for (int i = 0; i < buttons.Length; i++)
        {
            buttonRenderers[i].sprite = leverOffSprite;
        }
        finalButton.GetComponent<SpriteRenderer>().sprite = leverOffSprite;
    }

    private bool IsPlayerNearby(GameObject button)
    {
        return Vector2.Distance(button.transform.position, PlayerManager.instance.player.transform.position) <= distanceToPlayer;
    }

    public bool IsActionTriggered(string actionName)
    {
        var action = PlayerInputHandler.instance.GetAction(actionName);
        return action != null && action.triggered;
    }
}
