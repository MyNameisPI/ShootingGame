using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBeam : MonoBehaviour
{
    [SerializeField] private float _damage = 50f;
    [SerializeField] private GameObject _hitVFX;

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        //使用TryGetComponent 确定是否存在则组件 会比较节约性能
        if (collision.gameObject.TryGetComponent(out Player character) && collision.gameObject.activeInHierarchy)
        {
            character.TakaDamage(_damage);
            //使用collision2D.GetContact()获取碰撞点
            //碰撞点会返回各种信息 其中我们主要使用point和normal 
            //使用Quaternion.LookRotation()将生成的物体的Z轴指向法线方向
            PoolManager.Release(_hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
        }
    }
}
