using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitcher : MonoBehaviour
{

    public bool isSwitched = false;
    public Image image1;
    public Image image2;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // ���ο� ��� �̹����� �ε巴�� ��ȯ�ϴ�
    public void SwitchImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            image2.sprite = sprite;
            animator.SetTrigger("SwitchFirst");
        }
        else
        {
            image1.sprite = sprite;
            animator.SetTrigger("SwitchSecond");
        }
        isSwitched = !isSwitched;
    }

    // ���ο� ��� �̹����� ��� ����
    public void SetImage(Sprite sprite)
    {
        if (!isSwitched)
        {
            image1.sprite = sprite;
        }
        else
        {
            image2.sprite = sprite;
        }
    }

    public Sprite GetImage()
    {
        if (!isSwitched)
        {
            return image1.sprite;
        }
        else
        {
            return image2.sprite;
        }
    }
}
