using System.Collections;
using UnityEngine;

public class LightningSpawner : MonoBehaviour
{
    [Header("Position")]
    public Transform startPosition;
    public Transform endPosition;
    private Vector3 nextSpawnPosition;
    [SerializeField] private float increaseXPosition;

    [Header("Prefab")]
    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private GameObject warningSignPrefab;

    [Header("Timer")]
    [SerializeField] private float warningTime = 1f;
    [SerializeField] private float thunderAnimTime;
    [SerializeField] private float spawnThunderTime = 5f;

    public LayerMask isGround;
    public bool isStrength;
    [SerializeField] private float test;

    private void Start()
    {
        nextSpawnPosition = startPosition.position;
        StartCoroutine(SpawnThunder());
    }

    private IEnumerator SpawnThunder()
    {
        while (true)
        {
            //Spawn Waring Sign
            RaycastHit2D hit = Physics2D.Raycast(nextSpawnPosition, Vector2.down, 100f, isGround);
            float sizeofSprite = warningSignPrefab.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
            GameObject waring = Instantiate(warningSignPrefab, hit.point + new Vector2(0, sizeofSprite), Quaternion.identity);

            // Wait for second
            yield return new WaitForSeconds(warningTime);

            // Destory Waring and Instantiate Lightning
            Destroy(waring);

            GameObject lightningInstance = Instantiate(lightningPrefab, nextSpawnPosition, Quaternion.identity);
            ScaleThunder(lightningInstance);

            if (isStrength)
                lightningInstance.GetComponent<ThunderAnimationTrigger>().SetPercent(0.15f);

            // Add NextPosition.x
            if (nextSpawnPosition.x <= endPosition.position.x)
                nextSpawnPosition.x = nextSpawnPosition.x + increaseXPosition;
            else
                nextSpawnPosition.x = startPosition.position.x;
            
            // Wait aniamtion and destroy object
            yield return new WaitForSeconds(thunderAnimTime);
            Destroy(lightningInstance);

            yield return new WaitForSeconds(spawnThunderTime);

        }
    }

    private void ScaleThunder(GameObject target)
    {
        // 번개의 시작 위치에서 Ground까지의 거리 측정후 Scale 조정
        RaycastHit2D hit = Physics2D.Raycast(nextSpawnPosition, Vector2.down, 100f, isGround);

        float distance = Vector2.Distance(nextSpawnPosition, hit.point) / test;
        target.transform.localScale = new Vector3(5.3433f, distance, 1f);
    }
}
