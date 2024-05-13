using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_SPBar : MonoBehaviour
{
    private Entity entity;
    private CharacterStats myStats;
    private RectTransform myTransform;
    private Slider slider;

    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        UpdateSPUI();
    }

    private void UpdateSPUI()
    {
        slider.maxValue = myStats.GetMaxStaminaValue();
        slider.value = myStats.currentStamina;
    }

    public void FlipUI()
    {
        myTransform.Rotate(0, 180, 0);
    }

    private void OnDisable()
    {
        entity.onFliped -= FlipUI;
        myStats.onHealthChanged -= UpdateSPUI;
    }
}