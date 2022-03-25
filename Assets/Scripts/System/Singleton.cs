using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:Component
{
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        Debug.Log(gameObject.name + "≥ı ºªØ");
        Instance = this as T;
    }
}
