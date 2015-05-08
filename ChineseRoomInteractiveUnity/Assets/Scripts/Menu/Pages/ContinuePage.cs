using UnityEngine;
using System.Collections;

public class ContinuePage : UIMenuPage
{
    public UIMenuPage next_page;
    public float next_page_in_delay = 2;

    public void Update()
    {
        if (FlowManager.InputContinue() && !base.IsGoingOut())
        {
            ButtonContinue();
        }
    }

    public void ButtonContinue()
    {
        TransitionOut();
        on_transitioned_out = () => FlowManager.AnimatorNextScene();
        if (next_page != null) next_page.TransitionIn(default_seconds, next_page_in_delay);
    }
}
