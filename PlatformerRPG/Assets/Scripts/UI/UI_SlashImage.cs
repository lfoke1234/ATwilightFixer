using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SlashImage : MonoBehaviour
{
    [Header("Sprite")]
    [SerializeField] private Image baseImage;
    [SerializeField] private Image coolDownImage;

    [SerializeField] private Sprite slash1;
    [SerializeField] private Sprite slash2;
    [SerializeField] private Sprite slash3;

    [Header("Button")]
    [SerializeField] private UI_SkillTreeSlot slash1UnlockButton;
    [SerializeField] private UI_SkillTreeSlot slash2UnlockButton;
    [SerializeField] private UI_SkillTreeSlot slash3UnlockButton;

    private SkillManager skillManager;

    private void Start()
    {
        skillManager = SkillManager.instance;

        slash1UnlockButton.GetComponent<Button>().onClick.AddListener(ChangeSprite1);
        slash2UnlockButton.GetComponent<Button>().onClick.AddListener(ChangeSprite2);
        slash3UnlockButton.GetComponent<Button>().onClick.AddListener(ChangeSprite3);
    }

    private void CheckAndChangeSprite()
    {
        if (skillManager.slash.CheckCurrentSlash() == 1)
        {
            baseImage.sprite = slash1;
            coolDownImage.sprite = slash1;
        }
        else if (skillManager.slash.CheckCurrentSlash() == 2)
        {
            baseImage.sprite = slash2;
            coolDownImage.sprite = slash2;
        }
        else if (skillManager.slash.CheckCurrentSlash() == 3)
        {
            baseImage.sprite = slash3;
            coolDownImage.sprite = slash3;
        }
    }

    private void ChangeSprite1()
    {
        baseImage.sprite = slash1;
        coolDownImage.sprite = slash1;
    }
    private void ChangeSprite2()
    {
        baseImage.sprite = slash2;
        coolDownImage.sprite = slash2;
    }
    private void ChangeSprite3()
    {
        baseImage.sprite = slash3;
        coolDownImage.sprite = slash3;
    }
}
