using UnityEngine;

public class Slash_Skill_Controller : MonoBehaviour
{
    [System.Serializable]
    public class SlashData
    {
        public float x;
        public float y;

        public SlashData(float vectorX, float vectorY)
        {
            this.x = vectorX;
            this.y = vectorY;
        }
    }

    private Player player;

    private SlashData firstSlashCol = new SlashData(1,1);
    private SlashData secondSlashCol = new SlashData(2,2);
    private SlashData thirdSlashCol = new SlashData(3,3);

    public SlashData currentSlash;

    public void PerformSlash(int slashType, int damage)
    {
        switch (slashType)
        {
            case 1:
                currentSlash = firstSlashCol;
                break;
            case 2:
                currentSlash = secondSlashCol;
                break;
            case 3:
                currentSlash = thirdSlashCol;
                break;
        }
        CheckSlashCollision(currentSlash, damage);
    }

    private void CheckSlashCollision(SlashData slashData, int damage)
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(currentSlash.x, currentSlash.y), 0);

        foreach (var hit in hits)
        {
            if (hit.GetComponent<Enemy>() != null || hit.GetComponent<WorldObject>() != null)
            {
                EnemyStats _targetEnemy = hit.GetComponent<EnemyStats>();
                ObjectStats _targetObject = hit.GetComponent<ObjectStats>();

                if (_targetEnemy != null)
                {
                    player.stats.DoDamageWithValue(_targetEnemy, damage);
                }
                else if (_targetObject != null)
                {
                    player.stats.DoTrueDamage(_targetObject);
                }

                ItemData_Equipment weaponData = Inventory.Instance.GetEquipment(EquipmentType.Weapon);

                if (weaponData != null)
                {
                    weaponData.ExcuteItemEffect();
                }
            }
        }
    }
}

