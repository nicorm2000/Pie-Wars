using UnityEngine;

public class StoveCounterSFX : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSound = e.cookingState == StoveCounter.CookingState.Cooking || e.cookingState == StoveCounter.CookingState.Cooked;

        if (playSound) 
        { 
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }
}