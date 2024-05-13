using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private float originalTimeScale = 1.0f;

    [Header("Pop Up Text")]
    [SerializeField] private GameObject popUpTextPrefab;

    [Header("Screen Shake")]
    private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultiplier;
    [SerializeField] private Vector3 shakePower;

    [Header("Flash FX")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originMat;

    [Header("Aliment Colors")]
    [SerializeField] private Color[] poisonColor;
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] shockColor;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFX;
    [SerializeField] private GameObject spetialHitFX;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originMat = sr.material;
        player = PlayerManager.instance.player;
        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    public void CreatePopUpText(string _text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(1, 3);

        Vector3 positionOffset = new Vector3(randomX, randomY, 0);

        GameObject newText = Instantiate(popUpTextPrefab, transform.position, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;
    }

    #region Screen Shake
    public void ScreenShake(Vector3 _shakePower)
    {
        ScreenShake(_shakePower, 0.2f);
    }

    public void ScreenShake(Vector3 _shakePower, float _shakeMultiplier)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * _shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    #endregion

    public void GameSpeedControll(float newSpeed, float duration)
    {
        originalTimeScale = Time.timeScale;

        Time.timeScale = newSpeed;

        StartCoroutine(ResetGameSpeedAfterDelay(duration));
    }

    private IEnumerator ResetGameSpeedAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = originalTimeScale;
    }


    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        //Color currentColor = sr.color;
        //sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);

        //sr.color = currentColor;
        sr.material = originMat;

    }

    private void GrayColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.gray;
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void CreatHitFX(Transform _target, bool _isSpecial)
    {
        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-0.5f, 0.5f);
        float yPosition = Random.Range(-0.5f, 0.5f);

        GameObject hitPrefab = hitFX;

        if (_isSpecial)
        {
            hitPrefab = spetialHitFX;
        }

        GameObject newHitfx = Instantiate(hitPrefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity, _target);

        if (_isSpecial == false)
            newHitfx.transform.Rotate(new Vector3(0, 0, zRotation));
        else
            newHitfx.transform.localScale = new Vector3(GetComponent<Entity>().facingDir, 1, 1);

        Destroy(newHitfx, 0.5f);
    }

    #region AilmentColorChange
    public void poisonFxFor(float _second)
    {
        InvokeRepeating("posionColorFx", 0, 0.3f);
        Invoke("CancelColorChange", _second);
    }

    public void ChillFxFor(float _second)
    {
        InvokeRepeating("ChillColorFx", 0, 0.3f);
        Invoke("CancelColorChange", _second);
    }

    public void ShockFxFor(float _second)
    {
        InvokeRepeating("ShockColorFx", 0, 0.3f);
        Invoke("CancelColorChange", _second);
    }

    public void posionColorFx()
    {
        
        if (sr.color != poisonColor[0])
            sr.color = poisonColor[0];
        else
            sr.color = poisonColor[1];
    }

    public void ChillColorFx()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }

    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }
    #endregion
}
