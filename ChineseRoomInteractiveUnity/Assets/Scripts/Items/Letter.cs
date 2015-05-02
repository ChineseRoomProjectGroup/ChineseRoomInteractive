using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class Letter : Paper 
{
    private string tag_envelope_snap_loc = "Envelope Spot";

    // hand hover colliders
    public Collider2D collider_envelope, collider_opened;

    // base graphics objects for:
    // graphics_normal : envelope
    // graphics_hover : envelope hovered

    public Transform graphics_opened, graphics_opened_hover;

    // state
    private bool opened = false; // opened = not in envelope



    new public void Start()
    {
        base.Start();

        base.writable = opened;

        interactable_item_tags = new List<string>() { "Pencil", "Eraser" };

        // tool tips
        pick_up_tooltip = "Grab letter";
        interactable_item_tooltips = new Dictionary<string, string>()
        { 
            {"Pencil", "Write Chinese"},
            {"Eraser", "Erase"}
        };
    }

    protected override void OnSnapLocationChosen(SnapLocation loc)
    {
        base.OnSnapLocationChosen(loc);

        SetOpened(!loc.CompareTag(tag_envelope_snap_loc));
    }
    protected override void ChangeGraphicsByState()
    {
        base.ChangeGraphicsByState();

        if (opened && IsHovered())
            SetGraphicsObject(graphics_opened_hover);
        else if (opened)
            SetGraphicsObject(graphics_opened);

        // otherwise just use base assignments
    }
    protected override bool SetGraphicsObject(Transform new_graphics_obj)
    {
        if (!base.SetGraphicsObject(new_graphics_obj)) return false;

        // keep the text on the opened graphics object (Paper wants to always move it to the current graphics object)
        // COULD BE BETTER
        if (!opened) text_canvas_obj.transform.SetParent(graphics_opened, false);
        return true;
    }


    private void SetOpened(bool opened)
    {
        this.opened = opened;
        collider_opened.enabled = opened;
        collider_envelope.enabled = !opened;

        base.writable = opened;
    }

}
