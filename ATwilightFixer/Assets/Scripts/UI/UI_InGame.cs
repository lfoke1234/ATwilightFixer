using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    /* private */
    PlayerStats playerStats;
    [SerializeField] private Slider slider;
    [SerializeField] private Slider slider2;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image slashImage;

    [SerializeField] private TextMeshProUGUI[] quickSlotTexts;

    #region QuickSlot Image
    [SerializeField] private Image quickSlot1Base;
    [SerializeField] private Image quickSlot1;
    [SerializeField] private Image quickSlot2Base;
    [SerializeField] private Image quickSlot2;
    [SerializeField] private Image quickSlot3Base;
    [SerializeField] private Image quickSlot3;
    [SerializeField] private Image quickSlot4Base;
    [SerializeField] private Image quickSlot4;
    [SerializeField] private Image quickSlot5Base;
    [SerializeField] private Image quickSlot5;
    #endregion

    [SerializeField] private TextMeshProUGUI currentGold;
    private SkillManager skills;

    private void Start()
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            // 플레이어의 체력 및 스태미나가 변경될 때 호출될 메서드를 설정합니다.
            playerStats.onHealthChanged += UpdateHealthUI;
            playerStats.onStaminaChanged += UpdateSPUI;
        }

        skills = SkillManager.instance; // 스킬 매니저를 참조합니다.
    }

    private void Update()
    {
        // 현재 보유한 골드를 표시합니다.
        currentGold.text = PlayerManager.instance.GetCurrency().ToString("#,#");

        // 대쉬 및 슬래시 스킬의 쿨다운을 업데이트합니다.
        CheckSkillCooldown(dashImage, skills.dash.coolDownTimer, skills.dash.coolDown);
        CheckSkillCooldown(slashImage, skills.slash.coolDownTimer, skills.slash.coolDown);

        #region Update QuickSlot
        // 각 퀵 슬롯의 아이콘과 텍스트를 업데이트합니다.
        UpdateQuickSlotIcon(0, quickSlot1);
        UpdateQuickSlotIcon(0, quickSlot1Base);

        UpdateQuickSlotIcon(1, quickSlot2);
        UpdateQuickSlotIcon(1, quickSlot2Base);

        UpdateQuickSlotIcon(2, quickSlot3);
        UpdateQuickSlotIcon(2, quickSlot3Base);

        UpdateQuickSlotIcon(3, quickSlot4);
        UpdateQuickSlotIcon(3, quickSlot4Base);

        UpdateQuickSlotIcon(4, quickSlot5);
        UpdateQuickSlotIcon(4, quickSlot5Base);

        // 퀵 슬롯에 아이템 사용 시 쿨다운 설정을 합니다.
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetCooldownOf(quickSlot1);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetCooldownOf(quickSlot2);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetCooldownOf(quickSlot3);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            SetCooldownOf(quickSlot4);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            SetCooldownOf(quickSlot5);

        // 퀵 슬롯의 쿨다운을 업데이트합니다.
        CheckCooldownOf(quickSlot1, Inventory.Instance.usableItemCooldown);
        CheckCooldownOf(quickSlot2, Inventory.Instance.usableItemCooldown);
        CheckCooldownOf(quickSlot3, Inventory.Instance.usableItemCooldown);
        CheckCooldownOf(quickSlot4, Inventory.Instance.usableItemCooldown);
        CheckCooldownOf(quickSlot5, Inventory.Instance.usableItemCooldown);
        #endregion
    }

    // 체력 슬라이더의 값을 업데이트합니다.
    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    // 스태미나 슬라이더의 값을 업데이트합니다.
    private void UpdateSPUI()
    {
        slider2.maxValue = playerStats.GetMaxStaminaValue();
        slider2.value = playerStats.currentStamina;
    }

    // 스킬 또는 퀵 슬롯의 쿨다운을 설정합니다.
    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    // 스킬 또는 퀵 슬롯의 쿨다운을 업데이트합니다.
    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }

    // 스킬의 쿨다운을 업데이트합니다.
    private void CheckSkillCooldown(Image _image, float _timer, float _cooldown)
    {
        _image.fillAmount = _timer / _cooldown;
    }

    // 퀵 슬롯 아이콘을 업데이트합니다.
    private void UpdateQuickSlotIcon(int slotIndex, Image quickSlotImage)
    {
        if (Inventory.Instance.usable != null && slotIndex < Inventory.Instance.usable.Count)
        {
            InventoryItem quickSlotItem = Inventory.Instance.usable[slotIndex];
            if (quickSlotItem != null && quickSlotItem.data != null && quickSlotItem.stackSize > 0)
            {
                quickSlotImage.sprite = quickSlotItem.data.icon;
                quickSlotImage.enabled = true;
                quickSlotTexts[slotIndex].text = quickSlotItem.stackSize.ToString();
                quickSlotTexts[slotIndex].enabled = true;
            }
            else
            {
                quickSlotImage.enabled = false;
                quickSlotTexts[slotIndex].enabled = false;
            }
        }
        else
        {
            quickSlotImage.enabled = false;
            quickSlotTexts[slotIndex].enabled = false;
        }
    }
}
