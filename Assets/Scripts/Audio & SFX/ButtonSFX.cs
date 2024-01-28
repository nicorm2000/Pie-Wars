using UnityEngine;
using UnityEngine.UI;

public class ButtonSFX : MonoBehaviour
{
    AudioSource audioSource = null;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponentInParent<Button>().onClick.AddListener(() => audioSource.Play());
    }

    public void Callback() //referenciado desde la escena
    {
        audioSource.Play();
    }
}
