using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattleStartTrigger : MonoBehaviour
{
    [SerializeField] Enemy_Boss boss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            boss.startBattle = true;
            Destroy(gameObject);
        }
    }
}
