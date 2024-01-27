using UnityEngine;
using System;
using System.Collections;

public class StateChangeEventSystem
{
    public static event Action<EventTiles.State> OnStateChange; // Event for state change

    public static void TriggerStateChange(EventTiles.State newState)
    {
        OnStateChange?.Invoke(newState); // Trigger the state change event
    }
}

public class GameplayEvents : MonoBehaviour
{
    public GameObject[] gameObjects;
    public int minObjectsSelected = 1;
    public int maxObjectsSelected = 3;
    public float cooldown = 10f;
    public float stateDuration = 5f;
    public int maxRandomEvents = 5;

    private int eventCount = 0;

    void Start()
    {
        StateChangeEventSystem.OnStateChange += OnStateChangeHandler;
        StartCoroutine(RandomEventCoroutine());
    }

    void OnDestroy()
    {
        StateChangeEventSystem.OnStateChange -= OnStateChangeHandler;
    }

    void OnStateChangeHandler(EventTiles.State newState)
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.GetComponent<EventTiles>().ChangeState(newState);
        }
    }

    IEnumerator RandomEventCoroutine()
    {
        while (eventCount < maxRandomEvents)
        {
            yield return new WaitForSeconds(cooldown);

            EventTiles.State randomState = GetRandomState();

            int numObjectsToSelect = UnityEngine.Random.Range(minObjectsSelected, maxObjectsSelected + 1);
            for (int i = 0; i < numObjectsToSelect; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, gameObjects.Length);
                GameObject selectedObject = gameObjects[randomIndex];
                EventTiles eventTile = selectedObject.GetComponent<EventTiles>();
                eventTile.ChangeState(randomState);
                //StateChangeEventSystem.TriggerStateChange(randomState);

                StartCoroutine(ResetToIdle(eventTile));
            }

            eventCount++;
        }
    }

    IEnumerator ResetToIdle(EventTiles selectedObject)
    {
        yield return new WaitForSeconds(stateDuration);
        selectedObject.ChangeState(EventTiles.State.Idle);
    }

    EventTiles.State GetRandomState()
    {
        int randomState = UnityEngine.Random.Range(1, 4); // Exclude Idle stated
        Debug.Log((EventTiles.State)randomState);
        return (EventTiles.State)randomState;
    }
}
