using UnityEngine;

public class Puzzle_SequentialButton : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject door;
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private Sprite leverOnSprite;
    [SerializeField] private Sprite leverOffSprite;
    [SerializeField] private GameObject finalButton;

    private int currentButtonIndex; // 현재 눌러야 할 버튼의 인덱스
    private bool isCleard;
    private bool puzzleFailed;
    private SpriteRenderer[] buttonRenderers;

    // 퍼즐 초기화
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

    // 플레이어의 입력을 체크하고 버튼을 순서대로 누르는지 확인
    private void CheckPlayerInput()
    {
        if (!isCleard)
        {
            // 현재 눌러야 할 버튼을 플레이어가 근처에서 상호작용한 경우
            if (currentButtonIndex < buttons.Length && IsPlayerNearby(buttons[currentButtonIndex]) && IsActionTriggered("Interaction"))
            {
                TriggerButton(currentButtonIndex);
            }
            // 최종 버튼을 플레이어가 근처에서 상호작용한 경우
            else if (IsPlayerNearby(finalButton) && IsActionTriggered("Interaction"))
            {
                if (currentButtonIndex == buttons.Length && !puzzleFailed) // 모든 버튼을 올바르게 눌렀다면 퍼즐 완료
                {
                    OnPuzzleCompleted();
                }
                else // 올바르게 누르지 않았다면 퍼즐 초기화
                {
                    ResetPuzzle();
                }
            }
            else // 플레이어가 현재 눌러야 할 버튼이 아닌 다른 버튼을 누른 경우
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

    // 순서대로 버튼을 눌렀을 때의 동작
    private void TriggerButton(int buttonIndex)
    {
        if (buttonIndex == currentButtonIndex) // 올바른 버튼을 누른 경우
        {
            buttonRenderers[buttonIndex].sprite = leverOnSprite;
            currentButtonIndex++;
            Debug.Log("Button " + (buttonIndex + 1) + " activated.");
        }
        else
        {
            TriggerIncorrectButton(buttonIndex); // 잘못된 버튼을 누름
        }
    }

    // 잘못된 버튼을 눌렀을 때의 동작
    private void TriggerIncorrectButton(int buttonIndex)
    {
        buttonRenderers[buttonIndex].sprite = leverOnSprite;
        puzzleFailed = true; // 퍼즐 실패 상태로 전환
        Debug.Log("Puzzle failed!");
    }

    // 퍼즐이 완료되었을 때의 동작
    private void OnPuzzleCompleted()
    {
        door.SetActive(false);
        isCleard = true;
        finalButton.GetComponent<SpriteRenderer>().sprite = leverOnSprite; 
        Debug.Log("Puzzle completed!");
    }

    // 퍼즐을 초기 상태로 리셋
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

    // 플레이어가 특정 버튼 근처에 있는지 확인
    private bool IsPlayerNearby(GameObject button)
    {
        return Vector2.Distance(button.transform.position, PlayerManager.instance.player.transform.position) <= distanceToPlayer;
    }

    // 특정 액션이 트리거되었는지 확인 (상호작용 입력)
    public bool IsActionTriggered(string actionName)
    {
        var action = PlayerInputHandler.instance.GetAction(actionName);
        return action != null && action.triggered;
    }
}
