using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer; // ������ SpriteRenderer�� �����Ͽ� ������ ��������Ʈ ������ ���

    [Header("Interaction Text Info")]
    [SerializeField] private GameObject text; // ��ȣ�ۿ� �ؽ�Ʈ ������Ʈ

    [Header("Check Player")]
    [SerializeField] protected float distanceToPlayer; // �÷��̾���� ��ȣ�ۿ� ������ �Ÿ�

    protected static PuzzleController closestPuzzleController; // �÷��̾�� ���� ����� ���� ��Ʈ�ѷ�

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
    // �÷��̾� �Է��� üũ�Ͽ� ��ȣ�ۿ� ������ ���¶�� �̺�Ʈ �߻�
    public void CheckPlayerInput()
    {
        if (isPlayerDetected() && IsActionTriggered("Interaction"))
            TriggerEvent(); // ��ȣ�ۿ� �̺�Ʈ ȣ��
    }

    // ��ȣ�ۿ� �ؽ�Ʈ Ȱ��ȭ/��Ȱ��ȭ üũ
    private void ActivetextCheck()
    {
        if (isPlayerDetected())
        {
            // ���� ����� �÷��̾� ������ �Ÿ� ���
            float distance = Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position);

            // closestPuzzleController�� ����ְų�, ���� ������ �� ����� ���
            if (closestPuzzleController == null || distance < Vector2.Distance(closestPuzzleController.transform.position, 
                                                                               PlayerManager.instance.player.transform.position))
            {
                if (closestPuzzleController != null)
                {
                    closestPuzzleController.text.SetActive(false); // ������ ���� ����� ������ �ؽ�Ʈ ��Ȱ��ȭ
                }
                closestPuzzleController = this; // ���� ������ ���� ����� ����� ����

                // ��ȣ�ۿ� �ؽ�Ʈ ��ġ ����
                text.transform.position = new Vector2(transform.position.x, transform.position.y + spriteRenderer.sprite.bounds.size.y / 2 + 1f);
                text.SetActive(true);
            }
        }
        else if (closestPuzzleController == this) // ���� ������ �� �̻� ���� ����� ������ �ƴϰ� �� ���
        {
            text.SetActive(false); 
            closestPuzzleController = null; 
        }
    }

    // �÷��̾���� �Ÿ� üũ�� ���� �÷��̾ ��ȣ�ۿ� ������ ������ �ִ��� Ȯ��
    protected bool isPlayerDetected() => (Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position) <= distanceToPlayer);
    #endregion

    // ���� ��ȣ�ۿ� �̺�Ʈ (��ӵ� Ŭ�������� �������̵��Ͽ� Ư�� ���� ����)
    protected virtual void TriggerEvent()
    {
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, distanceToPlayer);
    }

    public bool IsActionTriggered(string actionName)
    {
        var action = PlayerInputHandler.instance.GetAction(actionName);
        return action != null && action.triggered;
    }
}
