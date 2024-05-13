using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour, ISaveManager
{
    public static UI instance;
    [SerializeField] int[] donSwitchInGameUI;

    [Header("End Screen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;

    public UI_SkillTooltip skillTooltip;
    public UI_ItemTooltip itemTooltip;
    public UI_StatTooltip statTooltip;

    [SerializeField] private UI_VolumeSlider[] volumeSettings;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        SwitchTo(skillTreeUI);

        if (SceneManager.GetActiveScene().buildIndex != 0)
            fadeScreen.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
            SwitchTo(inGameUI);
            
        skillTreeUI.gameObject.SetActive(false);
        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Debug.Log("Current Time.timeScale: " + Time.timeScale);
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (Input.GetKeyDown(KeyCode.I))
                SwitchWithKeyTo(characterUI);

            if (Input.GetKeyDown(KeyCode.K))
                SwitchWithKeyTo(skillTreeUI);

            if (Input.GetKeyDown(KeyCode.Escape))
                SwitchWithKeyTo(optionsUI);
        }
        
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleInGameUIShow(scene.buildIndex);
    }

    private void HandleInGameUIShow(int currentSceneIndex)
    {
        if (Array.Exists(donSwitchInGameUI, element => element == currentSceneIndex))
        {
            if (inGameUI != null) inGameUI.SetActive(false);
        }
        else
        {
            if (inGameUI != null) inGameUI.SetActive(true);
        }
    }

    public void SwitchTo(GameObject _menu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;
            if (!fadeScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if(_menu != null)
        {
            _menu.SetActive(true);
           AudioManager.instance.PlaySFX(5, null);
        }

        if (GameManager.Instance != null)
        {
            if (_menu == inGameUI || _menu == null)
                GameManager.Instance.PausueGame(false);
            else
                GameManager.Instance.PausueGame(true);
        }

    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if(_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
                return;
        }

        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()
    {
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutine());
    }

    IEnumerator EndScreenCorutine()
    {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
        yield return new WaitForSeconds(1);
        restartButton.SetActive(true);
    }

    public void RestartGameButton()
    {
        endText.SetActive(false);
        restartButton.SetActive(false);
        GameManager.Instance.RestartScene();
        fadeScreen.FadeIn();

        PlayerManager.instance.player.ResetPlayer();
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, float> pair in _data.volumSettings)
        {
            foreach (UI_VolumeSlider item in volumeSettings)
            {
                if (item.parameter == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }

    }

    public void SaveData(ref GameData _data)
    {
        _data.volumSettings.Clear();

        foreach (UI_VolumeSlider item in volumeSettings)
        {
            _data.volumSettings.Add(item.parameter, item.slider.value);
        }
    }
}
