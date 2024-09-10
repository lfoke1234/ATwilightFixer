using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Clone : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator animator;

    [SerializeField] private float colorLossingSpeed;

    private bool isEnd;
    [SerializeField] private int exitCount = 5;
    private int lastAttackNumber = -1;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 0.8f;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        if (isEnd)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLossingSpeed));

            if (sr.color.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetupClone(Vector2 newPosition, bool isFlip)
    {
        transform.position = newPosition;

        if (isFlip)
        {
            transform.Rotate(0f, 180f, 0f);
        }

        SetAnimation();
    }

    private void SetAnimation()
    {
        if (!isEnd)
        {
            int randomNum;
            do
            {
                randomNum = Random.Range(1, 3);
            } while (randomNum == lastAttackNumber);

            lastAttackNumber = randomNum;
            animator.SetInteger("AttackNumber", randomNum);
        }
    }

    private void AnimationTrigger()
    {
        if (exitCount > 0)
        {
            exitCount--;
            SetAnimation();
        }
        else if (exitCount <= 0)
        {
            isEnd = true;
        }
    }

    private void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(0, null);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats _target = hit.GetComponent<PlayerStats>();
                // ObjectStats _targetObject = hit.GetComponent<ObjectStats>();

                if (_target != null)
                {
                    PlayerManager.instance.player.stats.TakeDamage(10);
                }
                // else if (_targetObject != null)
                // {
                //     PlayerManager.instance.player.stats.DoTrueDamage(_targetObject);
                // }
            }
        }
    }
}
