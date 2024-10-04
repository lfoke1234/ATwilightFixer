using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWave
{
    public List<GameObject> enemies;
}

public class Puzzle_Wave : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private Transform posA;
    [SerializeField] private Transform posB;

    [SerializeField] private List<EnemyWave> enemyWavesA;
    [SerializeField] private List<EnemyWave> enemyWavesB;

    private List<GameObject> currentListA;
    private List<GameObject> currentListB;

    private List<GameObject> spawnedEnemiesA = new List<GameObject>();
    private List<GameObject> spawnedEnemiesB = new List<GameObject>();

    private int currentWave = 0;
    private bool isActive;
    [SerializeField] private float spawnDelay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && isActive == false)
        {
            StartWave();
            isActive = true;
        }
    }

    private void StartWave()
    {
        if (currentWave < enemyWavesA.Count && currentWave < enemyWavesB.Count)
        {
            currentListA = enemyWavesA[currentWave].enemies;
            currentListB = enemyWavesB[currentWave].enemies;

            StartCoroutine(SpawnEnemyA());
            StartCoroutine(SpawnEnemyB());
        }
    }

    private IEnumerator SpawnEnemyA()
    {
        for (int i = 0; i < currentListA.Count; i++)
        {
            GameObject monster = Instantiate(currentListA[i], posA.position, Quaternion.identity);
            spawnedEnemiesA.Add(monster);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private IEnumerator SpawnEnemyB()
    {
        for (int i = 0; i < currentListB.Count; i++)
        {
            GameObject monster = Instantiate(currentListB[i], posB.position, Quaternion.identity);
            spawnedEnemiesB.Add(monster);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        CheckEnemiesStatus();
    }

    private void CheckEnemiesStatus()
    {
        spawnedEnemiesA.RemoveAll(enemy => enemy == null);
        spawnedEnemiesB.RemoveAll(enemy => enemy == null);

        if (spawnedEnemiesA.Count == 0 && spawnedEnemiesB.Count == 0 && isActive)
        {
            currentWave++;
            if (currentWave < enemyWavesA.Count && currentWave < enemyWavesB.Count)
            {
                StartWave();
            }
            else
            {
                Clear();
            }
        }
    }

    private void Clear()
    {
        door.SetActive(false);
    }
}
