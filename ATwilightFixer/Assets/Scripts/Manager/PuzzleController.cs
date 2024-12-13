using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer; // 퍼즐의 SpriteRenderer를 참조하여 퍼즐의 스프라이트 정보를 사용

    [Header("Interaction Text Info")]
    [SerializeField] private GameObject text; // 상호작용 텍스트 오브젝트

    [Header("Check Player")]
    [SerializeField] protected float distanceToPlayer; // 플레이어와의 상호작용 가능한 거리

    protected static PuzzleController closestPuzzleController; // 플레이어와 가장 가까운 퍼즐 컨트롤러

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
    // 플레이어 입력을 체크하여 상호작용 가능한 상태라면 이벤트 발생
    public void CheckPlayerInput()
    {
        if (isPlayerDetected() && IsActionTriggered("Interaction"))
            TriggerEvent(); // 상호작용 이벤트 호출
    }

    // 상호작용 텍스트 활성화/비활성화 체크
    private void ActivetextCheck()
    {
        if (isPlayerDetected())
        {
            // 현재 퍼즐과 플레이어 사이의 거리 계산
            float distance = Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position);

            // closestPuzzleController가 비어있거나, 현재 퍼즐이 더 가까운 경우
            if (closestPuzzleController == null || distance < Vector2.Distance(closestPuzzleController.transform.position, 
                                                                               PlayerManager.instance.player.transform.position))
            {
                if (closestPuzzleController != null)
                {
                    closestPuzzleController.text.SetActive(false); // 이전에 가장 가까운 퍼즐의 텍스트 비활성화
                }
                closestPuzzleController = this; // 현재 퍼즐을 가장 가까운 퍼즐로 설정

                // 상호작용 텍스트 위치 설정
                text.transform.position = new Vector2(transform.position.x, transform.position.y + spriteRenderer.sprite.bounds.size.y / 2 + 1f);
                text.SetActive(true);
            }
        }
        else if (closestPuzzleController == this) // 현재 퍼즐이 더 이상 가장 가까운 퍼즐이 아니게 된 경우
        {
            text.SetActive(false); 
            closestPuzzleController = null; 
        }
    }

    // 플레이어와의 거리 체크를 통해 플레이어가 상호작용 가능한 범위에 있는지 확인
    protected bool isPlayerDetected() => (Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position) <= distanceToPlayer);
    #endregion

    // 퍼즐 상호작용 이벤트 (상속된 클래스에서 오버라이딩하여 특정 동작 구현)
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
