using System.Collections;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject born;
    private int i;

    [Header("Born Info")]
    [SerializeField] private float bornTimer = 0.5f;
    [SerializeField] private float animationTimer = 0.5f;
    [SerializeField] private float bornSpeed = 5.0f;
    [SerializeField] private int numberOfBorns = 20;
    private int damage;
    public bool bornSpawnEnd = false;

    private void Start()
    {
        StartCoroutine(BornSpawn());
    }

    private IEnumerator BornSpawn()
    {
        yield return new WaitForSeconds(animationTimer);

        while (i < numberOfBorns)
        {
            for (i = 0; i < numberOfBorns; i++)
            {
                SpawnRandomAngle();
                yield return new WaitForSeconds(bornTimer);
            }
            bornSpawnEnd = true;
        }
    }

    private void SpawnRandomAngle()
    {
        GameObject spawnedBorn = Instantiate(born, transform.position, Quaternion.identity);
        spawnedBorn.GetComponent<BornAnimation>().SetDamage(damage);
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        spawnedBorn.GetComponent<Rigidbody2D>().velocity = randomDirection * bornSpeed;
        spawnedBorn.GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    public void SetDamage(int value) => damage = value;
}
