using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{
    [Header("Credits Set Up")]
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
    }
}