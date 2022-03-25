using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{
    private static Text _amoutText;
    private static Image _coolDownImage;

    private void Awake()
    {
        _amoutText = transform.Find("Amout Text").GetComponent<Text>();
        _coolDownImage = transform.Find("Cool Down").GetComponent<Image>();
    }
    public static void UpdateAmountText(int amount) => _amoutText.text = amount.ToString();
    public static void UpdateCoolDownImage(float fillAmount) => _coolDownImage.fillAmount = fillAmount;
}
