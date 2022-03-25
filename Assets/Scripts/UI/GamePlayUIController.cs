using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUIController : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [Header("-----Canvas-----")]
    [SerializeField] private Canvas _canvas_HUD;
    [SerializeField] private Canvas _canvas_PauseMenu;
    [Header("-----Button-----")]
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _mainMenuButton;
    [Header("-----Audo Data-----")]
    [SerializeField] private AudioData _pauseSFX;
    [SerializeField] private AudioData _unpauseSFX;

    private int _buttonPressedParameter = Animator.StringToHash("Pressed");

    private void OnEnable()
    {
        _playerInput.OnPauseEvent += OnPause;
        _playerInput.UnPauseEvent += UnPause;

        ButtonPressedBehaviour._buttonFunctionTable.Add(_resumeButton.name, OnResumeButtonClick);
        ButtonPressedBehaviour._buttonFunctionTable.Add(_optionsButton.name, OnOptionsButtonClick);
        ButtonPressedBehaviour._buttonFunctionTable.Add(_mainMenuButton.name, OnMainMenuButtonClick);
    }
    private void OnDisable()
    {
        _playerInput.OnPauseEvent -= OnPause;
        _playerInput.UnPauseEvent -= UnPause;
        ButtonPressedBehaviour._buttonFunctionTable.Clear();
    }

    private void OnPause()
    {
        GameManager.GameState = GameState.Pause;
        TimeController.Instance.OnPause();
        _canvas_PauseMenu.enabled = true;
        _canvas_HUD.enabled = false;
        _playerInput.EnablePauseMenu();
        _playerInput.SwitchToDynamicUpdateMode();
        UIInput.Instance.SelectUI(_resumeButton);
        AudioManager.Instance.PlaySFX(_pauseSFX);
    }
    public void UnPause()
    {
        _resumeButton.Select();
        _resumeButton.animator.SetTrigger(_buttonPressedParameter);
        AudioManager.Instance.PlaySFX(_unpauseSFX);
    }
    void OnResumeButtonClick()
    {
        GameManager.GameState = GameState.Playing;
        TimeController.Instance.UnPause();
        _canvas_PauseMenu.enabled = false;
        _canvas_HUD.enabled = true;
        _playerInput.EnableGamePlayInput();
        _playerInput.SwitchToFixedUpdateMode();
    }

    void OnOptionsButtonClick()
    {
        //TODO:
        UIInput.Instance.SelectUI(_optionsButton);
        _playerInput.EnablePauseMenu();
    }

    void OnMainMenuButtonClick()
    {
        _canvas_PauseMenu.enabled = false;
        SceneLoader.Instance.LoadingMainMenu();
    }
}
