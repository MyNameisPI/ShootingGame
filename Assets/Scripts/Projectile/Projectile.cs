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
        //ʹ��TryGetComponent ȷ���Ƿ��������� ��ȽϽ�Լ����
        if (collision.gameObject.TryGetComponent<Character>(out Character character) && collision.gameObject.activeInHierarchy)
        { 
            character.TakaDamage(_damage);
            //ʹ��collision2D.GetContact()��ȡ��ײ��
            //��ײ��᷵�ظ�����Ϣ ����������Ҫʹ��point��normal 
            //ʹ��Quaternion.LookRotation()�����ɵ������Z��ָ���߷���
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
