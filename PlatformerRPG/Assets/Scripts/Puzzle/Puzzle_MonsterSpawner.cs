using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Puzzle_MonsterSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject door;
    [SerializeField] private List<GameObject> targetEnemy;
    [SerializeField] private GameObject[] spawnEnemy;

    [Space]
    [SerializeField] private float spawnDistance;
    [SerializeField] private float timer = 2f;

    private bool isActive;
    private bool isSpawning;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            isActive = true;
        }
    }

    private void Update()
    {
        targetEnemy.RemoveAll(monster => monster == null);

        if (isActive && !isSpawning)
        {
            door.SetActive(true);
            isSpawning = true;

            if (spawnEnemy.Length > 0)
                StartCoroutine(SpawnStart());
        }

        if (isActive && targetEnemy.Count == 0)
        {
            door.SetActive(false);
            gameObject.SetActive(false);

            if (spawnEnemy.Length > 0)
                StopCoroutine(SpawnStart());
        }
    }

    private IEnumerator SpawnStart()
    {
        while (true)
        {
            float randomPosX = Random.Range(0f, spawnDistance);
            int randomEnemy = Random.Range(0, spawnEnemy.Length);
            Vector2 spawnPos = new Vector2(transform.position.x + randomPosX, transform.position.y);

            GameObject gameObject = spawnEnemy[randomEnemy];
            Instantiate(gameObject, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(timer);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + spawnDistance, transform.position.y));
    }
}
