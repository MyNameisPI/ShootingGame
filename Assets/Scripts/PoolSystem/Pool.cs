using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Pool
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _size;

    private Queue<GameObject> _queue;
    private Transform _parent;

    public GameObject Prefab => _prefab;
    public int Size => _size;
    public int RuntimeSize => _queue.Count;

    GameObject Copy()
    {
        var copy = GameObject.Instantiate(_prefab);
        copy.transform.SetParent(_parent);
        copy.gameObject.SetActive(false);
        return copy;
    }

    public void Initialize(Transform parent)
    {
        _queue = new Queue<GameObject>();
        _parent = parent;

        for (int i = 0; i < _size; i++)
        {
            _queue.Enqueue(Copy());
        }
    }

    GameObject AvailableGameObject()
    {
        GameObject availableGameObject = null;
        if (_queue.Count>0&&!_queue.Peek().activeSelf)
        {
            availableGameObject = _queue.Dequeue();
        }
        else
        {
            availableGameObject = Copy();
        }
        _queue.Enqueue(availableGameObject);
        return availableGameObject;
    }

    public GameObject PrepareGameObject()
    {
        GameObject prepareGameObject = AvailableGameObject();
        prepareGameObject.SetActive(true);
        return prepareGameObject;
    }

    public GameObject PrepareGameObject(Vector3 position)
    {
        GameObject prepareGameObject = AvailableGameObject();
        prepareGameObject.SetActive(true);
        prepareGameObject.transform.position = position;
        return prepareGameObject;
    }
        public GameObject PrepareGameObject(Vector3 position,Quaternion rotation)
    {
        GameObject prepareGameObject = AvailableGameObject();
        prepareGameObject.SetActive(true);
        prepareGameObject.transform.position = position;
        prepareGameObject.transform.rotation = rotation;
        return prepareGameObject;
    }
    public GameObject PrepareGameObject(Vector3 position, Quaternion rotation,Vector3 localScale)
    {
        GameObject prepareGameObject = AvailableGameObject();
        prepareGameObject.SetActive(true);
        prepareGameObject.transform.position = position;
        prepareGameObject.transform.rotation = rotation;
        prepareGameObject.transform.localScale = localScale;
        return prepareGameObject;
    }

}
