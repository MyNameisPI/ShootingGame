using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectileOverDrive
{
    [SerializeField] private AudioData _targetAcquiredSFX;

    [Header("-----Speed Change-----")]
    [SerializeField] private float _lowSpeed = 8f;
    [SerializeField] private float _highSpeed = 25f;
    [SerializeField] private float _variableSpeedDelay = 0.5f;

    [Header("-----Explosion-----")]
    [SerializeField] private GameObject _explosionVFX;
    [SerializeField] private AudioData _explosionSFX;
    [SerializeField] private LayerMask _enemyLayerMask=default;
    [SerializeField] private float _explosionRadius = 3f;
    [SerializeField] private float _explosionDamage = 100f;

    private WaitForSeconds _waitForVariableSpeedDelay;

    protected override void Awake()
    {
        base.Awake();
        _waitForVariableSpeedDelay = new WaitForSeconds(_variableSpeedDelay);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(nameof(VariableSpeedCoroutine));
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        PoolManager.Release(_explosionVFX,transform.position);
        AudioManager.Instance.PlaySFX(_explosionSFX);

        var colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _enemyLayerMask);
        foreach (var item in colliders)
        {
           if(item.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakaDamage(_explosionDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
    IEnumerator VariableSpeedCoroutine()
    {
        _moveSpeed = _lowSpeed;
        yield return _waitForVariableSpeedDelay;
        _moveSpeed = _highSpeed;

        if (_target!=null)
        {
            AudioManager.Instance.PlaySFX(_targetAcquiredSFX);
        }
    }
}
