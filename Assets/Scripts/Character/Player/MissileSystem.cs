using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] private int _defaultAmount=5;
    [SerializeField] private float _coolTime=1f;
    [SerializeField] private GameObject _missilePrefab;
    [SerializeField] private AudioData _launchSFX;

    private bool _isReady=true;
    private int _currentAmount;

    private void Awake()
    {
        _currentAmount = _defaultAmount;
    }
    private void Start()
    {
        MissileDisplay.UpdateAmountText(_currentAmount);
        MissileDisplay.UpdateCoolDownImage(0f);
    }
    public  void Launch(Transform muzzleTF)
    {
        if (_currentAmount == 0||!_isReady) return;
        _isReady = false;
        PoolManager.Release(_missilePrefab, muzzleTF.position);
        AudioManager.Instance.PlaySFX(_launchSFX);
        _currentAmount--;
        MissileDisplay.UpdateAmountText(_currentAmount);
        if (_currentAmount==0)
        {
            MissileDisplay.UpdateCoolDownImage(1f);
        }
        else
        {
            StartCoroutine(nameof(CoolDownCoroutine));
        }
    }

    public void PickUp()
    {
        _currentAmount++;
        MissileDisplay.UpdateAmountText(_currentAmount);
        if (_currentAmount==1)
        {
            MissileDisplay.UpdateCoolDownImage(0f);
            _isReady = true;
        }
    }

    IEnumerator CoolDownCoroutine()
    {
        var coolDownValue = _coolTime;
        while (coolDownValue>0f)
        {
            MissileDisplay.UpdateCoolDownImage(coolDownValue / _coolTime);
            coolDownValue = Mathf.Clamp(coolDownValue -= Time.deltaTime, 0, _coolTime);

            yield return null;
        }
        _isReady = true;
    }
}
