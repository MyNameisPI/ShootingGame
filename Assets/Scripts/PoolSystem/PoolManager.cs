using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Pool[] _playerProjectilePools;
    [SerializeField] private Pool[] _enemyProjectilePools;
    [SerializeField] private Pool[] _vfxPools;
    [SerializeField] private Pool[] _enemyPrefabs;
    [SerializeField] private Pool[] _lootItems;
    private static Dictionary<GameObject, Pool> _poolDic = new Dictionary<GameObject, Pool>();
    private void Awake()
    {
        Initialized(_enemyPrefabs);
        Initialized(_playerProjectilePools);
        Initialized(_enemyProjectilePools);
        Initialized(_vfxPools);
        Initialized(_lootItems);
    }
    private void OnDestroy()
    {
        _poolDic.Clear();
#if UNITY_EDITOR
        CheckPoolSize(_enemyPrefabs);
        CheckPoolSize(_playerProjectilePools);
        CheckPoolSize(_enemyProjectilePools);
        CheckPoolSize(_vfxPools);
        CheckPoolSize(_lootItems);
#endif
    }


    void CheckPoolSize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
            if (pool.RuntimeSize>pool.Size)
            {
                Debug.LogWarning(string.Format("Pool:{0} has a runtime size {1} bigger than its initial size{2}",
                    pool.Prefab.name,
                    pool.RuntimeSize,
                    pool.Size));
            }
        }
    }

    void Initialized(Pool[] pools)
    {
        for (int i = 0; i < pools.Length; i++)
        {
            #if UNITY_EDITOR
            if (_poolDic.ContainsKey(pools[i].Prefab))
            {
                Debug.LogError("same prefab in multiple pools! Prefab:" + pools[i].Prefab.name);
                continue;
            }
            #endif

            _poolDic.Add(pools[i].Prefab, pools[i]);
            Transform poolParent = new GameObject("Pool: " + pools[i].Prefab.name).transform;
            poolParent.parent = transform;
            pools[i].Initialize(poolParent);
        }
    }

    /// <summary>
    /// <para>Release a specified prepared GameObject in the pool at specified position,rotation and scale</para>
    /// <para>根据传入的prefab参数，rotation参数和localScale参数，在position参数位置释放对象池中预备好的游戏对象</para>
    /// </summary>
    /// <param name="prefab">
    /// <para>Specified gameObject prefab</para>
    /// <para>指定的游戏对象预制体</para>
    /// <returns></returns>
    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!_poolDic.ContainsKey(prefab))
        {
            Debug.LogError("PoolManager could not find prefab :" + prefab.name);
            return null;
        }
#endif

        return _poolDic[prefab].PrepareGameObject();
    }

    public static GameObject Release(GameObject prefab,Vector3 position)
    {
#if UNITY_EDITOR
        if (!_poolDic.ContainsKey(prefab))
        {
            Debug.LogError("PoolManager could not find prefab :" + prefab.name);
            return null;
        }
#endif

        return _poolDic[prefab].PrepareGameObject(position);
    }

    public static GameObject Release(GameObject prefab,Vector3 position,Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!_poolDic.ContainsKey(prefab))
        {
            Debug.LogError("PoolManager could not find prefab :" + prefab.name);
            return null;
        }
#endif

        return _poolDic[prefab].PrepareGameObject(position, rotation);
    }

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation,Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!_poolDic.ContainsKey(prefab))
        {
            Debug.LogError("PoolManager could not find prefab :" + prefab.name);
            return null;
        }
#endif

        return _poolDic[prefab].PrepareGameObject(position, rotation,localScale);
    }
}
