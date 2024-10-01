using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionGround : MonoBehaviour
{
    [SerializeField] private float radious;
    [SerializeField] private float force;

    public void ExplodeInTimeLine()
    {
        Explode(this.transform.position, radious, force);
        
    }

    private void Explode(Vector2 explosionPoint, float explosionRadius, float explosionForce)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPoint, explosionRadius);

        foreach (Collider2D collider in colliders)
        {
            ExplosionGroundPices groundPiece = collider.GetComponent<ExplosionGroundPices>();

            if (groundPiece != null)
            {
                Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    if (rb.bodyType == RigidbodyType2D.Kinematic)
                    {
                        rb.bodyType = RigidbodyType2D.Dynamic;
                        rb.gravityScale = 3;
                    }

                    Vector2 direction = (Vector2)collider.transform.position - explosionPoint;

                    float distance = direction.magnitude;
                    float forceMagnitude = explosionForce * (1 - (distance / explosionRadius));

                    rb.AddForce(direction.normalized * forceMagnitude, ForceMode2D.Impulse);
                }
            }
        }
    }
}
