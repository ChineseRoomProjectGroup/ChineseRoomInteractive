using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeScreenPage : UIMenuPage 
{
    public Image overlay;
    private Color default_color;
    private bool initialized = false;


    public void Awake()
    {
        if (!initialized)
            Initiailze();
    }
    new public void Start()
    {
        base.Start();
        if (!transition_out_on_start)
        {
            gameObject.SetActive(false);
        }
    }
    private void Initiailze()
    {
        default_color = overlay.color;

        // enable the colored overlay here so that it need not be on in the editor
        overlay.gameObject.SetActive(true);
        
        initialized = true;
    }

    public void TransitionIn(float seconds, float delay_seconds, Color color)
    {
        overlay.color = color;

        base.TransitionIn(seconds, delay_seconds);
    }
    public override void TransitionIn(float seconds, float delay_seconds)
    {
        TransitionIn(seconds, delay_seconds, default_color);
    }
    public void TransitionOut(float seconds, float delay_seconds, Color color)
    {
        overlay.color = color;

        base.TransitionOut(seconds, delay_seconds);
    }
    public override void TransitionOut(float seconds, float delay_seconds)
    {
        TransitionOut(seconds, delay_seconds, default_color);
    }
}
