using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;

    [Header("Interaction Text Info")]
    [SerializeField] private GameObject text;

    [Header("Check Player")]
    [SerializeField] protected float distanceToPlayer;

    protected static PuzzleController closestPuzzleController;

    protected int enemyNum;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        CheckPlayerInput();
        ActivetextCheck();
    }

    #region Check Player
    public void CheckPlayerInput()
    {
        if (isPlayerDetected() && IsActionTriggered("Interaction"))
            TriggerEvent();
    }
    private void ActivetextCheck()
    {
        if (isPlayerDetected())
        {
            float distance = Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position);
            if (closestPuzzleController == null || distance < Vector2.Distance(closestPuzzleController.transform.position, PlayerManager.instance.player.transform.position))
            {
                if (closestPuzzleController != null)
                {
                    closestPuzzleController.text.SetActive(false);
                }
                closestPuzzleController = this;
                text.transform.position = new Vector2(transform.position.x, transform.position.y + spriteRenderer.sprite.bounds.size.y / 2 + 1f);
                text.SetActive(true);
            }
        }
        else if (closestPuzzleController == this)
        {
            text.SetActive(false);
            closestPuzzleController = null;
        }
    }

    // text를 각각 할당시켜줘야해서 바꿈 문제 생길시 이걸로 바꾸고 각각 할당 시켜주기
    // private void ActivetextCheck()
    // {
    //     if (isPlayerDetected())
    //     {
    //         text.transform.position = new Vector2(transform.position.x, transform.position.y + spriteRenderer.sprite.bounds.size.y / 2 + 1f);
    //         text.SetActive(true);
    //     }
    //     else
    //     {
    //         text.SetActive(false);
    //     }
    // }

    protected bool isPlayerDetected() => (Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position) <= distanceToPlayer);
    #endregion

    protected virtual void TriggerEvent()
    {

    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
    }

    public bool IsActionTriggered(string actionName)
    {
        var action = PlayerInputHandler.instance.GetAction(actionName);
        return action != null && action.triggered;
    }
}
