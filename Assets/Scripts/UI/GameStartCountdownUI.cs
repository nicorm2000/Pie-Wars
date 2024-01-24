using TMPro;
using System;
using System.Collections;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [Header("Game Start Countdown Set Up")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private string startText;
    [SerializeField] private float startTextDuration;

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Hide();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            Show();
        }
        else if (GameManager.Instance.IsGamePlaying())
        {
            StartCoroutine(StartText());
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GetCountdownToStartTimer() > 0)
            countdownText.text = Mathf.Ceil(GameManager.Instance.GetCountdownToStartTimer()).ToString();
    }

    private IEnumerator StartText()
    {
        countdownText.text = startText;
        yield return new WaitForSeconds(startTextDuration);
        Hide();
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