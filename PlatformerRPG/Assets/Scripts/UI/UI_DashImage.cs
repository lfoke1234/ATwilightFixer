using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DashImage : MonoBehaviour
{
    [Header("Sprite")]
    [SerializeField] private Image baseImage;
    [SerializeField] private Image coolDownImage;

    [SerializeField] private Sprite dash1;
    [SerializeField] private Sprite dash2;
    [SerializeField] private Sprite dash3;

    [Header("Button")]
    [SerializeField] private UI_SkillTreeSlot dash1UnlockButton;
    [SerializeField] private UI_SkillTreeSlot dash2UnlockButton;
    [SerializeField] private UI_SkillTreeSlot dash3UnlockButton;

    private SkillManager skillManager;

    private void Start()
    {
        skillManager = SkillManager.instance;

        dash1UnlockButton.GetComponent<Button>().onClick.AddListener(ChangeSprite1);
        dash2UnlockButton.GetComponent<Button>().onClick.AddListener(ChangeSprite2);
        dash3UnlockButton.GetComponent<Button>().onClick.AddListener(ChangeSprite3);
    }

    private void CheckAndChangeSprite()
    {
        if (skillManager.dash.CheckCurrentDash() == 1)
        {
            baseImage.sprite = dash1;
            coolDownImage.sprite = dash1;
        }
        else if (skillManager.dash.CheckCurrentDash() == 2)
        {
            baseImage.sprite = dash2;
            coolDownImage.sprite = dash2;
        }
        else if (skillManager.dash.CheckCurrentDash() == 3)
        {
            baseImage.sprite = dash3;
            coolDownImage.sprite = dash3;
        }

    }

    private void ChangeSprite1()
    {
        baseImage.sprite = dash1;
        coolDownImage.sprite = dash1;
    }
    private void ChangeSprite2()
    {
        baseImage.sprite = dash2;
        coolDownImage.sprite = dash2;
    }
    private void ChangeSprite3()
    {
        baseImage.sprite = dash3;
        coolDownImage.sprite = dash3;
    }
}
