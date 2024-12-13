using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �� ���̺긦 �����ϴ� Ŭ����, �� ���̺꿡�� ���� ������ ���Ե� �� ����
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

    // ��ȯ�� �� ���̺� ����Ʈ
    [SerializeField] private List<EnemyWave> enemyWavesA;
    [SerializeField] private List<EnemyWave> enemyWavesB;

    // ���� ���̺꿡 ��ȯ�� �� ����Ʈ
    private List<GameObject> currentListA;
    private List<GameObject> currentListB;

    // ������ ������ �����ϴ� ����Ʈ
    private List<GameObject> spawnedEnemiesA = new List<GameObject>();
    private List<GameObject> spawnedEnemiesB = new List<GameObject>();

    private int currentWave = 0;
    private bool isActive;
    [SerializeField] private float spawnDelay;

    // �÷��̾ Ʈ���ſ� �������� �� ���̺� ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && isActive == false)
        {
            StartWave();
            isActive = true; 
        }
    }

    // ���̺� ����, ���� ���̺꿡 �� ����Ʈ �Ҵ� �� ���� ����
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

    // �� ���� ��ġ A���� ���� ���������� ��ȯ
    private IEnumerator SpawnEnemyA()
    {
        for (int i = 0; i < currentListA.Count; i++)
        {
            GameObject monster = Instantiate(currentListA[i], posA.position, Quaternion.identity);
            spawnedEnemiesA.Add(monster); // ������ �� ����Ʈ�� �߰�
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // �� ���� ��ġ B���� ���� ���������� ��ȯ
    private IEnumerator SpawnEnemyB()
    {
        for (int i = 0; i < currentListB.Count; i++)
        {
            GameObject monster = Instantiate(currentListB[i], posB.position, Quaternion.identity); 
            spawnedEnemiesB.Add(monster); // ������ �� ����Ʈ�� �߰�
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // �� �����Ӹ��� �� ���� Ȯ��
    private void Update()
    {
        CheckEnemiesStatus();
    }

    // ���� ���̺��� ���� ��� óġ�Ǿ����� Ȯ���ϴ� �޼���
    private void CheckEnemiesStatus()
    {
        spawnedEnemiesA.RemoveAll(enemy => enemy == null); // ���� ������ ����Ʈ���� ���� (A ��ġ)
        spawnedEnemiesB.RemoveAll(enemy => enemy == null); // ���� ������ ����Ʈ���� ���� (B ��ġ)

        // ��� ���� óġ�ǰ� ���̺갡 Ȱ��ȭ�� ���
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

    // ��� ���̺갡 ������ �� ���� �����ִ� �޼���
    private void Clear()
    {
        door.SetActive(false);
    }
}
