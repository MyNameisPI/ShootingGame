using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : StatusBar_HUD
{
    protected override void SetPercentText()
    {
        //_percentText.text = (_targetFillAmount * 100f).ToString("f2") + "%";
        _percentText.text = _targetFillAmount.ToString("P");
    }
}
