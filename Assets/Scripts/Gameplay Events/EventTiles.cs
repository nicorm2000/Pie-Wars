using UnityEngine;

public class EventTiles : MonoBehaviour
{
    [SerializeField] private float debuffDuration = 0.5f;

    private Collider collider;


    void Start()
    {
        StateChangeEventSystem.OnStateChange += OnStateChangeHandler;
        collider = GetComponent<Collider>();
    }
    void OnDestroy()
    {
        StateChangeEventSystem.OnStateChange -= OnStateChangeHandler;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            if (isSlippery)
            {
                player.isSlipper = true;
            }

            if (isSlow)
            {
                player.isSlow = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        Debug.Log(player);
        if (player != null)
        {
            player.TriggerStateChange(debuffDuration);
        }
    }

    public enum State { Idle, Slow, Slippery, Block }
    private State currentState = State.Idle;

    private bool isSlippery = false;
    private bool isSlow = false;

    
    public void ChangeState(State newState)
    {
        currentState = newState;
        UpdateState();
    }

    void OnStateChangeHandler(State newState)
    {
        ChangeState(newState);
    }

    void UpdateState()
    {
        switch (currentState) //Animations pending for different states.
        {
            case State.Idle:
                isSlippery = false;
                isSlow = false;
                collider.isTrigger = true;
                break;
            case State.Slow:
                isSlow = true;
                break;
            case State.Slippery:
                isSlippery = true;
                break;
            case State.Block:
                collider.isTrigger = false;
                break;
        }
    }
}