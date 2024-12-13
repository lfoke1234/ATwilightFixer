using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private string targetLayerName = "Player";

    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool canMove;
    [SerializeField] private bool flipped;

    private CharacterStats stats;

    private void Update()
    {
        if (canMove)
            rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }

    public void SetArrow(float xVelocity, CharacterStats stats)
    {
        this.xVelocity = xVelocity;
        this.stats = stats;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            stats.DoDamage(collision.GetComponent<CharacterStats>());
            StackInto(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StackInto(collision);
        }
    }

    private void StackInto(Collider2D collision)
    {
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;

        Destroy(gameObject, Random.Range(3, 5));
    }

    public void FlipArrow()
    {
        if (flipped)
            return;

        xVelocity = xVelocity * -1;
        flipped = true;
        transform.Rotate(0, 180, 0);

        targetLayerName = "Enemy";
    }
}
