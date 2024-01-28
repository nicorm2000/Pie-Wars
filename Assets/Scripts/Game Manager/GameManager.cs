using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private static GameData gameData = new GameData();

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    public int blueTeamPoints = 0;
    public int redTeamPoints = 0;
    public WinnerTeam winnerTeam = WinnerTeam.Tie;
    private enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }
    public enum WinnerTeam
    {
        BlueTeam,
        RedTeam,
        Tie
    }
    public enum Team
    {
        Blue,
        Red
    }

    [Header("Game Set Up")]
    [SerializeField] private float countdownToStartTimer;
    [SerializeField] private float gamePlayingTimerMax;
    [SerializeField] private GameObject playerPrefab = null;
    [Header("Player skins")]
    [SerializeField] private Material[] skins = null;

    private float gamePlayingTimer;
    private bool isGamePaused;
    private GameState gameState;

    public static GameData GameData => gameData;
    public Material[] Skins => skins;

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

        SpawnPointsManager spawnPointsManager = FindObjectOfType<SpawnPointsManager>();
        Transform[] spawnPoints = playersInputsType.Length == 2 ? spawnPointsManager.SpawnPoints1v1 : spawnPointsManager.SpawnPoints2v2;

        for (int i = 0; i < playersInputsType.Length; i++)
        {
            GameObject player;

            if (playersInputsType[i] == PLAYER_INPUT.UNDEFINED)
            {
                //Instanciar bot, por ahora instancio un npc, pero esto es un patch, reemplazar por el prefabdel bot despues
                player = Instantiate(playerPrefab);
            }
            else
            {
                PlayerInput p = PlayerInput.Instantiate(playerPrefab, controlScheme: "Keyboard", pairWithDevice: Keyboard.current);
                p.SwitchCurrentActionMap("Player" + ((int)playersInputsType[i] - 1 == 0 ? "" : (int)playersInputsType[i] - 1));


                GameInput gi = p.GetComponent<GameInput>();

                gi.OnInteractAction += GameInput_OnInteractAction;
                gi.SetInputType(playersInputsType[i]);

                player = p.gameObject;
            }

            player.transform.position = spawnPoints[i].position;
            player.transform.rotation = spawnPoints[i].rotation;
            player.GetComponent<Player>().playerNumber = i + 1;
            player.GetComponentInChildren<PlayerSkinManager>().ChangeMaterial(skins[i]);
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
                    if (blueTeamPoints == redTeamPoints)
                        winnerTeam = WinnerTeam.Tie;
                    else if (blueTeamPoints > redTeamPoints)
                        winnerTeam = WinnerTeam.BlueTeam;
                    else
                        winnerTeam = WinnerTeam.RedTeam;

                    gameState = GameState.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case GameState.GameOver:
                switch (winnerTeam)
                {
                    case WinnerTeam.BlueTeam:

                        Debug.Log("Blue Wins");
                        break;
                    case WinnerTeam.RedTeam:
                        Debug.Log("Red Wins");
                        break;
                    case WinnerTeam.Tie:
                        Debug.Log("Tie");
                        break;
                }
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

    public void AddPoints(Team team, int pointsToSum)
    {
        if (team == Team.Blue)
            blueTeamPoints += pointsToSum;
        else
            redTeamPoints += pointsToSum;
    }
}