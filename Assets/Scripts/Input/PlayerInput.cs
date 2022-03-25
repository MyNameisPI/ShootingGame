using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
[CreateAssetMenu(fileName ="PlayerInput",menuName ="Input/PlayerInput")]
public class PlayerInput : ScriptableObject, InputActions.IGamePlayActions,InputActions.IPauseMenuActions,InputActions.IGameOverScreenActions
{
    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction MoveCanceledEvent = delegate { };
    public event UnityAction FireEvent = delegate { };
    public event UnityAction FireCanceledEvent = delegate { };
    public event UnityAction DodgeEvent = delegate { };
    public event UnityAction OverDriveEvent = delegate { };
    public event UnityAction OnPauseEvent = delegate { };
    public event UnityAction UnPauseEvent = delegate { };
    public event UnityAction LaunchMissileEvent = delegate { };
    public event UnityAction GameOverPressedEvent = delegate { };



    private InputActions _inputActions;

    private void OnEnable()
    {
        if (_inputActions == null)
        {
            _inputActions = new InputActions();
            _inputActions.GamePlay.SetCallbacks(this);
            _inputActions.PauseMenu.SetCallbacks(this);
            _inputActions.GameOverScreen.SetCallbacks(this);
        }
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    void SwitchInputActionMap(InputActionMap inputAction,bool isUIInput)
    {
        _inputActions.Disable();
        inputAction.Enable();
        if (isUIInput)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void EnableGamePlayInput() => SwitchInputActionMap(_inputActions.GamePlay, false);

    public void EnablePauseMenu() => SwitchInputActionMap(_inputActions.PauseMenu, true);
    public void EnableGameOverScreen() => SwitchInputActionMap(_inputActions.GameOverScreen, true);

    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;


    public void DisableAllInput()
    {
        _inputActions.GamePlay.Disable();
        _inputActions.PauseMenu.Disable();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            MoveEvent.Invoke(context.ReadValue<Vector2>());
        }
        if (context.phase== InputActionPhase.Canceled)
        {
            MoveCanceledEvent.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            FireEvent.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            FireCanceledEvent.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DodgeEvent.Invoke();
        }
    }

    public void OnOverDrive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OverDriveEvent.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnPauseEvent.Invoke();
    }

    public void OnUnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
            UnPauseEvent.Invoke();
    }

    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LaunchMissileEvent.Invoke();

        }
    }

    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameOverPressedEvent.Invoke();
        }
    }
}
