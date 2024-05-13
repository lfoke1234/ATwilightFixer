using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Explosion Effect", menuName = "Data/Item Effect/Explosion Effect")]
public class Explosion_Effect : ItemEffect
{
    [SerializeField] private int damage;
    [SerializeField] private float detectionRadius;

    public override void ExecuteEffect()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(playerStats.transform.position, detectionRadius);

        foreach (var hit in hitColliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                playerStats.DoDamageWithValue(_target, damage);
                playerStats.DoTrueDamage(_target, damage);
            }

            if (hit.GetComponent<WorldObject>() != null)
            {
                ObjectStats _targetO = hit.GetComponent<ObjectStats>();

                playerStats.DoTrueDamage(_targetO, damage);
            }
        }
    }
}
