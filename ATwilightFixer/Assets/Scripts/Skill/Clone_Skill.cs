using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    public GameObject CreateClone(Vector2 position, bool isFlip)
    {
        GameObject newClone = Instantiate(clonePrefab);
        // 適経 持失
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(position, isFlip);
        return newClone;
    }
}
