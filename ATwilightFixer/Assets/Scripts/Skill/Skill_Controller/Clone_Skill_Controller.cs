using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
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
        // 공격이 종료되면 클론이 서서히 사라지도록 처리
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
        // 클론의 위치와 방향 설정 및 애니메이션 초기화
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
            // 공격 애니메이션을 이전과 다른 랜덤한 번호로 재생
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
        // 남은 공격 횟수가 있을 경우 애니메이션 재생, 없으면 종료
        if (exitCount > 0)
        {
            exitCount--;
            SetAnimation();
        }
        else
        {
            animator.SetBool("End", true);
            isEnd = true;
        }
    }


    private void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(0, null);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Arrow_Controller>() != null)
            {
                hit.GetComponent<Arrow_Controller>().FlipArrow();
            }

            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                // ObjectStats _targetObject = hit.GetComponent<ObjectStats>();

                if (_target != null)
                {
                    PlayerManager.instance.player.stats.DoDamage(_target);
                }
                // else if (_targetObject != null)
                // {
                //     PlayerManager.instance.player.stats.DoTrueDamage(_targetObject);
                // }

                ItemData_Equipment weaponData = Inventory.Instance.GetEquipment(EquipmentType.Weapon);

                if (weaponData != null)
                {
                    weaponData.ExcuteItemEffect();
                }
            }
        }
    }

    public void DestroyClone()
    {
        Destroy(gameObject);
    }
}
