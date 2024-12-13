using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FlashCutState : EnemyState
{
    Enemy_Boss enemy;
    Player player;

    private float flashDistance = 5;
    private bool nextTrigger;
    private bool playerDetected;

    private float playerSpriteX;
    private bool flip;

    public Boss_FlashCutState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Boss enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.UseFlashCut();
        player = PlayerManager.instance.player;
        playerSpriteX = 1f;//player.GetComponentInChildren<Sprite>().bounds.size.x / 2;

        if ((player.transform.position.x < enemy.transform.position.x && enemy.facingDir == 1) ||
            (player.transform.position.x > enemy.transform.position.x && enemy.facingDir == -1))
        {
            enemy.Flip();
        }

        ExecuteFlashCut();
        nextTrigger = false;
    }

    public override void Exit()
    {
        base.Exit();
        nextTrigger = false;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (triggerCalled && !nextTrigger)
        {
            if(playerDetected)
            {
                if (enemy.facingDir == 1)
                {
                    playerSpriteX *= -1;
                    flip = false;
                }
                else
                {
                    playerSpriteX *= 1;
                    flip = true;
                }

                Vector2 position = new Vector2(player.transform.position.x + playerSpriteX, player.transform.position.y);


                CreateClone(position, flip);
            }

            triggerCalled = false;
            nextTrigger = true;
        }

        if (nextTrigger && triggerCalled)
        {
            enemy.stateMachine.ChangeState(enemy.idle);
        }
    }

    private void ExecuteFlashCut()
    {
        Vector2 origin = enemy.transform.position;
        Vector2 direction = new Vector2(enemy.facingDir, 0);

        // 레이어 설정
        LayerMask playerLayer = LayerMask.GetMask("Player");
        LayerMask groundLayer = LayerMask.GetMask("Ground");
        LayerMask brokenWall = LayerMask.GetMask("BrokenWall");
        float maxDistance = flashDistance;

        // 보스의 크기와 거리를 통한 충돌 판정
        Vector2 boxSize = new Vector2(enemy.transform.localScale.x, enemy.transform.localScale.y);
        RaycastHit2D[] hit = Physics2D.RaycastAll(enemy.transform.position, direction, flashDistance);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, boxSize, 0, direction, maxDistance);
        float travelDistance = maxDistance;

        // 감지된 충돌 객체들을 순회
        foreach (RaycastHit2D player in hit)
        {
            if (player.collider != null)
            {
                if ((((1 << player.collider.gameObject.layer) & groundLayer) != 0)
                    || ((1 << player.collider.gameObject.layer) & brokenWall) != 0)
                {
                    travelDistance = player.distance;
                }
                else if (((1 << player.collider.gameObject.layer) & playerLayer) != 0)
                {
                    playerDetected = true;
                }
            }
        }

        // 이동할 목표 위치 계산
        Vector2 targetPosition = origin + direction * travelDistance;
        enemy.transform.position = targetPosition;
    }

    // 지정된 위치에 보스의 분신을 생성
    private void CreateClone(Vector2 position, bool isFlip)
    {
        GameObject clone = Instantiate(enemy.clonePrefab, position, Quaternion.identity);
        clone.GetComponent<Boss_Clone>().SetupClone(position, isFlip);
    }
}
