using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : PersistentSingleton<GameManager>
{
    
    [SerializeField] private GameState _gameState = GameState.Playing;

    public UnityAction OnGameOver = delegate { };
    public static GameState GameState { get => Instance._gameState; set => Instance._gameState = value; }
}

public enum GameState
{
    Playing,
    Pause,
    GameOver,
    Scoring,
}
