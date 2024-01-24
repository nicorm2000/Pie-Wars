using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;

    private enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    [Header("Game Set Up")]
    [SerializeField] private float waitingToStartTimer;
    [SerializeField] private float countdownToStartTimer;
    [SerializeField] private float gamePlayingTimerMax;
    private float gamePlayingTimer;

    private GameState gameState;

    private void Awake()
    {
        Instance = this;

        gameState = GameState.WaitingToStart;
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;

                if (waitingToStartTimer < 0f)
                {
                    gameState = GameState.CountdownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
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
}