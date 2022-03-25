using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private bool _isSpawnEneny;
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private float _timeBetweenSpawns = 1f;
    [SerializeField] private float _timeBetweenWaves = 1f;
    [SerializeField] private int _minEnemyAmount = 4;
    [SerializeField] private int _maxEnemyAmount = 10;
    [SerializeField] GameObject _waveUI;
    [Header("-----Boss Setting-----")]
    [SerializeField] GameObject _BossPrefab;
    [SerializeField] int _bossWaveNumber;

    public int WaveNumber => _waveNumber;
    public GameObject RandomEnemy => _enemyList.Count == 0 ? null : _enemyList[Random.Range(0, _enemyList.Count)];
    public float TimeBetweenWaves => _timeBetweenWaves;

    private int _waveNumber = 1;
    private int _enemyAmount;
    WaitForSeconds _waitTimeBetweenSpawns;
    WaitForSeconds _waitTimeBetweenWaves;
    WaitUntil _waitUntilNoEnemy;
    List<GameObject> _enemyList = new List<GameObject>();
    

    protected override void Awake()
    {
        base.Awake();
        _waitTimeBetweenSpawns = new WaitForSeconds(_timeBetweenSpawns);
        _waitTimeBetweenWaves = new WaitForSeconds(_timeBetweenWaves);
        _waitUntilNoEnemy = new WaitUntil(() => _enemyList.Count == 0);
    }

    private IEnumerator Start()
    {
        while (_isSpawnEneny&& GameManager.GameState != GameState.GameOver)
        {
            yield return _waitUntilNoEnemy;
            _waveUI.SetActive(true);
            yield return _waitTimeBetweenWaves;
            _waveUI.SetActive(false);
            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
        }
    }

    IEnumerator RandomlySpawnCoroutine()
    {
        if (_waveNumber % _bossWaveNumber==0)
        {
           _enemyList.Add(PoolManager.Release(_BossPrefab));
        }
        else
        {
            _enemyAmount = Mathf.Clamp(_enemyAmount, _minEnemyAmount + _waveNumber / _bossWaveNumber, _maxEnemyAmount);
            for (int i = 0; i < _enemyAmount; i++)
            {
                _enemyList.Add(PoolManager.Release(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)]));
                yield return _waitTimeBetweenSpawns;
            }
        }
            _waveNumber++;
        
    }

    public void RemoveFromList(GameObject enemyPrefab) => _enemyList.Remove(enemyPrefab);
}
