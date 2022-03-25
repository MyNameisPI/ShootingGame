using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    private Text _text;

    private void Awake()
    {
        _text = GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        _text.text = "- WAVE " + EnemyManager.Instance.WaveNumber + " -";
    }
}
