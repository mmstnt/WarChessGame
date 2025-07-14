using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("∫ ≈•")]
    public VoidEventSO newGameEvent;
    public VoidEventSO loadGameEvent;
    public SceneLoadEventSO sceneUnloadEvent;

    [Header("≤’•Û")]
    public GameObject menu;
    public GameObject dialogInterface;
    public GameObject createCharacter;

    public GameObject backGround;

    private GameObject currentUI;
    private void Awake()
    {
        
    }

    private void Start()
    {
        menu.SetActive(true);
    }

    private void OnEnable()
    {
        //newGameEvent.onEventRaised += onLoadDataEvent;
        //loadGameEvent.onEventRaised += onLoadDataEvent;
        sceneUnloadEvent.LoadRequestEvent += onSceneUnloadEvent;
    }

    private void OnDisable()
    {
        //newGameEvent.onEventRaised -= onLoadDataEvent;
        //loadGameEvent.onEventRaised -= onLoadDataEvent;
        sceneUnloadEvent.LoadRequestEvent -= onSceneUnloadEvent;
    }

    private void onSceneUnloadEvent(GameSceneSO locationToLoad, bool fadeScreen)
    {
        onLoadDataEvent();
        switch (locationToLoad.sceneType)
        {
            case SceneType.Menu:
                menu.SetActive(true);
                break;
            case SceneType.Dialog:
                dialogInterface.SetActive(true);
                break;
            case SceneType.CreateCharacter:
                createCharacter.SetActive(true);
                break;
        }
    }

    private void onLoadDataEvent()
    {
        menu.SetActive(false);
        dialogInterface.SetActive(false);
        createCharacter.SetActive(false);
    }
}
