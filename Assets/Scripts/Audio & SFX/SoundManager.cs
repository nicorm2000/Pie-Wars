using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] objectDrop;
    [SerializeField] private AudioSource[] objectPickUp;
    [SerializeField] private AudioSource[] trash;
    [SerializeField] private AudioSource[] warning;

    private const string PLAYER_PREFS_SFX_VOLUME = "SXVolume";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioRefsSO audioRefsSO;

    private float volume = 1f;

    private void Awake()
    {
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SFX_VOLUME, 1f);
    }

    private void Start()
    {
        Player.OnPickSomething += Player_OnPickSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void OnDestroy()
    {
        Player.OnPickSomething -= Player_OnPickSomething;
        BaseCounter.OnAnyObjectPlacedHere -= BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed -= TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(trash);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        PlaySound(objectDrop);
    }

    private void Player_OnPickSomething(object sender, System.EventArgs e)
    {
        PlaySound(objectPickUp);
    }

    private void PlaySound(AudioSource[] audioSource)
    {
        audioSource[Random.Range(0, audioSource.Length)].Play();
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public void PlayCountdownSound()
    {
        PlaySound(warning);
    }

    public void PlayWarningSound()
    {
        PlaySound(warning);
    }

    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f)
        {
            volume = 0f;
        }

        PlayerPrefs.SetFloat(PLAYER_PREFS_SFX_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume() 
    {
        return volume;
    }
}