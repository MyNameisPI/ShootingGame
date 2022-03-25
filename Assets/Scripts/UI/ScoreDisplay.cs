using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    private static Text _scoreText;

    private void Awake()
    {
        _scoreText = GetComponent<Text>();
    }

    private void Start()
    {
        ScoreManager.Instance.ResetScore();
    }
    public static void UpdateScore(int score)
    {
        _scoreText.text = score.ToString();
    }

    public static void ScaleText(Vector3 targetScale) => _scoreText.rectTransform.localScale = targetScale;
}
