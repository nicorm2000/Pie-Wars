using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Main Menu Set Up")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() => 
        {
            Loader.Load(Loader.Scene.Game);
        });

        creditsButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.Credits);
        });

        exitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
#endif
            Application.Quit();
        });
    }
}