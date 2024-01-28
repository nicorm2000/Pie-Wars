using UnityEngine;

[CreateAssetMenu()]
public class AudioRefsSO : ScriptableObject
{
    public AudioClip[] objectDrop;
    public AudioClip[] objectPickUp;
    public AudioClip[] trash;
    public AudioClip[] warning;
    public AudioClip stoveSizzle;
}