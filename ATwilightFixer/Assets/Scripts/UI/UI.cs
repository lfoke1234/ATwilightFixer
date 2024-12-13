using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour, ISaveManager
{
    public static UI instance;
    [SerializeField] int[] donSwitchInGameUI;

    // 게임오버 화면 관련
    [Header("End Screen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]

    // UI 목록
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject keyBindingUI;
    [SerializeField] private GameObject resolutionUI;
    [SerializeField] private GameObject inGameUI;

    // 툴팁 목록
    public UI_SkillTooltip skillTooltip;
    public UI_ItemTooltip itemTooltip;
    public UI_StatTooltip statTooltip;

    // 볼륨
    [SerializeField] private UI_VolumeSlider[] volumeSettings;

    // UI 전환 관련
    private GameObject[] uiPanels;
    private int currentUIPanelIndex;
    private bool isActiveUI;

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

        uiPanels = new GameObject[] { characterUI, skillTreeUI, optionsUI };
             
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
        //if (SceneManager.GetActiveScene().buildIndex != 0)
        if (!Array.Exists(donSwitchInGameUI, element => element == SceneManager.GetActiveScene().buildIndex))
            SwitchTo(inGameUI);

        characterUI.gameObject.SetActive(false);
        skillTreeUI.gameObject.SetActive(false);
        optionsUI.gameObject.SetActive(false);
        keyBindingUI.gameObject.SetActive(false);
        resolutionUI.gameObject.SetActive(false);

        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Debug.Log("Current Time.timeScale: " + Time.timeScale);
        // if (SceneManager.GetActiveScene().buildIndex != 0)
        if (!Array.Exists(donSwitchInGameUI, element => element == SceneManager.GetActiveScene().buildIndex))
        {
            if (IsActionTriggered("UI_Inventory"))
                SwitchWithKeyTo(characterUI);

            if (IsActionTriggered("UI_Skilltree"))
                SwitchWithKeyTo(skillTreeUI);

            if (IsActionTriggered("UI_Option") && keyBindingUI.gameObject.activeSelf == false && resolutionUI.gameObject.activeSelf == false)
                SwitchWithKeyTo(optionsUI);

            if (IsActionTriggered("UI_Next") && isActiveUI == true)
            {
                Debug.Log("Active");
                SwitchToNextUIPanel();
            }

            if (IsActionTriggered("UI_Previous") && isActiveUI == true)
                SwitchToPreviousUIPanel();

            if (IsActionTriggered("UI_Select") && isActiveUI == false)
                SelectCurrentUIPanel();

            if (Input.GetKeyDown(KeyCode.Escape) && keyBindingUI.gameObject.activeSelf)
            {
                AudioManager.instance.PlaySFX(5, null);
                keyBindingUI.gameObject.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Escape) && resolutionUI.gameObject.activeSelf)
            {
                AudioManager.instance.PlaySFX(5, null);
                resolutionUI.gameObject.SetActive(false);
            }

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

    // 현재 씬의 인덱스를 기준으로 In-Game UI를 활성화하거나 비활성화합니다.
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

    // 특정 UI 패널로 전환합니다.
    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;
            if (!fadeScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
            AudioManager.instance.PlaySFX(5, null);
        }

        if (GameManager.Instance != null)
        {
            if (_menu == inGameUI || _menu == null)
            {
                isActiveUI = false;
                GameManager.Instance.PausueGame(false);
            }
            else
            {
                isActiveUI = true;
                GameManager.Instance.PausueGame(true);
            }
        }
    }

    // 키 입력으로 UI 패널을 전환합니다.
    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        SwitchTo(_menu);
    }

    // 다음 UI 패널로 전환합니다.
    private void SwitchToNextUIPanel()
    {
        currentUIPanelIndex = (currentUIPanelIndex + 1) % uiPanels.Length;
        SwitchTo(uiPanels[currentUIPanelIndex]);
    }

    // 이전 UI 패널로 전환합니다.
    private void SwitchToPreviousUIPanel()
    {
        currentUIPanelIndex = (currentUIPanelIndex - 1 + uiPanels.Length) % uiPanels.Length;
        SwitchTo(uiPanels[currentUIPanelIndex]);
    }

    // 현재 UI 패널을 선택합니다.
    private void SelectCurrentUIPanel()
    {
        if (uiPanels[currentUIPanelIndex].activeSelf)
        {
            Debug.Log("Selected: " + uiPanels[currentUIPanelIndex].name);
        }
    }

    // In-Game UI가 필요한지 확인하고 전환합니다.
    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
                return;
        }

        SwitchTo(inGameUI);
    }

    // 엔딩 화면을 전환합니다.
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

    public void SaveandExit()
    {
        SaveManager.instance.SaveGame();
        Application.Quit();
    }

    public void TimeScale()
    {
        GameManager.Instance.PausueGame(false);
    }

    public GameObject GetInventory()
    {
        return characterUI;
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

    protected bool IsActionTriggered(string actionName)
    {
        var action = PlayerInputHandler.instance.GetAction(actionName);
        return action != null && action.triggered;
    }
}
