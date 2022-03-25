using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar_HUD : StatusBar
{
    [SerializeField] protected Text _percentText;

    protected virtual void SetPercentText()
    {
        //_percentText.text = Mathf.RoundToInt(_targetFillAmount * 100)+"%";
        _percentText.text = _targetFillAmount.ToString("P0");
    }

    public override void Initialized(float currentValue, float maxValue)
    {
        base.Initialized(currentValue, maxValue);
        SetPercentText();
    }

    protected override IEnumerator BufferedFillingCoroutine(Image image)
    {
        SetPercentText();
        return base.BufferedFillingCoroutine(image);
    }
}
