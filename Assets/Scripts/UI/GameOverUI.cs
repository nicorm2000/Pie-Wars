using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private Button mainMenuButton;
    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
    }
    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Hide();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            if (GameManager.Instance.winnerTeam == GameManager.WinnerTeam.Tie)
            {
                winnerText.text = "It's a Tie!";
                winnerText.color = Color.grey;
            }
            else if (GameManager.Instance.winnerTeam == GameManager.WinnerTeam.RedTeam)
            {
                winnerText.text = "Red Team Wins!";
                winnerText.color = Color.red;
            }
            else
            {
                winnerText.text = "Blue Team Wins!";
                winnerText.color = Color.blue;
            }
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}