using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    [SerializeField] private float _continuousFireDuration = 1.5f;

    [Header("-----Player Detection-----")]
    [SerializeField] private Transform _playerDetectionTransform;
    [SerializeField] private Vector3 _playerDetectionSize;
    [SerializeField] private LayerMask _playerLayer;

    [Header("-----Beam-----")]
    [SerializeField] private float _beamCooldownTime = 12f;
    [SerializeField] private AudioData _beamChargingSFX;
    [SerializeField] private AudioData _beamLaunchSFX;

    private bool _isBeamReady;
    private int _launchBeamID = Animator.StringToHash("launchBeam");
    private Transform _playerTransform;

    private WaitForSeconds _waitForContinuousFireInterval;
    private WaitForSeconds _waitForFireInterval;
    private WaitForSeconds _waitForBeamCooldownTime;

    private List<GameObject> _magazine;
    private AudioData _launchSFX;

    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        _waitForContinuousFireInterval = new WaitForSeconds(_minFireInterval);
        _waitForFireInterval = new WaitForSeconds(_maxFireInterval);
        _waitForBeamCooldownTime = new WaitForSeconds(_beamCooldownTime);
        _magazine = new List<GameObject>(_projectiles.Length);
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void OnEnable()
    {
        _isBeamReady = false;
        StartCoroutine(nameof(BeamCooldownCoroutine));
        base.OnEnable();
    }

    void ActivateBeamWeapon()
    {
        _isBeamReady = false;
        _animator.SetTrigger(_launchBeamID);
        AudioManager.Instance.PlayRandomSFX(_beamChargingSFX);
    }

    void LoadProjectiles()
    {
        _magazine.Clear();
        if (Physics2D.OverlapBox(_playerDetectionTransform.position,_playerDetectionSize,0f,_playerLayer))
        {
            _magazine.Add(_projectiles[0]);
            _launchSFX = _projectileLaunchSFX[0];
        }
        else
        {
            if (Random.value<.5f)
            {
                _magazine.Add(_projectiles[1]);
                _launchSFX = _projectileLaunchSFX[1];
            }
            else
            {
                for (int i = 2; i < _projectiles.Length; i++)
                {
                    _magazine.Add(_projectiles[i]);
                }
                _launchSFX = _projectileLaunchSFX[2];
            }
        }
    }

    protected override IEnumerator RandomlyFireCoroutine()
    {
        while (isActiveAndEnabled)
        {
            if (_isBeamReady)
            {
                ActivateBeamWeapon();
                StartCoroutine(nameof(ChasingPlayerCoroutine));
                yield break;
            }
            if (GameManager.GameState == GameState.GameOver) yield break;
            yield return _waitForFireInterval;
            yield return StartCoroutine(nameof(ContinuousFireCoroutine));
        }
    }

    IEnumerator ContinuousFireCoroutine()
    {
        _muzzleVFX.Play();
        LoadProjectiles();
        //这个协程是连续发射子弹
        //_minFireInterval用以连续发射子弹时 两次发射子弹的间隔
        //_continuousFireDuration则是确定连续射击的持续时间
        float continuousFireTimer = 0f;
        while (continuousFireTimer < _continuousFireDuration)
        {
            foreach (var projectile in _magazine)
            {
                PoolManager.Release(projectile, _muzzle.position);
            }
            continuousFireTimer += _minFireInterval;
            AudioManager.Instance.PlayRandomSFX(_launchSFX);

            yield return _waitForContinuousFireInterval;
        }
        _muzzleVFX.Stop();
    }

    IEnumerator BeamCooldownCoroutine()
    {
        yield return _waitForBeamCooldownTime;
        _isBeamReady = true;
    }

    IEnumerator ChasingPlayerCoroutine()
    {
        while (isActiveAndEnabled)
        {
            targetPosition.x = ViewPort.Instance.Max_X - _paddingX;
            targetPosition.y = _playerTransform.position.y;
            yield return null;
        }
    }


    //AnimationEvent
    void AnimationEventLaunchBeam()
    {
        AudioManager.Instance.PlayRandomSFX(_beamLaunchSFX);
    }

    void AnimationEventStopBeam()
    {
        StopCoroutine(nameof(ChasingPlayerCoroutine));
        StartCoroutine(nameof(BeamCooldownCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_playerDetectionTransform.position, _playerDetectionSize);
    }


}
