using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private float timer = 1.5f;
    
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckAndPickup(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckAndPickup(collision);
    }

    private void CheckAndPickup(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && timer <= 0)
        {
            myItemObject.PickupItem();
        }
    }
}
