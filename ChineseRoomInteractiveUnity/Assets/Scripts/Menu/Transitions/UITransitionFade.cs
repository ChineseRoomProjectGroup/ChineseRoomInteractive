using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UITransitionFade : UITransition
{
    private CanvasRenderer canvas_r;
    private CanvasGroup canvas_group;
    private float alpha_initial;
    public float offset = 0;
    public float power = 4;


    public void Awake()
    {
        canvas_r = GetComponent<CanvasRenderer>();
        canvas_group = GetComponent<CanvasGroup>();

        if (canvas_group != null) alpha_initial = canvas_group.alpha;
        else alpha_initial = canvas_r.GetAlpha();
    }
    public override void UpdateTransition(float transition, bool going_in)
    {
        float t = 1 - Mathf.Pow(1 - transition, power);

        // offset
        if (going_in)
        {
            t = Mathf.Max(0, (t - offset) / (1 - offset));
        }
        else
        {
            t = 1 - Mathf.Max(0, ((1 - t) - offset) / (1 - offset));
        }

        // set alpha
        if (canvas_group != null) canvas_group.alpha = alpha_initial * t;
        else canvas_r.SetAlpha(alpha_initial * t);
    }

}
