using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] private LootSetting[] _lootSettings;

    public void Spawn(Vector2 positon)
    {
        foreach (var item in _lootSettings)
        {
            item.Spawn(positon + Random.insideUnitCircle);
        }
    }
}
