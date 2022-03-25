using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] private EnergyBar _energyBar;
    [SerializeField] private float _overDriveInterval=.1f;

    public const int MAXENERGY = 100;
    public const int PERCENT = 1;

    private int _energy;
    private bool _available = true;
    private WaitForSeconds _waitForOverDriveInterval;

    protected override void Awake()
    {
        base.Awake();
        _waitForOverDriveInterval = new WaitForSeconds(_overDriveInterval);
    }
    private void OnEnable()
    {
        PlayerOverDrive.overDriveON += PlayerOverDriveON;
        PlayerOverDrive.overDriveOFF += PlayerOverDriveOFF;
    }
    private void OnDisable()
    {
        PlayerOverDrive.overDriveON -= PlayerOverDriveON;
        PlayerOverDrive.overDriveOFF -= PlayerOverDriveOFF;
    }

    

    private void Start()
    {
        _energyBar.Initialized(_energy, MAXENERGY);
        Obtain(MAXENERGY);
    }

    public void Obtain(int value)
    {
        if (_energy > MAXENERGY||!_available||!gameObject.activeSelf) return;
        _energy += value;
        _energy = Mathf.Clamp(_energy, 0, MAXENERGY);
        _energyBar.UpdateStatus(_energy, MAXENERGY);
    }

    public void Use(int value)
    {
        _energy -= value;
        _energyBar.UpdateStatus(_energy, MAXENERGY);
        if (_energy ==0&&!_available)
        {
            PlayerOverDrive.overDriveOFF.Invoke();
        }
    }

    public bool IsEnough(int value) => _energy >= value;

    private void PlayerOverDriveON()
    {
        _available = false;
        StartCoroutine(nameof(KeepUseEnergy));
    }
    private void PlayerOverDriveOFF()
    {
        _available = true;
        StopCoroutine(nameof(KeepUseEnergy));
    }

    IEnumerator KeepUseEnergy()
    {
        while (gameObject.activeInHierarchy&&_energy>0)
        {     
            yield return _waitForOverDriveInterval;
            Use(PERCENT);
        }
    }

}
