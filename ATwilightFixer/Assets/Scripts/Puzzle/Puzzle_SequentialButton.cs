using UnityEngine;

public class Puzzle_SequentialButton : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject door;
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private Sprite leverOnSprite;
    [SerializeField] private Sprite leverOffSprite;
    [SerializeField] private GameObject finalButton;

    private int currentButtonIndex; // ���� ������ �� ��ư�� �ε���
    private bool isCleard;
    private bool puzzleFailed;
    private SpriteRenderer[] buttonRenderers;

    // ���� �ʱ�ȭ
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

    // �÷��̾��� �Է��� üũ�ϰ� ��ư�� ������� �������� Ȯ��
    private void CheckPlayerInput()
    {
        if (!isCleard)
        {
            // ���� ������ �� ��ư�� �÷��̾ ��ó���� ��ȣ�ۿ��� ���
            if (currentButtonIndex < buttons.Length && IsPlayerNearby(buttons[currentButtonIndex]) && IsActionTriggered("Interaction"))
            {
                TriggerButton(currentButtonIndex);
            }
            // ���� ��ư�� �÷��̾ ��ó���� ��ȣ�ۿ��� ���
            else if (IsPlayerNearby(finalButton) && IsActionTriggered("Interaction"))
            {
                if (currentButtonIndex == buttons.Length && !puzzleFailed) // ��� ��ư�� �ùٸ��� �����ٸ� ���� �Ϸ�
                {
                    OnPuzzleCompleted();
                }
                else // �ùٸ��� ������ �ʾҴٸ� ���� �ʱ�ȭ
                {
                    ResetPuzzle();
                }
            }
            else // �÷��̾ ���� ������ �� ��ư�� �ƴ� �ٸ� ��ư�� ���� ���
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

    // ������� ��ư�� ������ ���� ����
    private void TriggerButton(int buttonIndex)
    {
        if (buttonIndex == currentButtonIndex) // �ùٸ� ��ư�� ���� ���
        {
            buttonRenderers[buttonIndex].sprite = leverOnSprite;
            currentButtonIndex++;
            Debug.Log("Button " + (buttonIndex + 1) + " activated.");
        }
        else
        {
            TriggerIncorrectButton(buttonIndex); // �߸��� ��ư�� ����
        }
    }

    // �߸��� ��ư�� ������ ���� ����
    private void TriggerIncorrectButton(int buttonIndex)
    {
        buttonRenderers[buttonIndex].sprite = leverOnSprite;
        puzzleFailed = true; // ���� ���� ���·� ��ȯ
        Debug.Log("Puzzle failed!");
    }

    // ������ �Ϸ�Ǿ��� ���� ����
    private void OnPuzzleCompleted()
    {
        door.SetActive(false);
        isCleard = true;
        finalButton.GetComponent<SpriteRenderer>().sprite = leverOnSprite; 
        Debug.Log("Puzzle completed!");
    }

    // ������ �ʱ� ���·� ����
    private void ResetPuzzle()
    {
        currentButtonIndex = 0; 
        puzzleFailed = false;
        Debug.Log("Resetting puzzle.");

        for (int i = 0; i < buttons.Length; i++)
        {
            buttonRenderers[i].sprite = leverOffSprite;
        }
        finalButton.GetComponent<SpriteRenderer>().sprite = leverOffSprite;
    }

    // �÷��̾ Ư�� ��ư ��ó�� �ִ��� Ȯ��
    private bool IsPlayerNearby(GameObject button)
    {
        return Vector2.Distance(button.transform.position, PlayerManager.instance.player.transform.position) <= distanceToPlayer;
    }

    // Ư�� �׼��� Ʈ���ŵǾ����� Ȯ�� (��ȣ�ۿ� �Է�)
    public bool IsActionTriggered(string actionName)
    {
        var action = PlayerInputHandler.instance.GetAction(actionName);
        return action != null && action.triggered;
    }
}
