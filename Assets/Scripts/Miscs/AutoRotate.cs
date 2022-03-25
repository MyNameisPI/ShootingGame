using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] private float _speed = 360f;
    [SerializeField] private Vector3 _angle;

    private void Update()
    {
        transform.Rotate(_angle * _speed * Time.deltaTime);
    }
}
