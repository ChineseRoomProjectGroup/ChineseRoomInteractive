using UnityEngine;
using System.Collections;

public abstract class UITransition : MonoBehaviour
{
    public virtual void UpdateTransition(float transition, bool going_in) { }
    public virtual void OnFinishTransitionOut()
    {

    }
    public virtual void OnFinishTransitionIn()
    {

    }
}


