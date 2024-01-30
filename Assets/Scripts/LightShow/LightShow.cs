using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightShow : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnStateChanged += StartShow;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartShow(object sender, EventArgs e)
    {
        if (GameManager.Instance.GetGameState() == GameManager.GameState.CountdownToStart)
        {
            GetComponent<Animation>().Play();
            Destroy(gameObject, 3.0f);
        }
    }
}
