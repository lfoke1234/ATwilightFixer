using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletPattern : MonoBehaviour
{
    private Transform player;
    public bool isStr;

    [SerializeField] private int damage;

    [SerializeField] private GameObject spawner;
    [SerializeField] private float distance;

    [Header("Timer")]
    [SerializeField] private float timer;

    private void Start()
    {
        player = PlayerManager.instance.player.transform;
        StartCoroutine(SpawnBulletSpanwer());
    }

    private IEnumerator SpawnBulletSpanwer()
    {
        while (true)
        {
            float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 spawnPosition = player.position + new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0) * distance;

            GameObject bulletSpawner = Instantiate(spawner, spawnPosition, Quaternion.identity);
            if (isStr)
            {
                bulletSpawner.GetComponent<BulletSpawner>().SetDamage((int)(damage * 1.5f));
            }
            else if (!isStr)
            {
                bulletSpawner.GetComponent<BulletSpawner>().SetDamage(damage);
            }

            yield return new WaitUntil(() => bulletSpawner.GetComponent<BulletSpawner>().bornSpawnEnd);
            Destroy(bulletSpawner);

            yield return new WaitForSeconds(timer);
        }
    }

    private void SetRandomPosition()
    {
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 spawnPosition = player.position + new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0) * distance;

        Instantiate(spawner, spawnPosition, Quaternion.identity);
    }
}
