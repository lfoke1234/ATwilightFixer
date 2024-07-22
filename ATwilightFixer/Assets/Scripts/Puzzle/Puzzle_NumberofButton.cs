using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_NumberofButton : PuzzleController
{
    public bool isActive = false;
    public delegate void ButtonStateChanged(bool isActive);
    public event ButtonStateChanged OnButtonStateChanged;

    protected override void TriggerEvent()
    {
        base.TriggerEvent();
        isActive = !isActive;

        OnButtonStateChanged?.Invoke(isActive);
    }
}
