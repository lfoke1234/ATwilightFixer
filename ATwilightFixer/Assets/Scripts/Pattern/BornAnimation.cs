using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BornAnimation : MonoBehaviour
{
    private bool doDamage;
    private int damage;

    [SerializeField] private float lifeTime = 3.0f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
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


    public void SetDamage(int value) => damage = value;
}
