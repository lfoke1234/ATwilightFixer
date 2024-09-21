using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_OnOffButton : PuzzleController
{
    [Header("Gemic")]
    [SerializeField] private GameObject target;
    [SerializeField] private Sprite off;
    [SerializeField] private Sprite on;
    private SpriteRenderer sp;

    protected override void Start()
    {
        base.Start();
        sp = GetComponent<SpriteRenderer>();
    }

    protected override void TriggerEvent()
    {
        if (target.activeSelf)
        {
            sp.sprite = off;
            target.SetActive(false);
        }
        else
        {
            sp.sprite = on;
            target.SetActive(true);
        }
    }

}
