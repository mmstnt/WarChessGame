using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;
    [Header("監聽")]
    public SceneLoadEventSO sceneLoadEvent;
    public VoidEventSO newGameEvent;

    [Header("廣播")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;
    public SceneLoadEventSO sceneUnloadEvent;

    [Header("場景")]
    public GameSceneSO menuScene;
    public GameSceneSO createCharacterScene;
    public GameSceneSO dialogScene;

    private GameSceneSO currentLoadedScene;
    private GameSceneSO sceneToLoad;
    private bool fadeScreen;
    private bool isLoading;

    public float fadeDuration;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        sceneLoadEvent.RaiseLoadRequestEvent(menuScene, true);
        
    }

    private void OnEnable()
    {
        sceneLoadEvent.LoadRequestEvent += onLoadRequestEvent;
        newGameEvent.onEventRaised += onNewGameEvent;
    }

    private void OnDisable()
    {
        sceneLoadEvent.LoadRequestEvent -= onLoadRequestEvent;
        newGameEvent.onEventRaised -= onNewGameEvent;
    }

    private void onNewGameEvent()
    {
        sceneToLoad = createCharacterScene;
        sceneLoadEvent.RaiseLoadRequestEvent(sceneToLoad, true);
    }

    private void onLoadRequestEvent(GameSceneSO locationToLoad, bool fadeScreen)
    {
        if (isLoading)
            return;

        isLoading = true;
        sceneToLoad = locationToLoad;
        this.fadeScreen = fadeScreen;
        if (currentLoadedScene != null)
        {
            StartCoroutine(unLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
    }

    private IEnumerator unLoadPreviousScene()
    {
        if (fadeScreen)
        {
            //漸黑
            fadeEvent.FadeIn(fadeDuration);
        }

        yield return new WaitForSeconds(fadeDuration);

        sceneUnloadEvent.RaiseLoadRequestEvent(sceneToLoad, true);

        //if (currentLoadedScene != null)
        //{
        //    yield return currentLoadedScene.sceneReference.UnLoadScene();
        //}

        LoadNewScene();
    }

    private void LoadNewScene()
    {
        //var loadingOption = sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        //loadingOption.Completed += onLoadCompleted;
        onLoadCompleted();
    }

    private void onLoadCompleted()
    {
        currentLoadedScene = sceneToLoad;

        if (fadeScreen)
        {
            //漸透明
            fadeEvent.FadeOut(fadeDuration);
        }
        isLoading = false;


        //場景完成後事件
        afterSceneLoadedEvent.raiseEvent();
    }
}
