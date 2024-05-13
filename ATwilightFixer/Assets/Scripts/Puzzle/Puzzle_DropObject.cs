using UnityEngine;
using UnityEngine.UI;

public class Puzzle_DropObject : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField]private float distance;
    [SerializeField]private Transform check;
    [SerializeField]private LayerMask isPlayer;

    private bool drop;
    private bool doDamage;

    private float gravity = 9.81f;
    private float currentSpeed = 0f;

    private void Awake()
    {
        drop = false;
        doDamage = false;
    }

    private void Update()
    {
        Drop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats player = collision.GetComponent<PlayerStats>();

        if (player != null && !doDamage)
        {
            int playerArmor = player.armor.GetValue();
            player.TakeDamage(damage - playerArmor);
            doDamage = true;
        }
    }

    private bool DropCheck() => Physics2D.Raycast(check.position, Vector2.down, distance, isPlayer);

    private void Drop()
    {
        if (DropCheck())
            drop = true;

        if (drop)
        {
            currentSpeed += gravity * Time.deltaTime;
            transform.Translate(Vector3.down * currentSpeed * Time.deltaTime, Space.World);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(check.position, check.position + Vector3.down * distance);
    }
}
