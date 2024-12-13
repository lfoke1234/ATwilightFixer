using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적 웨이브를 정의하는 클래스, 각 웨이브에는 여러 적들이 포함될 수 있음
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

    // 소환할 적 웨이브 리스트
    [SerializeField] private List<EnemyWave> enemyWavesA;
    [SerializeField] private List<EnemyWave> enemyWavesB;

    // 현재 웨이브에 소환될 적 리스트
    private List<GameObject> currentListA;
    private List<GameObject> currentListB;

    // 스폰된 적들을 저장하는 리스트
    private List<GameObject> spawnedEnemiesA = new List<GameObject>();
    private List<GameObject> spawnedEnemiesB = new List<GameObject>();

    private int currentWave = 0;
    private bool isActive;
    [SerializeField] private float spawnDelay;

    // 플레이어가 트리거에 진입했을 때 웨이브 시작
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && isActive == false)
        {
            StartWave();
            isActive = true; 
        }
    }

    // 웨이브 시작, 현재 웨이브에 적 리스트 할당 후 스폰 시작
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

    // 적 스폰 위치 A에서 적을 순차적으로 소환
    private IEnumerator SpawnEnemyA()
    {
        for (int i = 0; i < currentListA.Count; i++)
        {
            GameObject monster = Instantiate(currentListA[i], posA.position, Quaternion.identity);
            spawnedEnemiesA.Add(monster); // 스폰된 적 리스트에 추가
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // 적 스폰 위치 B에서 적을 순차적으로 소환
    private IEnumerator SpawnEnemyB()
    {
        for (int i = 0; i < currentListB.Count; i++)
        {
            GameObject monster = Instantiate(currentListB[i], posB.position, Quaternion.identity); 
            spawnedEnemiesB.Add(monster); // 스폰된 적 리스트에 추가
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // 매 프레임마다 적 상태 확인
    private void Update()
    {
        CheckEnemiesStatus();
    }

    // 현재 웨이브의 적이 모두 처치되었는지 확인하는 메서드
    private void CheckEnemiesStatus()
    {
        spawnedEnemiesA.RemoveAll(enemy => enemy == null); // 죽은 적들을 리스트에서 제거 (A 위치)
        spawnedEnemiesB.RemoveAll(enemy => enemy == null); // 죽은 적들을 리스트에서 제거 (B 위치)

        // 모든 적이 처치되고 웨이브가 활성화된 경우
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

    // 모든 웨이브가 끝났을 때 문을 열어주는 메서드
    private void Clear()
    {
        door.SetActive(false);
    }
}
