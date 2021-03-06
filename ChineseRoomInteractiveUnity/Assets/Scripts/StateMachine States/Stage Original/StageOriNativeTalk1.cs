﻿using UnityEngine;
using System.Collections;

public class StageOriNativeTalk1 : StateMachineBehaviour
{
    private bool said_message = false;


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.99f && !said_message)
        {
            FlowManager.Instance.native_messenger.Message(" \"我要给我的笔友写一封信\" \nTranslation from Chinese: \n \"I'm going to write a letter to my penpal\"");
            said_message = true;
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FlowManager.Instance.native_messenger.Clear();

        // reset
        said_message = false;
    }

}
