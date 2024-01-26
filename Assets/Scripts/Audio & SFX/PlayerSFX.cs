using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
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
                SoundManager.Instance.PlayFootstepsSound(player.transform.position, footstepVolume);
            }
        }
    }
}