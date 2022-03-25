using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{

    [SerializeField] private Image _fillImageBack;
    [SerializeField] private Image _fillImageFront;

    [SerializeField] private float _fillTime;
    [SerializeField] private float _fillDelay;
    [SerializeField] private bool _isDelayFill;

    private float _currentFillAmount;
    protected float _targetFillAmount;
    private float t;
    private WaitForSeconds _waitForfillDelay;
    private Coroutine _bufferedFillCoroutine;


    private void Awake()
    {
        if (TryGetComponent<Canvas>(out Canvas canvas))
        {
            canvas.worldCamera = Camera.main;
        }
        _waitForfillDelay = new WaitForSeconds(_fillDelay);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public virtual void Initialized(float currentValue,float maxValue)
    {
        _currentFillAmount = currentValue / maxValue;
        _targetFillAmount = _currentFillAmount;
        _fillImageBack.fillAmount = _currentFillAmount;
        _fillImageFront.fillAmount = _currentFillAmount;
    }

    public void UpdateStatus(float currentValue, float maxValue)
    {
        _targetFillAmount = currentValue / maxValue;
        //当状态值减小时
        //前面的图片立即更新为目标值  后面的图片慢慢的更新为目标值
        if (_bufferedFillCoroutine != null) StopCoroutine(_bufferedFillCoroutine);
        if (_currentFillAmount > _targetFillAmount)
        {
            _fillImageFront.fillAmount = _targetFillAmount;
            _bufferedFillCoroutine = StartCoroutine(BufferedFillingCoroutine(_fillImageBack));
        }
        //当状态值增大时
        //后面的图片立即更新为目标值  前面的图片慢慢的更新为目标值
        else if (_currentFillAmount < _targetFillAmount)
        {
            _fillImageBack.fillAmount = _targetFillAmount;
            _bufferedFillCoroutine = StartCoroutine(BufferedFillingCoroutine(_fillImageFront));
        }

    }

    protected virtual IEnumerator BufferedFillingCoroutine(Image image)
    {
        if (_isDelayFill)
        {
            yield return _waitForfillDelay;
        }
        t = 0f;
        while (t<1f)
        {
            t += Time.deltaTime / _fillTime;
            _currentFillAmount = Mathf.Lerp(_currentFillAmount, _targetFillAmount, t);
            image.fillAmount = _currentFillAmount;
            yield return null;
        }
    }
}
