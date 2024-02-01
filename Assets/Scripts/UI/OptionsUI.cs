using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [Header("Options Set Up")]
    [SerializeField] private Button SFXButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI SFXText;
    [SerializeField] private TextMeshProUGUI musicText;

    [Header("Key Bindings Set Up")]
    [SerializeField] private Transform pressToRebindKeyTransform;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAltText;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;
    //[SerializeField] private Button gamepadInteractButton;
    //[SerializeField] private Button gamepadInteractAltButton;
    //[SerializeField] private Button gamepadPauseButton;

    private Action OnCloseButtonAction;

    private void Awake()
    {
        Instance = this;

        SFXButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            OnCloseButtonAction();
        });

        //moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Up); });
        //moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Down); });
        //moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Left); });
        //moveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Right); });
        //interactButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
        //interactAltButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.InteractAlternate); });
        //pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause); });
        //gamepadInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Interact); });
        //gamepadInteractAltButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_InteractAlternate); });
        //gamepadPauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Pause); });
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;

        UpdateVisual();

        HidePressToRebindKey();
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Hide();
    }
    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }
    private void UpdateVisual()
    {
        SFXText.text = "SFX: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);

        //moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        //moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        //moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        //moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        //interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        //interactAltText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        //pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    public void Show(Action OnCloseButtonAction)
    {
        this.OnCloseButtonAction = OnCloseButtonAction;

        gameObject.SetActive(true);

        SFXButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    //private void ShowPressToRebindKey()
    //{
    //    pressToRebindKeyTransform.gameObject.SetActive(true);
    //}

    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    //private void RebindBinding(GameInput.Binding binding)
    //{
    //    ShowPressToRebindKey();
    //    GameInput.Instance.RebindBinding(binding, () =>
    //    {
    //        HidePressToRebindKey();
    //        UpdateVisual();
    //    });
    //}
}