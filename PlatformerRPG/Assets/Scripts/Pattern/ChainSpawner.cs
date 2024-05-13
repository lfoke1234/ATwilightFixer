using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChainSpawner : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float distance;

    [SerializeField] private int secondDamage;
    public bool isStr;

    [Header("Prefab")]
    [SerializeField] private GameObject chainPrefab;
    [SerializeField] private GameObject waringPrefab;

    [Header("Timer")]
    [SerializeField] private float waringTimer;
    [SerializeField] private float chainTimer;
    [SerializeField] private float spawnTimer;


    private void Start()
    {
        player = PlayerManager.instance.player.transform;
        StartCoroutine(SpawnChain());
    }


    private IEnumerator SpawnChain()
    {
        while (true)
        {
            float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 spawnPosition = player.position + new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0) * distance;

            // Spawn Waring Sign

            GameObject waring = Instantiate(waringPrefab, spawnPosition, Quaternion.identity);

            Vector3 direction = player.position - waring.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            waring.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

            yield return new WaitForSeconds(waringTimer);

            // Spawn ChainPrefab
            GameObject chain = Instantiate(chainPrefab, spawnPosition, Quaternion.identity);
            chain.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

            if(isStr)
                chain.GetComponent<ChainAnimation>().SetDamage(secondDamage);

            yield return new WaitForSeconds(chainTimer);

            // Destroy TODO => fade out and destroy
            Destroy(waring);
            Destroy(chain);

            yield return new WaitForSeconds(spawnTimer);
        }

    }
    

    private void SpawnAroundPlayer(GameObject prefab, float randomAngle)
    {
        Vector3 spawnPosition = player.position + new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0) * distance;

        GameObject chain = Instantiate(prefab, spawnPosition, Quaternion.identity);

        Vector3 direction = player.position - chain.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        chain.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
    }


}
