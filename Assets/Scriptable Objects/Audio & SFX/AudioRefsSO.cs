using UnityEngine;

[CreateAssetMenu()]
public class AudioRefsSO : ScriptableObject
{
    public AudioClip[] footSteps;
    public AudioClip[] objectDrop;
    public AudioClip[] objectPickUp;
    public AudioClip[] trash;
    public AudioClip stoveSizzle;
}