using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingClownBehaviour : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] float explosionForce;

    private float timeToDestroy = 3f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.up * explosionForce, ForceMode.Impulse);
        Destroy(this, timeToDestroy);
    }
}
