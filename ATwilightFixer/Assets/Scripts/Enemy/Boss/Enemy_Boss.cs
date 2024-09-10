using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy_Boss : Enemy
{
    [Header("Boss Skill Info")]
    public float trackCooldown = 5f;
    public float trackPlayerDistance;
    private float lastTrackUseTime = -Mathf.Infinity;

    public float flashCutCooldown = 5f;
    public GameObject clonePrefab;
    private float lastFlashCutUseTime = -Mathf.Infinity;


    #region States

    public Boss_IdleState idle { get; private set; }
    public Boss_BattleState battle { get; private set; }
    public Boss_BasicAttack attack { get; private set; }


    public Boss_TrackState track { get; private set; }
    public Boss_FlashCutState flashCut { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        idle = new Boss_IdleState(this, stateMachine, "Idle", this);
        battle = new Boss_BattleState(this, stateMachine, "Battle", this);
        attack = new Boss_BasicAttack(this, stateMachine, "Attack", this);
        
        track = new Boss_TrackState(this, stateMachine, "Track", this);
        flashCut = new Boss_FlashCutState(this, stateMachine, "FlashCut", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idle);
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool CanUseTrack()
    {
        return Time.time >= lastTrackUseTime + trackCooldown;
    }

    public void UseTrack()
    {
        lastTrackUseTime = Time.time;
    }

    public bool CanUseFlashCut()
    {
        return Time.time >= lastFlashCutUseTime + flashCutCooldown;
    }

    public void UseFlashCut()
    {
        lastFlashCutUseTime = Time.time;
    }

    public override void Die()
    {
        base.Die();
    }
}
