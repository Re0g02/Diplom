using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private enum GameState
    {
        Play,
        Paused,
        Gameover
    }

    private GameState currentGameState;
    private GameState previosGameState;
    [Header("Screens")]
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _resultScreen;
    [SerializeField] private GameObject _gameUIScreen;
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
    [SerializeField] private List<Image> _playerWeapons = new List<Image>(6);
    [SerializeField] private List<Image> _playerItems = new List<Image>(6);

    private bool isGameOver = false;

    public String currentHealthText { get => _currentHealthText.text; set => _currentHealthText.text = value; }
    public String currentRecoveryText { get => _currentRecoveryText.text; set => _currentRecoveryText.text = value; }
    public String currentMoveSpeedText { get => _currentMoveSpeedText.text; set => _currentMoveSpeedText.text = value; }
    public String currentMightText { get => _currentMightText.text; set => _currentMightText.text = value; }
    public String currentProjectileSpeedText { get => _currentProjectileSpeedText.text; set => _currentProjectileSpeedText.text = value; }
    public String currentMagnetText { get => _currentMagnetText.text; set => _currentMagnetText.text = value; }
    public bool IsGameOver { get => isGameOver; private set => isGameOver = value; }

    void Awake()
    {
        Time.timeScale = 1f;
        if (instance == null) instance = this;
        else Destroy(gameObject);
        HideScreens();
        _gameUIScreen.SetActive(true);
    }
    void Update()
    {
        TestFunc();
        switch (currentGameState)
        {
            case GameState.Play:
                break;
            case GameState.Paused:
                break;
            case GameState.Gameover:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    DisplayResults();
                }
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
    }

    private void TestFunc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (currentGameState == GameState.Paused)
                unPauseGame();
            else PauseGame();
    }

    public void GameOver()
    {
        ChangeGameState(GameState.Gameover);
    }

    private void DisplayResults()
    {
        _resultScreen.SetActive(true);
        _gameUIScreen.SetActive(false);
    }

    public void ChangePlayerUIOnGameOverScreen(PlayerScriptableObject playerStats)
    {
        _playerIcon.sprite = playerStats.icon;
        _playerIcon.preserveAspect = true;
        _playerName.text = playerStats.playerName;
    }
    public void ChangePlayerLevelOnGameOverScreen(int Level)
    {
        _playerLevelText.text = Level.ToString();
    }

    public void ChangePlayerInventoryOnGameOverScreen(List<Image> weaponsImage, List<Image> itemsImage)
    {
        if (weaponsImage.Capacity != _playerWeapons.Capacity || itemsImage.Capacity != _playerItems.Capacity) return;
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
}
