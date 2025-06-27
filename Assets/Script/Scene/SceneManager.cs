using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;
    [Header("∫ ≈•")]
    public VoidEventSO newGameEvent;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        newGameEvent.onEventRaised += onNewGameEvent;
    }

    private void OnDisable()
    {
        newGameEvent.onEventRaised -= onNewGameEvent;
    }

    private void onNewGameEvent() 
    {
    
    }
}
