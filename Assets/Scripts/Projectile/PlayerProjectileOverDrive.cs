using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileOverDrive : PlayerProjectile
{
    [SerializeField] private ProjectileGuidanceSystem _guidSystem;

    protected override void OnEnable()
    {
        SetTarget(EnemyManager.Instance.RandomEnemy);
        transform.rotation = Quaternion.identity;
        if (_target==null)
        {
            base.OnEnable();

        }
        else
        {
            StartCoroutine(_guidSystem.HomingCoroutine(_target));
        }
    }
}
