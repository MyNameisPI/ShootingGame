using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeam : MonoBehaviour
{
    [SerializeField] private float _damage = 50f;
    [SerializeField] private GameObject _hitVFX;

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        //ʹ��TryGetComponent ȷ���Ƿ��������� ��ȽϽ�Լ����
        if (collision.gameObject.TryGetComponent(out Player character) && collision.gameObject.activeInHierarchy)
        {
            character.TakaDamage(_damage);
            //ʹ��collision2D.GetContact()��ȡ��ײ��
            //��ײ��᷵�ظ�����Ϣ ����������Ҫʹ��point��normal 
            //ʹ��Quaternion.LookRotation()�����ɵ������Z��ָ���߷���
            PoolManager.Release(_hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
        }
    }
}
