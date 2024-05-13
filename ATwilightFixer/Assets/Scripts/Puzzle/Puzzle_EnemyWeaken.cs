using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_EnemyWeaken : PuzzleController
{
    [Header("Change Enemy")]
    [SerializeField] private GameObject orignal;
    [SerializeField] private GameObject target;

    [Header("Decrease Stat")]
    [SerializeField] private GameObject targetEnemy;
    [SerializeField] private int decreaseHealt;
    [SerializeField] private int decreaseArmor;
    [SerializeField] private int decreaseDamage;

    [SerializeField]private bool changeEnemy;
    private bool isTrigged;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void TriggerEvent()
    {
        base.TriggerEvent();

        if (isTrigged)
        {
            CheckWeakenType();
        }
        else
            return;

    }

    private void CheckWeakenType()
    {
        if (!changeEnemy)
            targetEnemy.GetComponent<EnemyStats>().DecreaseStatWithValue(decreaseHealt, decreaseArmor, decreaseDamage);
        else
            ChangeEnemy(orignal, target);
    }

    private void ChangeEnemy(GameObject _orignal, GameObject _target)
    {
        _orignal.SetActive(false);
        _target.SetActive(true);
        
    }

    protected override void OnDrawGizmos()
    {
    }
}
