using UnityEngine;

public class Puzzle_SequentialButton : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;  // 순서대로 눌러야 하는 버튼들
    [SerializeField] private GameObject door;  // 퍼즐이 완료되면 열리는 문
    [SerializeField] private float distanceToPlayer;  // 플레이어와 버튼 간의 거리
    [SerializeField] private Sprite leverOnSprite;  // 활성화된 레버 스프라이트
    [SerializeField] private Sprite leverOffSprite; // 비활성화된 레버 스프라이트
    [SerializeField] private GameObject finalButton; // 최종 확인 버튼

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
            // currentButtonIndex가 buttons.Length보다 크지 않도록 확인
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
        buttonRenderers[buttonIndex].sprite = leverOnSprite; // 틀린 레버도 스프라이트 변경
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

        // 모든 레버의 스프라이트를 원래대로 돌림
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
