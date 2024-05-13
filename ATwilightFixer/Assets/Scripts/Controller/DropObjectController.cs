using UnityEngine;

public class DropObjectController : MonoBehaviour
{
    [SerializeField] private int damage;
    private ParticleSystem particle;

    private void Start()
    {
        particle.GetComponentInChildren<ParticleSystem>();    

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Entity>())
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

            Destroy(gameObject);
        }
        else
        {
        }
    }
}
