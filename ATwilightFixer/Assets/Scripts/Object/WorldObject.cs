using System;
using System.Collections;
using UnityEngine;

public class WorldObject : Entity
{
    #region Component
    private CharacterStats stat;
    #endregion

    [Header("Respawn")]
    [SerializeField] private bool isRespawn;
    [SerializeField] private float respawnTime;

    protected override void Start()
    {
        base.Start();
        stat = GetComponent<CharacterStats>();
    }

    public override void Die()
    {
        if (isRespawn)
        {
            StartCoroutine(RespawnObject());
        }
        else if (!isRespawn)
            DeadEvent();
    }

    protected virtual void DeadEvent()
    {
        Destroy(gameObject);
    }

    public void StartRespawnCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    private IEnumerator RespawnObject()
    {
        stat.currentHiddenHealth = stat.GetHiddenHealthValue();
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(respawnTime);
        stat.isDead = false;
        GetComponent<SpriteRenderer>().enabled = true;
    }


    protected override void OnDrawGizmos()
    {
    }
}