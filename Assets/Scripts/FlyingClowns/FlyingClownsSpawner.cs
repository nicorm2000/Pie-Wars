using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingClownsSpawner : MonoBehaviour
{

    [SerializeField] private GameObject cannon, cannon2;
    [SerializeField] private GameObject[] clownPrefabs;
    // Start is called before the first frame update

    [SerializeField] int minTimeBetweenClowns;
    [SerializeField] int maxTimeBetweenClowns;
    private float randomTimer = 0;

    void Update()
    {
        if (randomTimer == 0)
        {
            randomTimer = Random.Range(minTimeBetweenClowns, maxTimeBetweenClowns);
            StartCoroutine(ClownCoolDown());
        }
    }

    private IEnumerator ClownCoolDown()
    {
        yield return new WaitForSeconds(randomTimer);
        ShootClowns();
        randomTimer = 0;
    }

    private void ShootClowns()
    {
        Instantiate(clownPrefabs[Random.Range(0, clownPrefabs.Length)],cannon.transform.position, cannon.transform.rotation, cannon.transform);
        Instantiate(clownPrefabs[Random.Range(0, clownPrefabs.Length)], cannon2.transform.position, cannon2.transform.rotation, cannon2.transform);
    }
}
