using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    private SpriteSwitcher switcher;
    private Animator animator;
    private RectTransform rect;

    private void Awake()
    {
        switcher = GetComponent<SpriteSwitcher>();
        animator = GetComponent<Animator>();
        rect = GetComponent<RectTransform>();
    }

    // ĳ���� ��������Ʈ�� ����
    public void Setup(Sprite sprite)
    {
        switcher.SetImage(sprite);
    }

    #region Sprite Actions
    // ������ ��ǥ�� ĳ���͸� �����Ű�� �޼���
    public void Show(Vector2 coords)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        animator.SetTrigger("Show");
        rect.localPosition = coords;
    }

    // ĳ���͸� ������ ������� �ϴ� �޼���
    public void Hide()
    {
        animator.SetTrigger("Hide");
    }

    // ������ ��ǥ�� ��� ĳ���͸� �����Ű�� �޼���
    public void ShowInstantly(Vector2 coords)
    {
        rect.localPosition = coords;
        gameObject.SetActive(true);
    }

    // ĳ���͸� ��� ������� �ϴ� �޼���
    public void HideInstantly()
    {
        gameObject.SetActive(false);
    }

    // ������ ��ǥ�� ĳ���͸� �̵���Ű�� �޼���
    public void Move(Vector2 coords, float speed)
    {
        StartCoroutine(MoveCoroutine(coords, speed));
    }

    #endregion
    // ������ ��ǥ�� ĳ���͸� ������ �̵���Ű�� �ڷ�ƾ
    public IEnumerator MoveCoroutine(Vector2 coords, float speed)
    {
        while (rect.localPosition.x != coords.x || rect.localPosition.y != coords.y)
        {
            rect.localPosition = Vector2.MoveTowards(rect.localPosition, coords, Time.deltaTime  * 1000f * speed);
            yield return new WaitForSeconds(0.01f);
        }
    }
    // ĳ������ ��������Ʈ�� ��ȯ
    public void SwitchSprite(Sprite sprite)
    {
        if (switcher.GetImage() != sprite)
        {
            switcher.SwitchImage(sprite);
        }

    }
}
