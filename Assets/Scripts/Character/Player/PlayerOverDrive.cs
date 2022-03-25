using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerOverDrive : MonoBehaviour
{
    public static UnityAction overDriveON = delegate { };
    public static UnityAction overDriveOFF=delegate { };

    [SerializeField] GameObject _triggerVFX;
    [SerializeField] GameObject _engineVFXNormal;
    [SerializeField] GameObject _engineVFXOverDrive;

    [SerializeField] AudioData _onSFX;
    [SerializeField] AudioData _offSFX;
    private void OnEnable()
    {

        overDriveON += OverDriveOn;
        overDriveOFF += OverDriveOff;
    }
    private void OnDisable()
    {
        overDriveON -= OverDriveOn;
        overDriveOFF -= OverDriveOff;
    }

    private void OverDriveOn()
    {
        Debug.Log("OverDrive ON");
        _triggerVFX.SetActive(true);
        _engineVFXNormal.SetActive(false);
        _engineVFXOverDrive.SetActive(true);
        AudioManager.Instance.PlayRandomSFX(_onSFX);
    }

    private void OverDriveOff()
    {
        Debug.Log("OverDrive OFF");
        // 该特效自己会销毁自己 所以不用写_triggerVFX.SetActive(true);
        _engineVFXNormal.SetActive(true);
        _engineVFXOverDrive.SetActive(false);
        AudioManager.Instance.PlayRandomSFX(_offSFX);
    }
    
}
