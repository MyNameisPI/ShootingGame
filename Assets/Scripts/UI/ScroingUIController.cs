using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScroingUIController : MonoBehaviour
{
    [Header("-----Background-----")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Sprite[] _backgroundSprites;
    [Header("-----Scoring-----")]
    [SerializeField] private Canvas _scoringCanvas;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Button _buttonMainMenu;
    [SerializeField] private Transform _highScoreLeaderboardContainer;
    [Header("-----High Score Screen-----")]
    [SerializeField] private Canvas _newHighScoreScreenCanvas;
    [SerializeField] private Button _buttonCancel;
    [SerializeField] private Button _buttonSubmit;
    [SerializeField] private InputField _playerInputField;

    private void Start()
    {
        ShowRandomBackground();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (ScoreManager.Instance.HasNewHighScore)
        {
            ShowNewHighScoreScreen();
        }
        else
        {
            ShowScroingScreen();
        }
        

        ButtonPressedBehaviour._buttonFunctionTable.Add(_buttonMainMenu.name, OnButtonMainMenuClicked);
        ButtonPressedBehaviour._buttonFunctionTable.Add(_buttonCancel.name, HideNewHighScoreScreen);
        ButtonPressedBehaviour._buttonFunctionTable.Add(_buttonSubmit.name, OnButtonSubmitClicked);
        GameManager.GameState = GameState.Scoring;
    
    }

    private void OnDisable()
    {
        ButtonPressedBehaviour._buttonFunctionTable.Clear();
    }
    void ShowRandomBackground()
    {
        _backgroundImage.sprite = _backgroundSprites[Random.Range(0, _backgroundSprites.Length)];
    }

    void ShowScroingScreen()
    {
        _scoringCanvas.enabled = true;
        _scoreText.text = ScoreManager.Instance.Score.ToString();
        UIInput.Instance.SelectUI(_buttonMainMenu);
        //更新高分榜
        UpdateHighScoreLeaderboard();

    }
    private void ShowNewHighScoreScreen()
    {
        _newHighScoreScreenCanvas.enabled = true;
        UIInput.Instance.SelectUI(_buttonCancel);
        
    }
    private void HideNewHighScoreScreen()
    {
        _newHighScoreScreenCanvas.enabled = false;
        ScoreManager.Instance.SavePlayerScoreData();
        ShowRandomBackground();
        ShowScroingScreen();
    }

    private void UpdateHighScoreLeaderboard()
    {
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData()._playerScoreList;
        for (int i = 0; i < _highScoreLeaderboardContainer.childCount; i++)
        {
            var child = _highScoreLeaderboardContainer.GetChild(i);
            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i]._score.ToString();
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i]._name;
        }
    }

    void OnButtonMainMenuClicked()
    {
        _scoringCanvas.enabled = false;
        SceneLoader.Instance.LoadingMainMenu();
    }

    void OnButtonSubmitClicked()
    {
        if (!string.IsNullOrEmpty(_playerInputField.text))
        {
            ScoreManager.Instance.SetPlayerName(_playerInputField.text);
        }
        HideNewHighScoreScreen();
    }

    
}
