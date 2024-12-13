using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashCutState : PlayerState
{
    private List<Enemy> hitEnemies;
    private bool nextTrigger;

    public PlayerFlashCutState(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
        hitEnemies = new List<Enemy>();
    }

    public override void Enter()
    {
        base.Enter();
        ExecuteFlashCut();
        nextTrigger = false;
    }

    public override void Exit()
    {
        base.Exit();
        hitEnemies.Clear();
        nextTrigger = false;
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        if (triggerCalled && !nextTrigger)
        {
            foreach (Enemy enemy in hitEnemies)
            {
                if (enemy != null)
                {
                    player.StartCoroutine(ApplyRepeatedDamage(enemy, player.cutCount, 0.2f));
                }
            }

            triggerCalled = false;
            nextTrigger = true;
        }

        if (nextTrigger && triggerCalled)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }

    private void ExecuteFlashCut()
    {
        // 플레이어의 현재 위치와 방향 설정
        Vector2 origin = player.transform.position;
        Vector2 direction = new Vector2(player.facingDir, 0);

        // 타겟 레이어 설정 (적, 지면, 부서질 수 있는 벽)
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        LayerMask groundLayer = LayerMask.GetMask("Ground");
        LayerMask brokenWall = LayerMask.GetMask("BrokenWall");

        // 최대 이동 거리 설정
        float maxDistance = player.flashDistance;

        // 박스캐스트 크기 설정
        Vector2 boxSize = new Vector2(player.transform.localScale.x, player.transform.localScale.y);

        // 방향을 따라 최대 거리까지 박스캐스트 실행
        RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, boxSize, 0, direction, maxDistance);
        float travelDistance = maxDistance;

        // 박스캐스트 결과를 기반으로 충돌 처리
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null)
            {
                // 지면이나 부서질 수 있는 벽과 충돌한 경우 이동 거리를 충돌 지점으로 제한
                if ((((1 << hit.collider.gameObject.layer) & groundLayer) != 0)
                    || ((1 << hit.collider.gameObject.layer) & brokenWall) != 0)
                {
                    travelDistance = hit.distance;
                }
                // 적과 충돌한 경우 적을 스턴 상태로 설정
                else if (((1 << hit.collider.gameObject.layer) & enemyLayer) != 0)
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        hitEnemies.Add(enemy); // 스킬에 맞은 적 리스트에 추가
                        enemy.stunDuration = 3f; // 적의 스턴 지속 시간 설정
                        enemy.CanBeStunned(); // 적 스턴 처리
                    }
                    else
                    {
                        Debug.LogWarning("Hit object does not have an Enemy component.");
                    }
                }
            }
        }

        // 플레이어의 최종 이동 위치 설정
        Vector2 targetPosition = origin + direction * travelDistance;
        player.transform.position = targetPosition;
    }

    private IEnumerator ApplyRepeatedDamage(Enemy enemy, int hitCount, float interval)
    {
        // 지정된 적에게 반복적으로 데미지 적용
        for (int i = 0; i < hitCount; i++)
        {
            if (enemy != null)
            {
                // 매 히트마다 플레이어 공격력의 30%만큼 데미지 적용
                enemy.stats.TakeDamage((int)(player.stats.damage.GetValue() * 0.3f));
            }
            yield return new WaitForSeconds(interval); // 다음 공격까지의 간격 대기
        }
    }

}
