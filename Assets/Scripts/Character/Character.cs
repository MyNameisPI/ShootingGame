using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private GameObject _deathVFX;
    [SerializeField] private AudioData[] _deathSFX;
    [Header("-----Health-----")]
    [SerializeField] protected float _maxHealth;
    [SerializeField] private StatusBar _onHeadHealthBar;
    [SerializeField] private bool _isShowHeadHealthBar;

    protected float _health;

    protected virtual void OnEnable()
    {
        _health = _maxHealth;
        if (_isShowHeadHealthBar)
            ShowOnHeadHealthBar();
        else
            HideOnHeadHealthBar();
    }

    void ShowOnHeadHealthBar()
    {
        _onHeadHealthBar.gameObject.SetActive(true);
        _onHeadHealthBar.Initialized(_health, _maxHealth);
    }

    void HideOnHeadHealthBar()
    {
        _onHeadHealthBar.gameObject.SetActive(false);
    }

    public virtual void TakaDamage(float damage)
    {
        if (_health == 0) return;
        _health = Mathf.Clamp(_health -= damage, 0f, _maxHealth);  
        if (_isShowHeadHealthBar)
        {
            _onHeadHealthBar.UpdateStatus(_health, _maxHealth);
        }
        if (_health <= 0) Die();
    }

    public virtual void Die()
    {

        _health = 0;
        AudioManager.Instance.PlayRandomSFX(_deathSFX);
        gameObject.SetActive(false);
        PoolManager.Release(_deathVFX,transform.position);
    }

    public virtual void RestoreHealth(float value)
    {
        if (_health == _maxHealth) return;
        _health = Mathf.Clamp(_health + value, 0f, _maxHealth);
        if (_isShowHeadHealthBar)
        {
            _onHeadHealthBar.UpdateStatus(_health, _maxHealth);
        }
    }
    /// <summary>
    /// 生命再生协程 在单位时间不断恢复一定百分比的生命值
    /// </summary>
    /// <param name="waitTime">时间间隔</param>
    /// <param name="precent">百分比</param>
    /// <returns></returns>
    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitTime,float precent)
    {
        while (_health<_maxHealth)
        {
            yield return waitTime;
            RestoreHealth(_maxHealth * precent);
        }
    }
}
