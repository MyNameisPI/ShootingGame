using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] private Canvas _canvas_HUD;

    [SerializeField] private AudioData _confirmGameoverSFX;

    private Animator _animator;
    private Canvas _canvasThis;
    private int _exitID = Animator.StringToHash("GameOverScreenExit");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _canvasThis = GetComponent<Canvas>();

        _animator.enabled = false;
        _canvasThis.enabled = false;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameOver += OnGameOver;
        _playerInput.GameOverPressedEvent += OnConfirmGameOver;
    }


    private void OnDisable()
    {
        GameManager.Instance.OnGameOver -= OnGameOver;
        _playerInput.GameOverPressedEvent -= OnConfirmGameOver;
    }

    private void OnGameOver()
    {
        _canvas_HUD.enabled = false;
        _canvasThis.enabled = true;
        _animator.enabled = true;
        _playerInput.DisableAllInput();
    }

    void EnableGameOverScreenInput()
    {
        _playerInput.EnableGameOverScreen();
    }
    private void OnConfirmGameOver()
    {
        AudioManager.Instance.PlaySFX(_confirmGameoverSFX);
        _playerInput.DisableAllInput();
        _animator.Play(_exitID);
        SceneLoader.Instance.LoadingScoring();
    }
}
