using UnityEngine;
using System.Collections;

public class StageOriNativeTalk2 : StateMachineBehaviour
{
    private bool said_message = false;


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.9f && !said_message)
        {
            FlowManager.Instance.native_messenger.Message("I totally understand this reply!");
            said_message = true;
        }
        if (FlowManager.Continue())
        {
            animator.SetTrigger("Next");
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FlowManager.Instance.native_messenger.Clear();

        // reset
        said_message = false;
    }

}
