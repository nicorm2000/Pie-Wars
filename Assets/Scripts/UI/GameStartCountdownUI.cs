using TMPro;
using System;
using System.Collections;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopUp";

    [Header("Game Start Countdown Set Up")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private string startText;
    [SerializeField] private float startTextDuration;

    private Animator animator;
    private int previousCountdownNumber;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

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
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTimer());
        if (GameManager.Instance.GetCountdownToStartTimer() > 0)
        {
            countdownText.text = countdownNumber.ToString();

            if (previousCountdownNumber != countdownNumber)
            {
                previousCountdownNumber = countdownNumber;
                animator.SetTrigger(NUMBER_POPUP);
                SoundManager.Instance.PlayCountdownSound();
            }
        }

    }

    private IEnumerator StartText()
    {
        countdownText.text = startText;
        SoundManager.Instance.PlayCountdownSound();
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