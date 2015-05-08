using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;


public class UIMenuPage : MonoBehaviour 
{
    public float default_seconds = 0.5f;
    public float default_delay_seconds = 0;
    public bool transition_out_on_start = false;
    public float on_start_transition_delay = 0;

    private float duration_seconds;
    private float delay_seconds;

    public System.Action on_transitioned_in;
    public System.Action on_transitioned_out;

    private bool going_in = false, going_out = false;
    protected float transition = 0;
    protected List<UITransition> transition_objects;


    // PUBLIC MODIFIERS

    public void Start()
    {
        if (transition_out_on_start)
        {
            transition = 1;
            TransitionOut(default_seconds, on_start_transition_delay);
        }
        else
        {
            transition = 0;
            TransitionIn(default_seconds, on_start_transition_delay);
        }
    }

    public virtual void TransitionIn(float duration_seconds, float delay_seconds)
    {
        if (!going_in)
        {
            this.duration_seconds = duration_seconds;
            this.delay_seconds = delay_seconds;

            gameObject.SetActive(true);
            transform.SetAsLastSibling(); // make the topmost page (to disable interaction with lower pages)
            UpdateTransitioningObjectList();

            OnStartTransitionIn();

            going_out = false;
            StopCoroutine("UpdateTransitionOut");

            going_in = true;
            StartCoroutine("UpdateTransitionIn");
        }
    }
    public void TransitionIn()
    {
        TransitionIn(default_seconds, default_delay_seconds);
    }
    public virtual void TransitionOut(float duration_seconds, float delay_seconds)
    {
        if (!going_out)
        {
            this.duration_seconds = duration_seconds;
            this.delay_seconds = delay_seconds;

            transform.SetAsFirstSibling(); // make the bottommost page (to disable interaction with lower pages)
            UpdateTransitioningObjectList();

            OnStartTransitionOut();

            going_in = false;
            StopCoroutine("UpdateTransitionIn");

            going_out = true;
            StartCoroutine("UpdateTransitionOut");
        }
    }
    public void TransitionOut()
    {
        TransitionOut(default_seconds, default_delay_seconds);
    }


    // PRIVATE / PROTECTED MODIFIERS

    private void UpdateTransitioningObjectList()
    {
        transition_objects = GetComponentsInChildren<UITransition>().ToList<UITransition>();
        UITransition[] trans = GetComponents<UITransition>();
        if (trans.Length != 0)
        {
            transition_objects.AddRange(trans);
        }
    }

    private IEnumerator UpdateTransitionIn()
    {
        // update transitioning objects before the delay
        foreach (UITransition obj in transition_objects)
        {
            obj.UpdateTransition(transition, going_in);
        }

        // delay before transition begins
        yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(delay_seconds));

        // transition
        while (true)
        {
            transition += Time.unscaledDeltaTime / duration_seconds;
            transition = Mathf.Min(transition, 1);

            foreach (UITransition obj in transition_objects)
            {
                obj.UpdateTransition(transition, going_in);
            }

            // transition finished
            if (transition >= 1)
            {
                going_in = false;

                OnFinishTransitionIn();
                foreach (UITransition obj in transition_objects)
                    obj.OnFinishTransitionIn();

                if (on_transitioned_in != null) on_transitioned_in();
                break;
            }

            yield return null;
        }
    }
    private IEnumerator UpdateTransitionOut()
    {
        // update transitioning objects before the delay
        foreach (UITransition obj in transition_objects)
        {
            obj.UpdateTransition(transition, going_in);
        }

        // delay before transition begins
        yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(delay_seconds));

        // transition
        while (true)
        {
            transition -= Time.unscaledDeltaTime / duration_seconds;
            transition = Mathf.Max(transition, 0);

            foreach (UITransition obj in transition_objects)
                obj.UpdateTransition(transition, going_in);

            // transition finished
            if (transition <= 0)
            {
                going_out = false;

                OnFinishTransitionOut();
                foreach (UITransition obj in transition_objects)
                    obj.OnFinishTransitionOut();

                gameObject.SetActive(false);
                if (on_transitioned_out != null)
                {
                    on_transitioned_out();
                }
                break;
            }

            yield return null;
        }
    }

    protected virtual void OnStartTransitionOut() { }
    protected virtual void OnFinishTransitionOut() { }
    protected virtual void OnStartTransitionIn() { }
    protected virtual void OnFinishTransitionIn() { }


    // PUBLIC ACCESSORS

    public bool IsGoingOut()
    {
        return going_out;
    }
    public bool IsGoingIn()
    {
        return going_in;
    }
    public bool IsTransitioning()
    {
        return going_in || going_out;
    }
	public float Transition()
    {
        return transition;
    }
}
