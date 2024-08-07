using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "HealSP effect", menuName = "Data/Item Effect/HealSP effect")]
public class HealSP_Effect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float interval = 1f;

    public override void ExecuteEffect()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        playerStats.StartCoroutine(HealOverTime(playerStats));
    }

    private IEnumerator HealOverTime(PlayerStats playerStats)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            int healAmount = Mathf.RoundToInt(playerStats.GetMaxStaminaValue() * healPercent);
            playerStats.RecoveryStaminaBy(healAmount);

            yield return new WaitForSeconds(interval);
            timePassed += interval;
        }
    }
}
