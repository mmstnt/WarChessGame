using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("��ť")]
    public VoidEventSO newGameEvent;
    public VoidEventSO loadGameEvent;

    [Header("�C������")]
    public GameObject menu;
    public GameObject dialogInterface;

    private void Awake()
    {
        menu.SetActive(true);
    }

    private void OnEnable()
    {
        newGameEvent.onEventRaised += onLoadDataEvent;
        loadGameEvent.onEventRaised += onLoadDataEvent;
    }

    private void OnDisable()
    {
        newGameEvent.onEventRaised -= onLoadDataEvent;
        loadGameEvent.onEventRaised -= onLoadDataEvent;
    }

    private void onLoadDataEvent()
    {
        menu.SetActive(false);
    }
}
