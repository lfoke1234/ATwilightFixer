using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Potal : MonoBehaviour
{
    Puzzle_Potal1 potal;
    [SerializeField] Transform target;

    private float timer;
    public bool DisableTarget1;

    private void Start()
    {
        potal = GetComponentInChildren<Puzzle_Potal1>();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (Vector2.Distance(transform.position, PlayerManager.instance.player.transform.position) < 5f)
        {
            timer = .5f;
        }

        if (timer >= 0)
        {
            DisableTarget1 = true;
        }
        else
        {
            DisableTarget1 = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && potal.DisableTarget2 == false)
        {
            PlayerManager.instance.player.SetZeroVelocity();
            collision.transform.position = target.position;
        }
    }
}