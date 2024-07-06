using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "Using Key effect", menuName = "Data/Item Effect/Using Key effect")]
public class ItemEffect_UsingKey : ItemEffect
{
    [SerializeField] private float searchRadius = 5.0f;
    [SerializeField] private LayerMask searchLayerMask;

    public override void ExecuteEffect()
    {
        FindComponentsInRange<ItemEffect_UsingKeyTarget>(PlayerManager.instance.player.transform.position, searchRadius);
    }

    private void FindComponentsInRange<T>(Vector2 center, float radius) where T : Component
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(center, radius, searchLayerMask);

        foreach (Collider2D collider in colliders)
        {
            T component = collider.GetComponent<T>();
            if (component != null)
            {
                collider.gameObject.SetActive(false);
            }
        }
    }
}
