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

    public bool canUnlock;
    public bool unlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlotUI - " + skillName;
    }
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

    public void UnlockSkillSlot()
    {
        if (canUnlock == false)
        {
            Debug.Log("You need Clear Stage");
            return;
        }

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
            unlocked = value2;
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
