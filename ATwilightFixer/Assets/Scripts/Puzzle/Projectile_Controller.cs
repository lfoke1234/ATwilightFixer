using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Projectile_Controller : MonoBehaviour
{
    [SerializeField] private string targetLayerName = "Player";

    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;

    private float timer = 3;
    private bool rotation;
    [SerializeField] private float speed;
    private CharacterStats stats;

    private void Update()
    {
        timer -= Time.deltaTime;

        if (rotation == false)
        {
            transform.Rotate(0, 0, speed * Time.deltaTime);
        }
    }

    public void SetupParabolicMotionToTarget(Vector3 targetPosition, float gravityScale, CharacterStats _stats)
    {
        stats = _stats;

        // 투사체와 목표 지점간의 벡터 계산
        Vector2 toTarget = targetPosition - transform.position;

        // 방향 벡터 계산
        float distance = toTarget.magnitude;
        Vector2 direction = toTarget.normalized;

        // 중력 설정과 초기 각도 설정
        // 최대 사거리와 적절한 포물선 운동을 위한 각도 계산
        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        float angle = 45 * Mathf.Deg2Rad;
        float velocity = Mathf.Sqrt((gravity * distance) / Mathf.Sin(2 * angle));

        // 벡터 설정후 포물선의 초기 속도를 결정
        Vector2 launchVelocity = new Vector2(velocity * Mathf.Cos(angle), velocity * Mathf.Sin(angle));
        launchVelocity.x *= Mathf.Sign(toTarget.x);

        rb.velocity = launchVelocity;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            stats.DoDamage(collision.GetComponent<CharacterStats>());

            if (targetLayerName == "Enemy")
                Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StuckInto(collision);
            rotation = true;
        }
        else if (timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void StuckInto(Collider2D collision)
    {
        // GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;

        Destroy(gameObject, Random.Range(3, 5));
    }
}
