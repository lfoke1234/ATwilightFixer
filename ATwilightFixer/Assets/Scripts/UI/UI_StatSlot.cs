using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    // 스텟 정보
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [SerializeField] private string statDescription;

    // 툴팁 위치 조정 관련 변수
    [SerializeField] private float xLimit = 960;
    [SerializeField] private float yLimit = 540;
    [SerializeField] private float xOffset = 150;
    [SerializeField] private float yOffset = 150;

    private void OnValidate()
    {
        gameObject.name = "Stat -" + statName;

        if (statNameText != null)
            statNameText.text = statName;
    }

    void Start()
    {
        UpdateStatValueUI();
        ui = GetComponentInParent<UI>();
    }

    // 스탯값 업데이트
    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();
        }
    }

    // 마우스가 스탯 슬롯 위에 올려졌을 때 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector2 mousePosition = Input.mousePosition;

        float newXoffset = 0;
        float newYoffset = 0;

        if (mousePosition.x > xLimit)
            newXoffset = -xOffset;
        else
            newXoffset = xOffset;

        if (mousePosition.y > yLimit)
            newYoffset = -yOffset;
        else
            newYoffset = yOffset;

        ui.statTooltip.ShowStatTooltip(statDescription);
        ui.statTooltip.transform.position = new Vector2(mousePosition.x + newXoffset, mousePosition.y + newYoffset);
    }

    // 마우스가 스탯 슬롯에서 벗어났을 때 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statTooltip.HideStatTooltip();
    }
}
