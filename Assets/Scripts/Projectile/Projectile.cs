using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private GameObject _hitVFX;
    [SerializeField] private AudioData[] _hitSFX;
    [SerializeField] private float _damage=1;
    [SerializeField]protected float _moveSpeed;
    [SerializeField]protected Vector2 _moveDirection;

    protected GameObject _target;

    protected virtual void OnEnable()
    {
        StartCoroutine(MoveDirectly());
    }
    IEnumerator MoveDirectly()
    {
        while (gameObject.activeSelf)
        {
            Move();
            yield return null;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //使用TryGetComponent 确定是否存在则组件 会比较节约性能
        if (collision.gameObject.TryGetComponent<Character>(out Character character) && collision.gameObject.activeInHierarchy)
        { 
            character.TakaDamage(_damage);
            //使用collision2D.GetContact()获取碰撞点
            //碰撞点会返回各种信息 其中我们主要使用point和normal 
            //使用Quaternion.LookRotation()将生成的物体的Z轴指向法线方向
            PoolManager.Release(_hitVFX,collision.GetContact(0).point,Quaternion.LookRotation(collision.GetContact(0).normal));
            AudioManager.Instance.PlayRandomSFX(_hitSFX);
            gameObject.SetActive(false);
        }
    }

    protected void SetTarget(GameObject target)
    {
        this._target = target;
    }

    public void Move()
    {
        transform.Translate(_moveDirection * _moveSpeed * Time.deltaTime);
    }

}
