using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private int _scorePoint=100;
    [SerializeField]private int _deathEnergyBonus = 3;
    [SerializeField] protected int _healthFactor = 2;

    private LootSpawner _lootSpawner;

    protected virtual void Awake()
    {
        _lootSpawner = GetComponent<LootSpawner>();
    }
    protected override void OnEnable()
    {
        SetHealth();
        base.OnEnable();
    }
    protected virtual  void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.Die();
            Die();
        }
    }
    public override void Die()
    {
        ScoreManager.Instance.AddScore(_scorePoint);
        PlayerEnergy.Instance.Obtain(_deathEnergyBonus);
        EnemyManager.Instance.RemoveFromList(this.gameObject);
        _lootSpawner.Spawn(transform.position);
        base.Die();
    }

    protected virtual void SetHealth()
    {
        _maxHealth +=EnemyManager.Instance.WaveNumber / _healthFactor;
    }
}
