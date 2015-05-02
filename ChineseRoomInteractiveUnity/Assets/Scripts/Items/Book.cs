using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;


public class Book : GrabItem 
{
    public int[] shelf_location_indices;
    public int[] open_location_indices;

    // Base graphics objects used for:
    //  graphics_normal : shelf
    //  graphics_hover  : shelf_hover 

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
        open = false;

        base.Grab();
    }

    protected override void OnSnapLocationChosen(int snap_object_index)
    {
        base.OnSnapLocationChosen(snap_object_index);

        open = open_location_indices.Contains(snap_object_index);
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
}
