using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    /* private */ PlayerStats playerStats;
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
            playerStats.onHealthChanged += UpdateHealthUI;
            playerStats.onStaminaChanged += UpdateSPUI;
        }

        skills = SkillManager.instance;
    }

    

    private void Update()
    {
        currentGold.text = PlayerManager.instance.GetCurrency().ToString("#,#");

        if (Input.GetKeyDown(KeyCode.Z))
            SetCooldownOf(dashImage);
        if (Input.GetKeyDown(KeyCode.S))
            SetCooldownOf(slashImage);

        CheckCooldownOf(dashImage, skills.dash.coolDown);
        CheckCooldownOf(slashImage, skills.slash.coolDown);

        #region Update QuickSlot
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
        
        CheckCooldownOf(quickSlot1, Inventory.Instance.usableItemCooldown);
        CheckCooldownOf(quickSlot2, Inventory.Instance.usableItemCooldown);
        CheckCooldownOf(quickSlot3, Inventory.Instance.usableItemCooldown);
        CheckCooldownOf(quickSlot4, Inventory.Instance.usableItemCooldown);
        CheckCooldownOf(quickSlot5, Inventory.Instance.usableItemCooldown);
        #endregion
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }
    private void UpdateSPUI()
    {
        slider2.maxValue = playerStats.GetMaxStaminaValue();
        slider2.value = playerStats.currentStamina;
    }

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }

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
