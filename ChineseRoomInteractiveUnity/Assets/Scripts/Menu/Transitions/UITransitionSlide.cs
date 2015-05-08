using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class UITransitionSlide : UITransition
{
    private RectTransform rect;
    public Vector2 in_offset, out_offset;
    private Vector2 initial_pos;

    public void Awake()
    {
        rect = GetComponent<RectTransform>();
        initial_pos = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y);
    }
    public override void UpdateTransition(float transition, bool going_in)
    {
        float t = 1 - Mathf.Pow(1 - transition, 4);

        Vector2 pos = initial_pos;
        pos += (going_in ? in_offset : out_offset) * (1-t);
        rect.anchoredPosition = pos;
    }

}
