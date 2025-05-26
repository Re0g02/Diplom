using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private enum GameState
    {
        Play,
        Paused,
        Gameover,
        LevelUp
    }

    private GameState currentGameState;
    private GameState previosGameState;
    [Header("Screens")]
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _resultScreen;
    [SerializeField] private GameObject _gameUIScreen;
    [SerializeField] private GameObject _levelUpScreen;

    [Header("Pause Screen")]
    [SerializeField] private TextMeshProUGUI _currentHealthText;
    [SerializeField] private TextMeshProUGUI _currentRecoveryText;
    [SerializeField] private TextMeshProUGUI _currentMoveSpeedText;
    [SerializeField] private TextMeshProUGUI _currentMightText;
    [SerializeField] private TextMeshProUGUI _currentProjectileSpeedText;
    [SerializeField] private TextMeshProUGUI _currentMagnetText;

    [Header("GameOver Screen")]
    [SerializeField] private Image _playerIcon;
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private TextMeshProUGUI _playerLevelText;
    [SerializeField] private TextMeshProUGUI _playerTimerText;
    [SerializeField] private List<Image> _playerWeapons = new List<Image>(6);
    [SerializeField] private List<Image> _playerItems = new List<Image>(6);
    private bool isGameOver = false;

    [Header("Play")]
    [SerializeField] private float _timeLimit;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image expBar;
    [SerializeField] private TextMeshProUGUI expText;
    private float currentTime;

    [Header("Damage Text")]
    [SerializeField] private GameObject damageText;
    [SerializeField] private Camera referenceCamera;
    [SerializeField] private AudioClip lvlSound;
    [SerializeField] private AudioClip loseSound;

    private bool isChoosingUpdate = false;
    private AudioSource audioSource;
    private AudioSource backgroundMusic;
    public GameObject playerObject;

    public String currentHealthText { get => _currentHealthText.text; set => _currentHealthText.text = value; }
    public String currentRecoveryText { get => _currentRecoveryText.text; set => _currentRecoveryText.text = value; }
    public String currentMoveSpeedText { get => _currentMoveSpeedText.text; set => _currentMoveSpeedText.text = value; }
    public String currentMightText { get => _currentMightText.text; set => _currentMightText.text = value; }
    public String currentProjectileSpeedText { get => _currentProjectileSpeedText.text; set => _currentProjectileSpeedText.text = value; }
    public String currentMagnetText { get => _currentMagnetText.text; set => _currentMagnetText.text = value; }
    public bool IsGameOver { get => isGameOver; private set => isGameOver = value; }
    public bool IsChoosingUpdate { get => isChoosingUpdate; private set => isChoosingUpdate = value; }
    public float HealthBarFillAmount { set => healthBar.fillAmount = value; }
    public float ExpBarFillAmount { set => expBar.fillAmount = value; }
    public String ExpBarText { set => expText.text = value; }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        backgroundMusic = FindFirstObjectByType<Camera>().GetComponent<AudioSource>();
        Time.timeScale = 1f;
        if (instance == null) instance = this;
        else Destroy(gameObject);
        HideScreens();
        _gameUIScreen.SetActive(true);
    }
    void Update()
    {

        switch (currentGameState)
        {
            case GameState.Play:
                PauseInput();
                UpdateCurrentTime();
                break;
            case GameState.Paused:
                PauseInput();
                break;
            case GameState.Gameover:
                if (!isGameOver)
                    DisplayResults();
                break;
            case GameState.LevelUp:
                if (!isChoosingUpdate)
                    StartLevelUp();
                break;
            default:
                Debug.Log("Unprocessable state");
                break;
        }
    }

    public void PauseGame()
    {
        if (currentGameState == GameState.Paused)
            return;
        backgroundMusic.Pause();
        previosGameState = currentGameState;
        ChangeGameState(GameState.Paused);
        _pauseScreen.SetActive(true);
        _gameUIScreen.SetActive(false);
        Time.timeScale = 0f;
    }

    public void unPauseGame()
    {
        if (currentGameState != GameState.Paused)
            return;
        backgroundMusic.UnPause();
        ChangeGameState(previosGameState);
        _pauseScreen.SetActive(false);
        _gameUIScreen.SetActive(true);
        Time.timeScale = 1f;
    }

    private void ChangeGameState(GameState state)
    {
        currentGameState = state;
    }

    private void HideScreens()
    {
        _pauseScreen.SetActive(false);
        _resultScreen.SetActive(false);
        _levelUpScreen.SetActive(false);
    }

    private void PauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (currentGameState == GameState.Paused)
                unPauseGame();
            else PauseGame();
    }

    public void GameOver()
    {
        backgroundMusic.Stop();
        audioSource.clip = loseSound;
        audioSource.Play();
        _playerTimerText.text = _timerText.text;
        ChangeGameState(GameState.Gameover);
    }

    private void DisplayResults()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        _resultScreen.SetActive(true);
        _gameUIScreen.SetActive(false);
    }

    public void ChangePlayerUIOnGameOverScreen(PlayerDataScriptableObject playerStats)
    {
        _playerIcon.sprite = playerStats.Icon;
        _playerIcon.preserveAspect = true;
        _playerName.text = playerStats.Name;
    }
    public void ChangePlayerLevelOnGameOverScreen(int Level)
    {
        _playerLevelText.text = Level.ToString();
    }

    public void ChangePlayerInventoryOnGameOverScreen(List<Image> weaponsImage, List<Image> itemsImage)
    {
        for (int i = 0; i < weaponsImage.Count; i++)
        {
            if (weaponsImage[i].sprite)
            {
                _playerWeapons[i].enabled = true;
                _playerWeapons[i].sprite = weaponsImage[i].sprite;
            }
        }
        for (int i = 0; i < itemsImage.Count; i++)
        {
            if (itemsImage[i].sprite)
            {
                _playerItems[i].enabled = true;
                _playerItems[i].sprite = itemsImage[i].sprite;
            }
        }
    }

    void UpdateCurrentTime()
    {
        currentTime += Time.deltaTime;
        UpdateTimer();
        if (currentTime >= _timeLimit) GameOver();
    }

    void UpdateTimer()
    {
        var minutes = Mathf.FloorToInt(currentTime / 60);
        var seconds = Mathf.FloorToInt(currentTime % 60);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void LevelUp()
    {
        audioSource.clip = lvlSound;
        audioSource.Play();
        ChangeGameState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void StartLevelUp()
    {
        isChoosingUpdate = true;
        Time.timeScale = 0f;
        _gameUIScreen.SetActive(false);
        _levelUpScreen.SetActive(true);
    }

    public void EndLevelUp()
    {
        isChoosingUpdate = false;
        Time.timeScale = 1f;
        _levelUpScreen.SetActive(false);
        _gameUIScreen.SetActive(true);
        ChangeGameState(GameState.Play);
    }

    public static void GenerateDamageText(string text, Transform target, float radius, float duration = 1f, float speed = 1f)
    {
        if (!instance.referenceCamera) instance.referenceCamera = Camera.main;

        instance.StartCoroutine(instance.GenerateDamageTextCorotune(text, target, radius, duration, speed));
    }

    IEnumerator GenerateDamageTextCorotune(string text, Transform target, float radius, float duration = 1f, float speed = 50f)
    {
        var textObj = Instantiate(damageText);
        var textObjTMPro = textObj.GetComponent<TextMeshProUGUI>();
        textObjTMPro.text = text;
        Vector3 textPos = (UnityEngine.Random.insideUnitSphere * radius);
        var textObjRect = textObj.GetComponent<RectTransform>();
        textObjRect.position = referenceCamera.WorldToScreenPoint(textPos + target.position);

        textObj.transform.SetParent(instance._gameUIScreen.transform);

        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0;
        float yOffset = 0;
        Vector3 currentTargetPos = new Vector3();
        while (t < duration)
        {
            yield return w;
            t += Time.deltaTime;
            textObjTMPro.color = new Color(textObjTMPro.color.r, textObjTMPro.color.g, textObjTMPro.color.b, 1 - t / duration);
            yOffset += speed * Time.deltaTime;
            if (target) currentTargetPos = target.position;
            textObjRect.position = referenceCamera.WorldToScreenPoint(textPos + currentTargetPos + new Vector3(0, yOffset));
        }
        Destroy(textObj.gameObject);
    }
}
