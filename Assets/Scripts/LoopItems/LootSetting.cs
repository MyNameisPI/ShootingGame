using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class LootSetting
{
    public GameObject _prefab;
    [Range(0f, 100f)] public float _dropPrecentage;

    public void Spawn(Vector3 position)
    {
        if (Random.Range(0f,100f) <= _dropPrecentage)
        {
            PoolManager.Release(_prefab, position);
        }
    }
}
