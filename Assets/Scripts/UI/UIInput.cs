using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIInput : Singleton<UIInput>
{
    [SerializeField]private PlayerInput _playerInput;

    private InputSystemUIInputModule _UIinputModule;
    protected override void Awake()
    {
        base.Awake();
        _UIinputModule = GetComponent<InputSystemUIInputModule>();
        _UIinputModule.enabled = false;
    }

    public void SelectUI(Selectable UIObject)
    {
        UIObject.Select();
        UIObject.OnSelect(null);
        _UIinputModule.enabled = true;
    }

    public void DisableAllUIInput()
    {
        _playerInput.DisableAllInput();
        _UIinputModule.enabled = false;
    }
}
