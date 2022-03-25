using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressedBehaviour : StateMachineBehaviour
{
    public static Dictionary<string, System.Action> _buttonFunctionTable;

    private void Awake()
    {
        _buttonFunctionTable = new Dictionary<string, System.Action>();
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UIInput.Instance.DisableAllUIInput();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _buttonFunctionTable[animator.gameObject.name].Invoke();
    }

}
