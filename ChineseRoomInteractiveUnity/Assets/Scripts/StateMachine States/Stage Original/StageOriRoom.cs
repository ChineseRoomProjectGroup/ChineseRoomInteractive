﻿using UnityEngine;
using System.Collections;

public class StageOriRoom : StateMachineBehaviour 
{
    private bool said_message = false;
    private float next_timer = -1, next_time_max = 1;


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.9f && !said_message)
        {
            FlowManager.Instance.room_messenger.Message("You've got mail! Let's send a reply...", 3f);

            // bring up the options buttons and display the help text
            FlowManager.Instance.room_buttons_page.TransitionIn();
            FlowManager.Instance.room_buttons_page.OpenHelpPageDelayed(1.5f);

            said_message = true;
        }
        if (FlowManager.OutputCorrect() && next_timer < 0)
        {
            // delay before going to next state
            next_timer = next_time_max;
        }

        if (next_timer >= 0)
        {
            next_timer -= Time.deltaTime;
            if (next_timer <= 0)
            {
                // delay over, go to next state
                FlowManager.AnimatorNextScene();
            }
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // reset
        said_message = false;
        next_timer = -1;

        // remove the room buttons
        FlowManager.Instance.room_buttons_page.TransitionOut();
    }

}
