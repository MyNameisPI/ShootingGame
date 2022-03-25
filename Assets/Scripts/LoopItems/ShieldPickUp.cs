using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUp : LootItem
{
    [SerializeField] private int _fullHealthScoreBonus= 200;
    [SerializeField] private int _shieldBonus = 20;
    [SerializeField] private AudioData _fullHealthSFX;

    protected override void PickUp()
    {
        if (_player.IsFullHealth)
        {
            _pickUpSFX = _fullHealthSFX;
            _lootMessage.text = $"SCORE + {_fullHealthScoreBonus}";
            ScoreManager.Instance.AddScore(_fullHealthScoreBonus);
        }
        else
        {
            _pickUpSFX = _defaultPickUpSFX;
            _lootMessage.text = $"SHIELD + {_shieldBonus}";
            _player.RestoreHealth(_shieldBonus);
        }
        base.PickUp();
    }
}
