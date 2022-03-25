using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour
{
    [SerializeField] private float _minBallisticAngle= -50f;
    [SerializeField] private float _maxBallisticAngle= 50f;

    [SerializeField]private Projectile _projectile;
    private float _ballisticAngle;

    private Vector3 _moveDirection;
    public IEnumerator HomingCoroutine(GameObject target)
    {
        _ballisticAngle = Random.Range(_minBallisticAngle, _maxBallisticAngle);
        while (gameObject.activeSelf)
        {
            if (target.activeSelf)
            {
                _moveDirection = target.transform.position - transform.position;
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg, Vector3.forward);
                transform.rotation *= Quaternion.Euler(0f, 0f, _ballisticAngle);
                _projectile.Move();
            }
            else
            {
                _projectile.Move();
            }
            yield return null;
        }
    }
}
