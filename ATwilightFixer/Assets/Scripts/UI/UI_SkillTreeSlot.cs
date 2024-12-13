using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerClickHandler, ISaveManager
{
    private UI ui;
    [SerializeField] private Image skillImage; 
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedSkillColor;

    public bool canUnlock; // 스킬 해제 여부
    public bool unlocked; // 스킬 활성화 여부

    // 이 스킬을 활성화 하기 위해 열려있어야 되는 슬롯 목록
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    // 이 스킬을 활성하 하기 위해 잠겨있어야 되는 슬롯 목록
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlotUI - " + skillName;
    }

    //  버튼 클릭 리스너를 추가
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();
        skillImage.color = lockedSkillColor;
    }

    private void Update()
    {
        if (unlocked && skillImage.color != Color.white)
            skillImage.color = Color.white;
    }

    // 스킬 슬롯을 잠금 해제
    public void UnlockSkillSlot()
    {
        if (canUnlock == false) // 잠금 해제가 불가능하다면 종료
        {
            return;
        }

        // 다른 스킬은 잠금
        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                shouldBeLocked[i].unlocked = false;
                shouldBeLocked[i].skillImage.color = lockedSkillColor;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    // 스킬 슬롯을 클릭했을 때 툴팁을 표시
    public void OnPointerClick(PointerEventData eventData)
    {
        ui.skillTooltip.SetTooltip(skillDescription, skillName, skillImage);
    }

    public void LoadData(GameData _data)
    {
        if (_data.skillCanUnlock.TryGetValue(skillName, out bool value))
        {
            canUnlock = value;
        }

        if (_data.skillUnlocked.TryGetValue(skillName, out bool value2))
        {
            unlocked = value2; // 스킬의 잠금 해제 여부를 불러옵니다.
        }
    }

    public void SaveData(ref GameData _data)
    {
        if (_data.skillCanUnlock.TryGetValue(skillName, out bool value))
        {
            _data.skillCanUnlock.Remove(skillName);
            _data.skillCanUnlock.Add(skillName, canUnlock);
        }
        else
        {
            _data.skillCanUnlock.Add(skillName, canUnlock);
        }

        if (_data.skillUnlocked.TryGetValue(skillName, out bool value2))
        {
            _data.skillUnlocked.Remove(skillName);
            _data.skillUnlocked.Add(skillName, unlocked);
        }
        else
        {
            _data.skillUnlocked.Add(skillName, unlocked);
        }
    }
}
