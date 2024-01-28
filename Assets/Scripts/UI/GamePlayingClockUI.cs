using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GamePlayingClockUI : MonoBehaviour
{
    [Header("Game Playing Clock UI Set Up")]
    [SerializeField] private Image timerImage;
    [SerializeField] private TextMeshProUGUI blueTeam;
    [SerializeField] private TextMeshProUGUI redTeam;
    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Hide();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update()
    {
        timerImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized();
        blueTeam.text  = GameManager.Instance.blueTeamPoints.ToString();
        redTeam.text = GameManager.Instance.redTeamPoints.ToString();
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