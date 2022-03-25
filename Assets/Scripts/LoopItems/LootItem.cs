using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootItem : MonoBehaviour
{
    [SerializeField] private float _minSpeed=5f;
    [SerializeField] private float _maxSpeed=15f;
    [SerializeField] protected AudioData _defaultPickUpSFX;

    protected Player _player;

    private Animator _animatior;
    private int _pickUpStateID = Animator.StringToHash("PickUp");

    protected AudioData _pickUpSFX;

    protected Text _lootMessage;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _animatior = GetComponent<Animator>();
        _pickUpSFX = _defaultPickUpSFX;
        _lootMessage = GetComponentInChildren<Text>(true);
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(MoveCoroutine));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickUp();
    }
    IEnumerator MoveCoroutine()
    {
        float speed = Random.Range(_minSpeed, _maxSpeed);
        Vector3 direction = Vector3.left;

        while (true)
        {
            if (_player.isActiveAndEnabled)
            {
                direction = (_player.transform.position - transform.position).normalized;
                transform.Translate(direction*speed*Time.deltaTime);
            }
                yield return null;
        }
    }

    protected virtual void PickUp()
    {
        StopAllCoroutines();
        _animatior.Play(_pickUpStateID);
        AudioManager.Instance.PlayRandomSFX(_pickUpSFX);
    }
}
