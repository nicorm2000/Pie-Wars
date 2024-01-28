using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [SerializeField] private AudioSource[] footsteps = null;
    [SerializeField] private float footstepTimerMax;
    [SerializeField] private float footstepVolume;

    private Player player;
    private float footstepTimer;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;

        if (footstepTimer < 0f)
        {
            footstepTimer = footstepTimerMax;

            if (player.IsWalking())
            {
                footsteps[Random.Range(0, footsteps.Length)].Play();
            }
        }
    }
}