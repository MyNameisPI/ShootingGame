using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{

    private BossHealthBar _bossHealthBar;
    private Canvas _bossBarCanvas;

    protected override void Awake()
    {
        base.Awake();
        _bossHealthBar = FindObjectOfType<BossHealthBar>();
        _bossBarCanvas = _bossHealthBar.GetComponent<Canvas>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        _bossHealthBar.Initialized(_health, _maxHealth);
        _bossBarCanvas.enabled = true;
    }

    void OnDisable()
    {
        if (_bossBarCanvas!=null)
        {
            _bossBarCanvas.enabled = false;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.Die();
        }
    }

    public override void Die()
    {
        _bossBarCanvas.enabled = false;
        base.Die();
    }

    public override void TakaDamage(float damage)
    {
        base.TakaDamage(damage);
        _bossHealthBar.UpdateStatus(_health, _maxHealth);
    }

    protected override void SetHealth()
    {
        _maxHealth += EnemyManager.Instance.WaveNumber * _healthFactor;
    }

}
