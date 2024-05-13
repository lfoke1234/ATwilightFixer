using System.Collections;
using UnityEngine;

public class TimelineScreenShake : MonoBehaviour
{
    private Transform player;
    [SerializeField] private GameObject prefab;

    [SerializeField] private float timer;
    [SerializeField] private float distance;

    [Header("Trigger")]
    [SerializeField] private Vector2 shakePower;
    [SerializeField] private float shakeDurations = 0.5f;

    [Header("Timeline")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.5f;
    [SerializeField] private bool trigger;

    private void Awake()
    {
        player = PlayerManager.instance.player.transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && trigger == false)
        {
            PlayerManager.instance.player.fx.ScreenShake(shakePower, shakeDurations);
            trigger = true;
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && GameManager.Instance.isPlayCutScene == false)
        {
            PlayerManager.instance.player.fx.ScreenShake(shakePower, shakeDuration);
            // AudioManager.instance.PlaySFX(, null);

            RandomTimer();
        }


    }

    private void RandomDrop()
    {
        if (prefab == null)
            return;

        float playerX = player.position.x;
        float randomX = playerX + Random.Range(-distance, distance);

        Vector3 dropPosition = new Vector3(randomX, this.transform.position.y + player.position.y, 0);

        Instantiate(prefab, dropPosition, Quaternion.identity);
    }


    private void RandomTimer()
    {
        RandomDrop();
        timer = Random.Range(3f, 4f);
    }

    public void StartShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        StartCoroutine(Shake());
    }

    public void TriggerShake()
    {
        StartShake(shakeDuration, shakeMagnitude);
    }

    IEnumerator Shake()
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = originalPos.x + Random.Range(-1f, 1f) * shakeMagnitude;
            float y = originalPos.y + Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 leftPos = new Vector3(transform.position.x - distance, this.transform.position.y, 0);
        Vector3 rightPos = new Vector3(transform.position.x + distance, this.transform.position.y, 0);
        Gizmos.DrawLine(leftPos, rightPos);

    }
}
