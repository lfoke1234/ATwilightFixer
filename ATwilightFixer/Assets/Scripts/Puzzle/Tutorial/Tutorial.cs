using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public float visibleDistance = 5.0f;
    public float fadeDuration = 1.0f;
    private TextMeshPro textMesh;
    private Coroutine fadeCoroutine;

    private void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        SetAlpha(0.0f);
    }

    private void Update()
    {
        if (GameManager.Instance.isPlayCutScene) return;

        float distance = Vector2.Distance(PlayerManager.instance.player.transform.position, transform.position);
        bool shouldFadeIn = distance < visibleDistance && textMesh.color.a < 1.0f;
        bool shouldFadeOut = distance >= visibleDistance && textMesh.color.a > 0.0f;

        if (shouldFadeIn && fadeCoroutine == null)
        {
            fadeCoroutine = StartCoroutine(FadeTextAlpha(1.0f));
        }
        else if ((shouldFadeOut && fadeCoroutine == null) || GameManager.Instance.isPlayCutScene)
        {
            fadeCoroutine = StartCoroutine(FadeTextAlpha(0f));
        }
    }

    private IEnumerator FadeTextAlpha(float targetAlpha)
    {
        float startAlpha = textMesh.color.a;
        float timer = 0.0f;

        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            SetAlpha(alpha);
            timer += Time.deltaTime;
            yield return null;
        }
        SetAlpha(targetAlpha);
        fadeCoroutine = null;
    }

    private void SetAlpha(float alpha)
    {
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
    }
}
