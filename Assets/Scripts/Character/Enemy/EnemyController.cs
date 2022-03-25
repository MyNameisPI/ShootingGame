using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("-----Move-----")]
    [SerializeField] protected float _paddingX;
    [SerializeField] private float _paddingY;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _moverotation = 2f;

    [Header("-----Fire-----")]
    [SerializeField] protected float _minFireInterval;
    [SerializeField] protected float _maxFireInterval;
    [SerializeField] protected GameObject[] _projectiles;
    [SerializeField] protected AudioData[] _projectileLaunchSFX;
    [SerializeField] protected Transform _muzzle;
    [SerializeField] protected ParticleSystem _muzzleVFX;

    protected Vector3 targetPosition;

    protected virtual void Awake()
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        _paddingX = size.x / 2f;
        _paddingY = size.y / 2f;
    }
    protected virtual void OnEnable()
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = ViewPort.Instance.RandomEnemySpawnPosition(_paddingX, _paddingY);
        targetPosition = ViewPort.Instance.RandomRightHalfPosition(_paddingX, _paddingY);
        while (gameObject.activeSelf)
        {
            //计算是否到达目标点  epsilon是一个无限接近于0的浮点数
            if (Mathf.Abs(transform.position.sqrMagnitude - targetPosition.sqrMagnitude)<Mathf.Epsilon)
            {
                targetPosition = ViewPort.Instance.RandomRightHalfPosition(_paddingX, _paddingY);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y*_moverotation,Vector3.right);
            }
            yield return null;
        }
        
    }

    protected virtual IEnumerator RandomlyFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(_minFireInterval, _maxFireInterval));
            if (GameManager.GameState == GameState.GameOver) yield break;
            foreach (var item in _projectiles)
            {
                PoolManager.Release(item, _muzzle.position);
            }
            AudioManager.Instance.PlayRandomSFX(_projectileLaunchSFX);
            _muzzleVFX.Play();
        }
    }
}
