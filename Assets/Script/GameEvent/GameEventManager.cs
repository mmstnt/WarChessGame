using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance;
    [Header("¼s¼½")]
    public VoidEventSO gameConfirmEvent;

    public PlayerInputControl playerInputControl;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        playerInputControl = new PlayerInputControl();
    }

    private void OnEnable()
    {
        playerInputControl.Enable();
        playerInputControl.GamePlay.Confirm.started += onConfirm;
    }

    private void OnDisable()
    {
        playerInputControl.Disable();
        playerInputControl.GamePlay.Confirm.started -= onConfirm;
    }

    private void onConfirm(InputAction.CallbackContext context)
    {
        gameConfirmEvent.raiseEvent();
    }
}
