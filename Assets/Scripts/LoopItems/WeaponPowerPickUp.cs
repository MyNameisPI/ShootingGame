using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerPickUp : LootItem
{
    [SerializeField] private int _fullPowerScoreBonus = 200;
    [SerializeField] private AudioData _fullPowerSFX;

    protected override void PickUp()
    {
        if (_player.IsWeaponPowerFull)
        {
            _pickUpSFX = _fullPowerSFX;
            _lootMessage.text = $"SCORE + {_fullPowerScoreBonus}";
            ScoreManager.Instance.AddScore(_fullPowerScoreBonus);
        }
        else
        {
            _pickUpSFX = _defaultPickUpSFX;
            _lootMessage.text = "POWER UP!";
            _player.PowerUp();
        }
        base.PickUp();
    }
}
