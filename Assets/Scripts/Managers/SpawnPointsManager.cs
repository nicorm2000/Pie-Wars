using UnityEngine;

public class SpawnPointsManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints1v1 = null;
    [SerializeField] private Transform[] spawnPoints2v2 = null;

    public Transform[] SpawnPoints1v1 { get => spawnPoints1v1;  }
    public Transform[] SpawnPoints2v2 { get => spawnPoints2v2; }
}
