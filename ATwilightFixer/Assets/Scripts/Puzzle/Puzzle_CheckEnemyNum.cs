using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_CheckEnemyNum : PuzzleController
{
    [Header("Puzzle Info")]
    [SerializeField] protected Transform checkNum;
    [SerializeField] protected Vector2 checkSize;
    [SerializeField] protected LayerMask isEnemy;

    [SerializeField] private GameObject door;

    protected override void Update()
    {
        base.Update();
    }

    protected void CheckEnemiseNum()
    {
        enemyNum = 0;

        Collider2D[] hit = Physics2D.OverlapBoxAll(checkNum.position, checkSize, 0, isEnemy);

        foreach (var hitCollider in hit)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                enemyNum++;
            }
        }
    }
    protected override void TriggerEvent()
    {
        CheckEnemiseNum();
        if (enemyNum >= 6)
        {
            door.SetActive(false);
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(checkNum.position, checkSize);

    }
}
