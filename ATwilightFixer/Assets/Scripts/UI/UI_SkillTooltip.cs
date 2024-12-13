using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private Image skillIcon;

    // 툴팁의 내용을 설정
    public void SetTooltip(string _text, string _name, Image _icon)
    {
        skillName.text = _name;
        skillText.text = _text;
        skillIcon.sprite = _icon.sprite;
    }
}
