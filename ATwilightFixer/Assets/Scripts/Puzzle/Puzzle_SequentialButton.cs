using UnityEngine;

public class Puzzle_SequentialButton : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject door;
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private GameObject textPrefab;
    private int currentButtonIndex;
    private bool isCleard;

    private GameObject[] texts;

    private void Start()
    {
        texts = new GameObject[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            texts[i] = Instantiate(textPrefab, buttons[i].transform.position + Vector3.up * 2, Quaternion.identity);
            texts[i].SetActive(false);
        }
    }

    private void Update()
    {
        CheckPlayerInput();
        UpdateTextVisibility();
    }

    private void UpdateTextVisibility()
    {
        if (!isCleard)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (IsPlayerNearby(buttons[i]))
                {
                    texts[i].SetActive(true);
                }
                else
                {
                    texts[i].SetActive(false);
                }
            }
        }
    }

    private void CheckPlayerInput()
    {
        if (IsPlayerNearby(buttons[currentButtonIndex]) && IsActionTriggered("Interaction"))
        {
            TriggerButton(buttons[currentButtonIndex]);
        }
        else
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i != currentButtonIndex && IsPlayerNearby(buttons[i]) && IsActionTriggered("Interaction"))
                {
                    ResetPuzzle();
                    break;
                }
            }
        }
    }

    private bool IsPlayerNearby(GameObject button)
    {
        return Vector2.Distance(button.transform.position, PlayerManager.instance.player.transform.position) <= distanceToPlayer;
    }

    private void TriggerButton(GameObject button)
    {
        if (currentButtonIndex < buttons.Length - 1)
        {
            currentButtonIndex++;
            Debug.Log(currentButtonIndex);
        }
        else
        {
            OnPuzzleCompleted();
        }
    }

    private void OnPuzzleCompleted()
    {
        door.SetActive(false);
        isCleard = true;
    }

    private void ResetPuzzle()
    {
        currentButtonIndex = 0;
        Debug.Log("¶¯!");
    }

    public bool IsActionTriggered(string actionName)
    {
        var action = PlayerInputHandler.instance.GetAction(actionName);
        return action != null && action.triggered;
    }
}
