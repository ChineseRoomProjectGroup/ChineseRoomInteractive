using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;


public class Book : GrabItem 
{
    private string tag_openbook_snap_loc = "Open Book Spot";

    // hand hover colliders
    public Collider2D collider_shelf, collider_open;

    // base graphics objects for:
    // graphics_normal : shelf
    // graphics_hover : shelf hovered

    public Transform graphics_open, graphics_open_hover, graphics_closed;
    public Canvas text_canvas_obj;

    // state
    private bool open = false;


    new public void Start()
    {
        interactable_item_tags = new List<string>() { };

        // tool tips
        pick_up_tooltip = "Grab book";
        interactable_item_tooltips = new Dictionary<string, string>() { };

        // check references


        base.Start();
    }
    
    public override void Grab()
    {
        SetOpen(false);

        base.Grab();
    }

    protected override void OnSnapLocationChosen(SnapLocation loc)
    {
        base.OnSnapLocationChosen(loc);

        SetOpen(loc.CompareTag(tag_openbook_snap_loc));
    }
    protected override void ChangeGraphicsByState()
    {
        base.ChangeGraphicsByState();

        if (open && IsHovered())
            SetGraphicsObject(graphics_open_hover);
        else if (open)
            SetGraphicsObject(graphics_open);
        else if (IsGrabbed())
            SetGraphicsObject(graphics_closed);

        // otherwise just use base assignments
    }
    protected override bool SetGraphicsObject(Transform new_graphics_obj)
    {
        if (!base.SetGraphicsObject(new_graphics_obj)) return false;

        // keep the text on the book if the book is in the open or hovered open state
        if (open) text_canvas_obj.transform.SetParent(new_graphics_obj, false);
        return true;
    }


    private void SetOpen(bool open)
    {
        this.open = open;
        collider_shelf.enabled = !open;
        collider_open.enabled = open;
    }

}
