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

    // 캐릭터 스프라이트를 설정
    public void Setup(Sprite sprite)
    {
        switcher.SetImage(sprite);
    }

    #region Sprite Actions
    // 지정된 좌표에 캐릭터를 등장시키는 메서드
    public void Show(Vector2 coords)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        animator.SetTrigger("Show");
        rect.localPosition = coords;
    }

    // 캐릭터를 서서히 사라지게 하는 메서드
    public void Hide()
    {
        animator.SetTrigger("Hide");
    }

    // 지정된 좌표에 즉시 캐릭터를 등장시키는 메서드
    public void ShowInstantly(Vector2 coords)
    {
        rect.localPosition = coords;
        gameObject.SetActive(true);
    }

    // 캐릭터를 즉시 사라지게 하는 메서드
    public void HideInstantly()
    {
        gameObject.SetActive(false);
    }

    // 지정된 좌표로 캐릭터를 이동시키는 메서드
    public void Move(Vector2 coords, float speed)
    {
        StartCoroutine(MoveCoroutine(coords, speed));
    }

    #endregion
    // 지정된 좌표로 캐릭터를 서서히 이동시키는 코루틴
    public IEnumerator MoveCoroutine(Vector2 coords, float speed)
    {
        while (rect.localPosition.x != coords.x || rect.localPosition.y != coords.y)
        {
            rect.localPosition = Vector2.MoveTowards(rect.localPosition, coords, Time.deltaTime  * 1000f * speed);
            yield return new WaitForSeconds(0.01f);
        }
    }
    // 캐릭터의 스프라이트를 전환
    public void SwitchSprite(Sprite sprite)
    {
        if (switcher.GetImage() != sprite)
        {
            switcher.SwitchImage(sprite);
        }

    }
}
