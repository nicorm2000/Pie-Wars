using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingClownsSpawner : MonoBehaviour
{

    [SerializeField] private GameObject cannon, cannon2;
    [SerializeField] private Transform shootingPoint, shootingPoint2;
    [SerializeField] private GameObject[] clownPrefabs;
    // Start is called before the first frame update

    [SerializeField] int minTimeBetweenClowns;
    [SerializeField] int maxTimeBetweenClowns;
    private float randomTimer = 0;

    private int playersReady = 0;
    void Update()
    {
        if (randomTimer == 0)
        {
            randomTimer = Random.Range(minTimeBetweenClowns, maxTimeBetweenClowns);
            StartCoroutine(ClownCoolDown());
        }
        if (FindObjectOfType<SelectPlayersManager>() != null)
        {
            if (FindObjectOfType<SelectPlayersManager>().GetPlayersReady() > playersReady)
            {
                playersReady = FindObjectOfType<SelectPlayersManager>().GetPlayersReady();
                ShootClowns();
            }
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
        Instantiate(clownPrefabs[Random.Range(0, clownPrefabs.Length)], shootingPoint.transform.position, shootingPoint.transform.rotation);
        Instantiate(clownPrefabs[Random.Range(0, clownPrefabs.Length)], shootingPoint2.transform.position, shootingPoint2.transform.rotation);

        cannon.transform.Find("Explosion").GetComponent<ParticleSystem>().Play();
        cannon2.transform.Find("Explosion").GetComponent<ParticleSystem>().Play();
    }
}
