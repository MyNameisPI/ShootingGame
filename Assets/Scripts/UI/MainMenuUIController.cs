using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{

    [Header("-----Canvas-----")]
    [SerializeField] private Canvas _mainCanvas;

    [Header("-----Button-----")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _quiteButton;

    private void OnEnable()
    {
        ButtonPressedBehaviour._buttonFunctionTable.Add(_startButton.name, OnStartButtonClick);
        ButtonPressedBehaviour._buttonFunctionTable.Add(_optionsButton.name, OnOptionsButtonClick);
        ButtonPressedBehaviour._buttonFunctionTable.Add(_quiteButton.name, OnQuitButtonClick);
    }
    private void OnDisable()
    {
        ButtonPressedBehaviour._buttonFunctionTable.Clear();
    }
    private void Start()
    {
        GameManager.GameState = GameState.Playing;
        Time.timeScale = 1f;
        UIInput.Instance.SelectUI(_startButton);
    }

    void OnStartButtonClick()
    {
        _mainCanvas.enabled = false;
        SceneLoader.Instance.LoadingGamePlay();
    }

    void OnOptionsButtonClick()
    {
        UIInput.Instance.SelectUI(_optionsButton);
    }

    void OnQuitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();      
#endif
    }
}
