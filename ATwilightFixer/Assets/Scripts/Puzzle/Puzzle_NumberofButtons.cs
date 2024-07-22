using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_NumberofButtons : PuzzleController
{
    private Puzzle_NumberofButton[] buttons;
    [SerializeField] private GameObject door;
    private int activeCount;
    [SerializeField] private int count;

    protected override void Start()
    {
        base.Start();
        buttons = GetComponentsInChildren<Puzzle_NumberofButton>();

        foreach (var button in buttons)
        {
            button.OnButtonStateChanged += UpdateActiveCount;
        }
    }

    private void OnDestroy()
    {
        foreach (var button in buttons)
        {
            button.OnButtonStateChanged -= UpdateActiveCount;
        }
    }

    private void UpdateActiveCount(bool isActive)
    {
        if (isActive)
        {
            activeCount++;
        }
        else
        {
            activeCount--;
        }
    }

    protected override void TriggerEvent()
    {
        base.TriggerEvent();
        if (activeCount == count)
        {
            ClearPuzzle();
        }
        else
        {
            ResetPuzzle();
        }
    }

    private void ResetPuzzle()
    {
        Debug.Log("ResetCount");
        activeCount = 0;

        foreach (Puzzle_NumberofButton item in buttons)
        {
            item.isActive = false;
        }
    }

    private void ClearPuzzle()
    {
        door.SetActive(false);
    }
}
