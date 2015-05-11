using UnityEngine;
using System.Collections;

public class StageOriNativeTalk2 : StateMachineBehaviour
{
    private bool said_message = false;


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.9f && !said_message)
        {
            FlowManager.Instance.native_messenger.Message("\"我完全看得懂！\" \nTranslation from Chinese: \n \"I totally understand this reply!\"");
            said_message = true;
        }
        if (FlowManager.InputContinue())
        {
            FlowManager.AnimatorNextScene();
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FlowManager.Instance.native_messenger.Clear();

        // next page
        FlowManager.Instance.outro_page.TransitionIn(0.5f);

        // reset
        said_message = false;
    }

}
