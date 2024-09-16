using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Heal_Statue : MonoBehaviour
{
    private Player player;

    private bool heal;
    private bool hasHealed = false;

    [SerializeField] private float distance;

    [SerializeField] private float healPersnet;
    [Space]
    [SerializeField] private bool isContinue;
    [SerializeField] private float time;
    private float timer;

    private void Start()
    {
        player = PlayerManager.instance.player;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (Vector2.Distance(player.transform.position, transform.position) < distance)
        {
            heal = true;
        }
        else
        {
            heal = false;
        }

        if (heal == true && isContinue == false)
        {
            if (hasHealed == false)
            {
                int hp = player.stats.GetMaxHealthValue();
                player.stats.IncreaseHealthBy((int)(hp * (healPersnet / 100)));
                hasHealed = true;
            }
        }
        else if (heal == true && isContinue == true)
        {
            int hp = player.stats.GetMaxHealthValue();
            
            if (timer <= 0)
            {
                player.stats.IncreaseHealthBy((int)(hp * (healPersnet / 100)));
                timer = time;
            }
        }
    }
}
