using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    #region ScoreDisplay
    private int _score;
    private int _currentScore;
    private Vector3 _scoreTextScale = new Vector3(1.2f, 1.2f, 1f);

    public int Score => _score;

    public void ResetScore()
    {
        _score = 0;
        _currentScore = 0;
        ScoreDisplay.UpdateScore(_score);
    }

    public void AddScore(int score)
    {
        _currentScore += score;
        StartCoroutine(nameof(AddScoreCoroutine));
    }

    IEnumerator AddScoreCoroutine()
    {
        ScoreDisplay.ScaleText(_scoreTextScale);
        while (_score < _currentScore)
        {
            _score += 1;
            ScoreDisplay.UpdateScore(_score);
            yield return null;
        }
        ScoreDisplay.ScaleText(Vector3.one);
    }
    #endregion

    #region High Score System
    [System.Serializable]
    public class PlayerScore
    {
        public string _name;
        public int _score;

        public PlayerScore(string name , int score)
        {
            _name = name;
            _score = score;
        }
    }

    [System.Serializable]
    public class PlayerScoreData
    {
        public List<PlayerScore> _playerScoreList = new List<PlayerScore>();
    }

    readonly string SaveFileName = "player_score.json";
    string _playerName = "No Name";


    public bool HasNewHighScore => _score > LoadPlayerScoreData()._playerScoreList[9]._score;

    public void SetPlayerName(string newName)
    {
        _playerName = newName;
        Debug.Log(_playerName);
    }
    public void SavePlayerScoreData()
    {
        var data = LoadPlayerScoreData();
        data._playerScoreList.Add(new PlayerScore(_playerName, _score));
        data._playerScoreList.Sort((x, y) => y._score.CompareTo(x._score));
        SaveSystem.SaveByJson(SaveFileName, data);
    }
    public PlayerScoreData LoadPlayerScoreData()
    {
        PlayerScoreData playerScoreDate = new PlayerScoreData();
        if (SaveSystem.SaveFileExists(SaveFileName))
        {
            playerScoreDate = SaveSystem.LoadFormJson<PlayerScoreData>(SaveFileName);
        }
        else
        {
            while (playerScoreDate._playerScoreList.Count<10)
            {
                playerScoreDate._playerScoreList.Add(new PlayerScore(_playerName, 0));
            }
            SaveSystem.SaveByJson(SaveFileName, playerScoreDate);
        }
        return playerScoreDate;
    }

    #endregion

}
