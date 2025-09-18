using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance;
    [Header("¼s¼½")]
    public VoidEventSO gameConfirmEvent;
    public VoidEventSO mouseClickEvent;

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
        playerInputControl.GamePlay.MouseClick.started += onMouseClick;
    }

    private void OnDisable()
    {
        playerInputControl.Disable();
        playerInputControl.GamePlay.Confirm.started -= onConfirm;
        playerInputControl.GamePlay.MouseClick.started -= onMouseClick;
    }

    private void onConfirm(InputAction.CallbackContext context)
    {
        gameConfirmEvent.raiseEvent();
    }

    private void onMouseClick(InputAction.CallbackContext context)
    {
        mouseClickEvent.raiseEvent();
    }
}
