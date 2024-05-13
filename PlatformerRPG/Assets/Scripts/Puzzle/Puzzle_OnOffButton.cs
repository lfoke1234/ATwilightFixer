using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_OnOffButton : PuzzleController
{
    [Header("Gemic")]
    [SerializeField] private GameObject target;

    protected override void TriggerEvent()
    {
        if (target.activeSelf)
        {
            target.SetActive(false);
        }
        else
        {
            target.SetActive(true);
        }
    }

}
