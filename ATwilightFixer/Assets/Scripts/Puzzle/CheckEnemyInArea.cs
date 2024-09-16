using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemyInArea : MonoBehaviour
{
    private float timer;

    [SerializeField] private GameObject door;

    [SerializeField] private Vector2 boxSize;
    [SerializeField] private Transform area;

    [SerializeField] private LayerMask enemy;

    private void Update()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(area.position, boxSize, 0, Vector2.zero, 0, enemy);

        if (hits.Length == 0)
        {
            EnemyCheck();
        }
        else
        {
            timer = 3f;
        }
    }

    private void EnemyCheck()
    {
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            door.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(area.position, boxSize);
    }
}
