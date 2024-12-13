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

    public float spawnThunderCooldown = 5f;
    public GameObject thunderPrefab;
    [SerializeField] private Transform thunderSpawnPos;
    [SerializeField] private float thunderSpawnDistance;
    private float lastThunderTime = -Mathf.Infinity;

    
    public float spawnEnemiesCooldown = 5f;
    public GameObject[] enemies;
    [SerializeField] private float enemiesSpawnDistance;
    private float lastEnemiesTime = -Mathf.Infinity;

    public bool startBattle;
    [SerializeField] private GameObject end;

    #region States

    public Boss_IdleState idle { get; private set; }
    public Boss_BattleState battle { get; private set; }
    public Boss_BasicAttack attack { get; private set; }


    public Boss_TrackState track { get; private set; }
    public Boss_FlashCutState flashCut { get; private set; }
    public Boss_SpawnThunderState spawnThunder { get; private set; }
    public Boss_SpawnEneiesState spawnEnemies { get; private set; }

    public Boss_DeadState dead { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        idle = new Boss_IdleState(this, stateMachine, "Idle", this);
        battle = new Boss_BattleState(this, stateMachine, "Battle", this);
        attack = new Boss_BasicAttack(this, stateMachine, "Attack", this);
        
        track = new Boss_TrackState(this, stateMachine, "Track", this);
        flashCut = new Boss_FlashCutState(this, stateMachine, "FlashCut", this);
        spawnThunder = new Boss_SpawnThunderState(this, stateMachine, "Thunder", this);
        spawnEnemies = new Boss_SpawnEneiesState(this, stateMachine, "Enemies", this);

        dead = new Boss_DeadState(this, stateMachine, "Dead", this);
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

    #region Skill
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

    public void UseSpawnThunder()
    {
        lastThunderTime = Time.time;
        SpawnThunder();
    }

    public bool CanUseThunder()
    {
        return Time.time >= lastThunderTime + spawnThunderCooldown;
    }

    public void UseSpawnEnemies()
    {
        lastEnemiesTime = Time.time;
        SpawnEnemies();
    }

    public bool CanUseSpawnEnemies()
    {
        return Time.time >= lastEnemiesTime + spawnEnemiesCooldown;
    }
    #endregion

    // 보스위치에서 지정된 범위 내에 번개 소환
    private void SpawnThunder()
    {
        for (int i = 0; i < 4; i++)
        {
            float randomPos = Random.Range(-thunderSpawnDistance, thunderSpawnDistance);
            Vector2 spawnPosition = new Vector2(thunderSpawnPos.position.x + randomPos, thunderSpawnPos.position.y);
            Instantiate(thunderPrefab, spawnPosition, Quaternion.identity, null);
        }
    }

    // 보스위치에서 지정된 범위 내에 적 소환
    private void SpawnEnemies()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            float randomPos = Random.Range(-thunderSpawnDistance, thunderSpawnDistance);
            Vector2 spawnPosition = new Vector2(thunderSpawnPos.position.x + randomPos, thunderSpawnPos.position.y);
            Instantiate(enemies[i], spawnPosition, Quaternion.identity, null);
        }
    }

    public override void Die()
    {
        base.Die();
        end.SetActive(true);
        stateMachine.ChangeState(dead);
        Destroy(gameObject, 2f);
    }
}
