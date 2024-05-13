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
        if (isPlayerDetected() && Input.GetKeyDown(KeyCode.E))
            TriggerEvent();
    }
    private void ActivetextCheck()
    {
        if (isPlayerDetected())
        {
            text.transform.position = new Vector2(transform.position.x ,transform.position.y + spriteRenderer.sprite.bounds.size.y / 2 + 1f);
            text.SetActive(true);
        }
        else
            text.SetActive(false);
    }

    protected bool isPlayerDetected() => (Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position) <= distanceToPlayer);
    #endregion

    protected virtual void TriggerEvent()
    {

    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
    }
}
