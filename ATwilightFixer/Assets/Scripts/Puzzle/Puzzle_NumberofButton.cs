using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_NumberofButton : PuzzleController
{
    public bool isActive = false;
    public delegate void ButtonStateChanged(bool isActive);
    public event ButtonStateChanged OnButtonStateChanged;

    [SerializeField] private Sprite on;
    [SerializeField] private Sprite off;


    protected override void TriggerEvent()
    {
        base.TriggerEvent();
        isActive = !isActive;

        if(isActive)
        {
            spriteRenderer.sprite = on;
        }
        else
        {
            spriteRenderer.sprite = off;
        }

        OnButtonStateChanged?.Invoke(isActive);
    }
}
