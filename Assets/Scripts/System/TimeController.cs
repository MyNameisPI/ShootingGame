using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : Singleton<TimeController>
{
    [SerializeField] [Range(0,1)] private float _bulletTimeScale = 0.1f;

    private float _defaulFixedDeltaTime;

    private float _lerpTime;

    private float _timeScaleBeforePaused;

    protected override void Awake()
    {
        base.Awake();
        _defaulFixedDeltaTime = Time.fixedDeltaTime;
    }
    public void BulletTime(float duration)
    {
        Time.timeScale = _bulletTimeScale;
        Time.fixedDeltaTime = _defaulFixedDeltaTime * Time.timeScale;
        StartCoroutine(SlowOutCoroutine(duration));
    }

    public void BulletTime(float inDuration,float outDuration)
    {
        StartCoroutine(SlowInAndOutCoroutine(inDuration, outDuration));
    }

    public void BulletTime(float inDuration,float keepingDuration, float outDutaion)
    {
        StartCoroutine(SlowInKeepAndOutDuration(inDuration, keepingDuration, outDutaion));
    }

    public void SlowIn(float duration)
    {
        StartCoroutine(SlowInCoroutine(duration));
    }

    public void SlowOut(float duration)
    {
        StartCoroutine(SlowOutCoroutine(duration));
    }

    public void OnPause()
    {
        _timeScaleBeforePaused = Time.timeScale;
        Time.timeScale = 0;
    }

    public void UnPause()
    {
        Time.timeScale = _timeScaleBeforePaused;
    }


    IEnumerator SlowInKeepAndOutDuration(float inDuration, float keepDuration, float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine(inDuration));
        yield return new WaitForSecondsRealtime(keepDuration);
        StartCoroutine(SlowOutCoroutine(outDuration));
    }
    IEnumerator SlowInAndOutCoroutine(float inDuration,float outDuration)
    {
        yield return StartCoroutine(SlowInCoroutine((inDuration)));
        StartCoroutine(SlowOutCoroutine(outDuration));
    }


    IEnumerator SlowOutCoroutine(float duration)
    {
        _lerpTime = 0f;
        while (_lerpTime < 1f)
        {
            if (GameManager.GameState !=GameState.Pause)
            {
                _lerpTime += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(_bulletTimeScale, 1f, _lerpTime);
                Time.fixedDeltaTime = _defaulFixedDeltaTime * Time.timeScale;
            }
            yield return null;
        }
    }

    IEnumerator SlowInCoroutine(float duration)
    {
        _lerpTime = 0f;
        while (_lerpTime < 1f)
        {
            if (GameManager.GameState !=GameState.Pause)
            {
                _lerpTime += Time.unscaledDeltaTime / duration;
                Time.timeScale = Mathf.Lerp(1f, _bulletTimeScale, _lerpTime);
                Time.fixedDeltaTime = _defaulFixedDeltaTime * Time.timeScale;
            }
             yield return null;
        }
    }
}
