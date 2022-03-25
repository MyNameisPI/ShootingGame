using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private StatusBar_HUD _healthStatusBar_HUD;
    [SerializeField] private bool _regenerateHealth = true;
    [SerializeField] private float _healthRegenerateTime;
    [SerializeField] [Range(0,1)] private float _healthRegeneratePercent;
    [Header("-----Input-----")]
    [SerializeField] private PlayerInput _playerInput;
    [Header("-----Move-----")]
    [SerializeField] private float _moveSpeed=10f;
    [SerializeField] private float _moveRotation= 30f;
    [SerializeField] private float _lerpTime=3f;
    [SerializeField] private float _fireInterval;


    [Header("-----Fire-----")]
    [SerializeField] [Range(0,2)]private int _weaponPower;

    [SerializeField] private GameObject _projectileProfab01;
    [SerializeField] private GameObject _projectileProfab02;
    [SerializeField] private GameObject _projectileProfab03;
    [SerializeField] private GameObject _projectileOverDrive;
    [SerializeField] private ParticleSystem _muzzleVFX;

    [SerializeField] private Transform _muzzleTop;
    [SerializeField] private Transform _muzzleMiddle;
    [SerializeField] private Transform _muzzleBottom;
    [SerializeField] private AudioData _projectileLaunchSFX;


    [Header("-----Dodge-----")]
    [SerializeField][Range(0,100)] private int _dodgeEnergyCost = 25;
    [SerializeField] private float _maxRoll = 720;
    [SerializeField] private float _rollSpeed = 360;
    [SerializeField] private Vector3 _dodgeScale = new Vector3(.5f, .5f, .5f);
    [SerializeField] private AudioData _dodgeSFX;

    [Header("-----OverDrive-----")]
    [SerializeField] private float _overDriveSpeedFactor = 1.2f;
    [SerializeField] private float _overDriveFireFactor = 1.2f;
    [SerializeField] private int _overDriveDodgeFactor = 2;
    [SerializeField] private float _bulletTimeDuration = 1f;

    private MissileSystem _missileSystem;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private Coroutine _moveCoroutine;
    private Coroutine _regenerateCoroutine;
    private WaitForSeconds _waitForFireInterval;
    private WaitForSeconds _waitForOverDirveFireInterval;
    private WaitForSeconds _waitForRegenerateTime;
    private WaitForSeconds _waitForDecelerationTime;
    private WaitForFixedUpdate _waitForFixedUpdate;
    private WaitForSeconds _waitForInvincibleTime;
    private float _currentRoll;
    private float _dodgeDuration;
    private bool _isDodging = false;
    private bool _isOverDrive = false;

    private float _invincibleTime = 1f;

    private float t;
    private Vector2 _velocity;
    private Quaternion _rotation;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _missileSystem = GetComponent<MissileSystem>();
        _dodgeDuration = _maxRoll / _rollSpeed;
        _waitForFireInterval = new WaitForSeconds(_fireInterval);
        _waitForRegenerateTime = new WaitForSeconds(_healthRegenerateTime);
        _waitForFixedUpdate = new WaitForFixedUpdate();
        _waitForOverDirveFireInterval = new WaitForSeconds(_fireInterval / _overDriveFireFactor);
        _waitForDecelerationTime = new WaitForSeconds(_lerpTime);
        _waitForInvincibleTime = new WaitForSeconds(_invincibleTime);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        _playerInput.MoveEvent += Move;
        _playerInput.MoveCanceledEvent += StopMove;
        _playerInput.FireEvent += Fire;
        _playerInput.FireCanceledEvent += StopFire;
        _playerInput.DodgeEvent += OnDodge;
        _playerInput.OverDriveEvent += OnOverDrive;
        PlayerOverDrive.overDriveON += OverDriveON;
        PlayerOverDrive.overDriveOFF += OverDriveOFF;
        _playerInput.LaunchMissileEvent += LauchMissile;
    }


    private void OnDisable()
    {
        _playerInput.MoveEvent -= Move;
        _playerInput.MoveCanceledEvent -= StopMove;
        _playerInput.FireEvent -= Fire;
        _playerInput.FireCanceledEvent -= StopFire;
        _playerInput.DodgeEvent -= OnDodge;
        _playerInput.OverDriveEvent -= OnOverDrive;
        PlayerOverDrive.overDriveON -= OverDriveON;
        PlayerOverDrive.overDriveOFF -= OverDriveOFF;
        _playerInput.LaunchMissileEvent -= LauchMissile;
        StopAllCoroutines();
    }
    private void Start()
    {
        _playerInput.EnableGamePlayInput();
        _healthStatusBar_HUD.Initialized(_health, _maxHealth);
    }


    #region Health

    public bool IsFullHealth => _health == _maxHealth;
    public override void TakaDamage(float damage)
    {
        base.TakaDamage(damage);
        _healthStatusBar_HUD.UpdateStatus(_health, _maxHealth);
        if (gameObject.activeSelf)
        {
            StartCoroutine(nameof(InvincibleCoroutine));
            if (_regenerateHealth)
            {
                if (_regenerateCoroutine != null) StopCoroutine(_regenerateCoroutine);
                _regenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(_waitForRegenerateTime, _healthRegeneratePercent));
            }
        }
    }

    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        _healthStatusBar_HUD.UpdateStatus(_health, _maxHealth);
    }

    public override void Die()
    {
        GameManager.GameState = GameState.GameOver;
        GameManager.Instance.OnGameOver.Invoke();
        _healthStatusBar_HUD.UpdateStatus(0f, _maxHealth);
        base.Die();
    }

    IEnumerator InvincibleCoroutine()
    {
        _collider.isTrigger = true;
        yield return _waitForInvincibleTime;
        _collider.isTrigger = false;

    }
    #endregion


    #region Move
    private void Move(Vector2 moveInput)
    {
        if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
        _moveCoroutine = StartCoroutine(MoveCoroutine(_lerpTime, moveInput.normalized * _moveSpeed, Quaternion.AngleAxis(_moveRotation*moveInput.y, Vector3.right)));
        StopCoroutine(nameof(DecelerationCoroutine));
        StartCoroutine(nameof(MovePositionLimitCoroutine));
    }

    private void StopMove()
    {
        //通过Coroutine类变量停止上一个的协程
        if (_moveCoroutine != null) StopCoroutine(_moveCoroutine);
        //每次开始一个新协程时 将其赋值给我们创建的Coroutine类变量
        _moveCoroutine = StartCoroutine(MoveCoroutine(_lerpTime, Vector2.zero, Quaternion.identity));
        //停止时有个BUG：当移动限制协程关闭时 移动协程产生减速效果会使飞机飞出视口
        //所以我们再使用一个协程 当触发停止 移动协程开始减速操作时 开启这个协程 在一定的延迟后在关闭移动限制协程
        //其实就是让减速过程中 移动依旧受到限制 等飞机真正停下来了 才关闭移动限制协程
        StartCoroutine(nameof(DecelerationCoroutine));
    }

    IEnumerator MoveCoroutine(float time,Vector2 targetVelocity,Quaternion targetRotation)
    {
        t = 0f;
        _velocity = _rigidbody.velocity;
        _rotation = transform.rotation;
        while (t<time )
        {
            t += Time.fixedDeltaTime;
            _rigidbody.velocity = Vector2.Lerp(_velocity, targetVelocity, t / time);
            transform.rotation = Quaternion.Lerp(_rotation, targetRotation, t / time);
            yield return _waitForFixedUpdate;
        }
    }

    IEnumerator MovePositionLimitCoroutine()
    {
        
        while (true)
        {
            transform.position = ViewPort.Instance.PlayerMoveablePosition(transform.position);
            yield return null;
        }
    }

    IEnumerator DecelerationCoroutine()
    {
        yield return _waitForDecelerationTime;
        StopCoroutine(nameof(MovePositionLimitCoroutine));
    }
    #endregion


    #region Fire

    public bool IsWeaponPowerFull => _weaponPower == 2;

    public void PowerUp()
    {
        _weaponPower++;
        _weaponPower = Mathf.Clamp(_weaponPower,0, 2);
    }
    private void Fire()
    {
        _muzzleVFX.Play();
        StartCoroutine(nameof(FireCoroutine));
    }

    private void StopFire()
    {
        _muzzleVFX.Stop();
        StopCoroutine(nameof(FireCoroutine));
    }

    IEnumerator FireCoroutine()
    {
        while (true)
        {
            //Instantiate(_projectileProfab, _muzzle.position, Quaternion.identity);
            switch (_weaponPower)
            {
                case 0:
                    //单行射击
                    PoolManager.Release(_isOverDrive?_projectileOverDrive:_projectileProfab01, _muzzleMiddle.position);
                    break;
                case 1:
                    //两行射击
                    PoolManager.Release(_isOverDrive ? _projectileOverDrive : _projectileProfab01, _muzzleTop.position);
                    PoolManager.Release(_isOverDrive ? _projectileOverDrive : _projectileProfab01, _muzzleBottom.position);
                    break;
                case 2:
                    ///散射
                    PoolManager.Release(_isOverDrive ? _projectileOverDrive : _projectileProfab01, _muzzleMiddle.position);
                    PoolManager.Release(_isOverDrive ? _projectileOverDrive : _projectileProfab02, _muzzleTop.position);
                    PoolManager.Release(_isOverDrive ? _projectileOverDrive : _projectileProfab03, _muzzleBottom.position);
                    break;
                default:
                    break;
            }
            AudioManager.Instance.PlaySFX(_projectileLaunchSFX);
            yield return _isOverDrive ? _waitForOverDirveFireInterval : _waitForFireInterval;
        }
    }
    #endregion

    #region Dodge
    private void OnDodge()
    {
        if (_isDodging || !PlayerEnergy.Instance.IsEnough(_dodgeEnergyCost)) return;//判断玩家是否处于闪避状态中 并且能量是否足够
        StartCoroutine(DodgeCoroutine());
    }

    IEnumerator DodgeCoroutine()
    {
        _isDodging = true;
        AudioManager.Instance.PlayRandomSFX(_dodgeSFX);
        //消耗能量
        PlayerEnergy.Instance.Use(_dodgeEnergyCost);
        //闪避时无敌
        _collider.isTrigger = true;
        //float t1 = 0f;
        //float t2 = 0f;
        //while(_currentRoll<_maxRoll)
        //{//让玩家沿着X轴旋转
        //    _currentRoll += _rollSpeed * Time.deltaTime;
        //    transform.rotation = Quaternion.AngleAxis(_currentRoll, Vector3.right);
        //    if (_currentRoll<_maxRoll/2f)
        //    {

        //        t1 += Time.deltaTime/(_dodgeDuration/2f);
        //        transform.localScale = Vector3.Lerp(Vector3.one, _dodgeScale, t1);
        //    }
        //    else
        //    {
        //        t2 += Time.deltaTime / (_dodgeDuration / 2f);
        //        transform.localScale = Vector3.Lerp(_dodgeScale, Vector3.one, t2);
        //    }
        //    yield return null;
        //}

        while (_currentRoll < _maxRoll)
        {
            _currentRoll += _rollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(_currentRoll, Vector3.right);
            //改变玩家的缩放 形成闪避时的纵深效果
            transform.localScale = BezierCurve.QuadraticBezierCurve(Vector3.one, _dodgeScale, Vector3.one, _currentRoll / _maxRoll);
            yield return null;
        }
        _collider.isTrigger = false;
        _currentRoll = 0f;
        _isDodging = false;
    }
    #endregion


    #region OverDrive
    private void OnOverDrive()
    {
        if (!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAXENERGY)) return;
        PlayerOverDrive.overDriveON.Invoke();
    }

    private void OverDriveOFF()
    {
        _isOverDrive = false;
        _dodgeEnergyCost *= _overDriveDodgeFactor;
        _moveSpeed *= _overDriveSpeedFactor;
    }

    private void OverDriveON()
    {
        _isOverDrive = true;
        _dodgeEnergyCost /= _overDriveDodgeFactor;
        _moveSpeed /= _overDriveSpeedFactor;
        TimeController.Instance.BulletTime(_bulletTimeDuration,1f,_bulletTimeDuration);
    }
    #endregion

    #region Missile
    public void PickUpMissile()
    {
        _missileSystem.PickUp();
    }
    private void LauchMissile()
    {
        _missileSystem.Launch(_muzzleMiddle);
    }
    #endregion
}
