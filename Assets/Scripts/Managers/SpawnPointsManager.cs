using UnityEngine;

public class SpawnPointsManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints = null;

    public Transform[] SpawnPoints { get => spawnPoints;  }
}
