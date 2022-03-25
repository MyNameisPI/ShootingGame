using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_Aiming : Projectile
{
    private void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void OnEnable()
    {
        StartCoroutine(MoveDirection());
        base.OnEnable();
    }

    IEnumerator MoveDirection()
    {
        yield return null;
        if (_target.activeSelf)
        {
            _moveDirection = (_target.transform.position - transform.position).normalized;
        }
    }
}
