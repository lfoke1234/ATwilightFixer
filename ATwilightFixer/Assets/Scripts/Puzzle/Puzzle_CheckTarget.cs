using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Puzzle_CheckTarget : MonoBehaviour
{
    [SerializeField] private GameObject[] target;
    private GameObject spawner;

    private void Start()
    {
        spawner = GameObject.Find("MonsterSpawner");
    }

    private void Update()
    {
        if (target == null)
        {

        }
    }
}
