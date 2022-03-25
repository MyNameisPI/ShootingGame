using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    [SerializeField] private Image _transitionImage;
    [SerializeField] private float _fadeTime = 3.5f;

    Color _color;

    const string GAMEPLAY = "GamePlay";
    const string MAINMENU = "MainMenu";
    const string SCORING = "Scoring";

    void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadingGamePlay()
    {
        //��ֹ��ͻ ����һ����Э��ʱ �Ȱ�֮ǰ��Э�̶�ֹͣ
        StopAllCoroutines();
        StartCoroutine(LoadCoroutine(GAMEPLAY));
    }

    public void LoadingMainMenu()
    {
        StopAllCoroutines();
        StartCoroutine(LoadCoroutine(MAINMENU));
    }

    public void LoadingScoring()
    {
        StopAllCoroutines();
        StartCoroutine(LoadCoroutine(SCORING));
    }

    IEnumerator LoadCoroutine(string sceneName)
    {
        var loadingOperation =  SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;

        //Fade Out
        _transitionImage.gameObject.SetActive(true);
        while (_color.a < 1f)
        {
            _color.a = Mathf.Clamp01(_color.a += Time.unscaledDeltaTime / _fadeTime);
            _transitionImage.color = _color;
            yield return null;
        }
        //�ȴ������������ ����0.9����Ϊ ֻ�е� ����������ʱ�Ż�ʱ1
        yield return new WaitUntil(() => loadingOperation.progress >= .9f);
        loadingOperation.allowSceneActivation = true;
        //Fade In
        while (_color.a > 0f)
        {
            _color.a = Mathf.Clamp01(_color.a -= Time.unscaledDeltaTime / _fadeTime);
            _transitionImage.color = _color;
            yield return null;
        }
        _transitionImage.gameObject.SetActive(false);
    }


}
