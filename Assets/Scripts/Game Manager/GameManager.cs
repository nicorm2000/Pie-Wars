using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private static GameData gameData = new GameData();

    public static GameData GameData => gameData;

    private enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    [Header("Game Set Up")]
    [SerializeField] private float countdownToStartTimer;
    [SerializeField] private float gamePlayingTimerMax;
    [SerializeField] private GameObject playerPrefab = null;

    private float gamePlayingTimer;
    private bool isGamePaused;
    private GameState gameState;

    private void Awake()
    {        
        if (Instance != null && Instance != this)
        { 
            DestroyImmediate(gameObject);
            return;
        }
        else 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            gameState = GameState.WaitingToStart;

            SceneManager.sceneLoaded += 
                (scene, mode) => 
                {
                    if (scene.name == Loader.Scene.Level.ToString())
                    {
                        InitializeGameplay();
                    }
                };
        }
    }

    private void InitializeGameplay()
    {
        SpawnPlayers();

        GameInput.OnPauseAction += GameInput_OnPauseAction;
    }

    private void SpawnPlayers()
    {
        PLAYER_INPUT[] playersInputsType = gameData.GetPlayersInputsType();
        Transform[] spawnPoints = FindObjectOfType<SpawnPointsManager>().SpawnPoints;

        for (int i = 0; i < playersInputsType.Length; i++)
        {
            if (playersInputsType[i] == PLAYER_INPUT.UNDEFINED)
            {
                //Instantiate bot
            }
            else
            {
                PlayerInput p;

                if (playersInputsType[i] == PLAYER_INPUT.GAMEPAD)
                {
                    p = PlayerInput.Instantiate(playerPrefab, controlScheme: "Gamepad", pairWithDevice: Gamepad.all[i - 2]);
                }
                else
                {
                    p = PlayerInput.Instantiate(playerPrefab, controlScheme: "Keyboard", pairWithDevice: Keyboard.current);
                }

                p.SwitchCurrentActionMap("Player" + ((int)playersInputsType[i] - 1 == 0 ? "" : (int)playersInputsType[i] - 1));
                p.transform.position = spawnPoints[i].position;
                p.transform.rotation = spawnPoints[i].rotation;

                GameInput gi = p.GetComponent<GameInput>();

                gi.OnInteractAction += GameInput_OnInteractAction;
                gi.SetInputType(playersInputsType[i]);
            }            
        }
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (gameState == GameState.WaitingToStart)
        {
            gameState = GameState.CountdownToStart;

            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.WaitingToStart:
                    
                break;
            case GameState.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;

                if (countdownToStartTimer < 0f)
                {
                    gameState = GameState.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;

                if (gamePlayingTimer < 0f)
                {
                    gameState = GameState.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.GameOver:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return gameState == GameState.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return gameState == GameState.CountdownToStart;
    }

    public bool IsGameOver()
    {
        return gameState == GameState.GameOver;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}