using UnityEngine;
using System.Collections;

public class UITransitionAudioFade : UITransition 
{
    public bool full_volume_when_out = false;

    public override void UpdateTransition(float transition, bool going_in)
    {
        AudioListener.volume = full_volume_when_out ? 1 - transition : transition;
        base.UpdateTransition(transition, going_in);
    }
    public override void OnFinishTransitionIn()
    {
        AudioListener.volume = full_volume_when_out ? 0 : 1;
        base.OnFinishTransitionIn();
    }
    public override void OnFinishTransitionOut()
    {
        AudioListener.volume = full_volume_when_out ? 1 : 0;
        base.OnFinishTransitionOut();
    }
}
