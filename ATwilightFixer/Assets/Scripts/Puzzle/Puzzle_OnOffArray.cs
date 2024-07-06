using UnityEngine;

public class Puzzle_OnOffArray : PuzzleController
{
    [Header("Gemic")]
    [SerializeField] private GameObject[] targets;
    private bool isUsed;

    protected override void TriggerEvent()
    {
        base.TriggerEvent();

        if (!isUsed)
        {
            foreach (GameObject target in targets)
            {
                target.SetActive(!target.activeSelf);
            }
            isUsed = true;
        }
    }
}
