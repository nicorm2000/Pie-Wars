using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationHelper : MonoBehaviour
{
    [SerializeField] private UnityEvent callback = null;

    public void Callback()
    {
        callback.Invoke();
    }
}
