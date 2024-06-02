using System.Threading;
using UnityEngine;

public class DropObjectController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private CapsuleCollider2D cd;

    [SerializeField] private ParticleSystem particle;
    private bool hasDamaged;
    [SerializeField] private int damage;
    private float timer = 5f;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        cd = GetComponent<CapsuleCollider2D>();

        cd.enabled = false;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 3.5f)
            cd.enabled = true;

        if (timer <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layerIndex = collision.gameObject.layer;
        string layerName = LayerMask.LayerToName(layerIndex);

        if (collision.GetComponent<Entity>() && hasDamaged == false)
        {
            Enemy targetEnemy = collision.GetComponent<Enemy>();
            Player targetPlayer = collision.GetComponent<Player>();

            if (targetEnemy != null)
            {
                targetEnemy.stats.TakeDamage(damage);
            }
            if (targetPlayer != null)
            {
                targetPlayer.stats.TakeDamage(damage);
            }

            hasDamaged = true;
        }

        if (layerName == "Ground" || layerName == "BrokenWall")
        {
            hasDamaged = true;
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            sr.enabled = false;
            particle.Play();
        }
    }
}
