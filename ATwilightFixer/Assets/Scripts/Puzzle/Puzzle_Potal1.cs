using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_Potal1 : MonoBehaviour
{
    Puzzle_Potal potal;
    [SerializeField] Transform target;

    private float timer;
    public bool DisableTarget2;

    private void Start()
    {
        potal = GetComponentInParent<Puzzle_Potal>();
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
            DisableTarget2 = true;
        }
        else
        {
            DisableTarget2 = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && potal.DisableTarget1 == false)
        {
            PlayerManager.instance.player.SetZeroVelocity();
            collision.transform.position = target.position;
        }
    }
}