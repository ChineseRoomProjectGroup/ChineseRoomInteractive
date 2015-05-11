using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabItem : Item 
{
    /// Graphics

    private string GrabbedSortingLayer = "Hand UI";
    private string NormalSortingLayer = "Foreground";


    /// Interaction and Tooltips

    protected string pick_up_tooltip = "Grab item";


    /// General

    // Objects whose positions are placement locations for grabbed items
    // Can be used as placement indicators (snap objects will be active only
    // when the GrabItem is grabbed)
    public List<SnapLocation> snap_locations;

    // can be left null if no snap location will be used initially
    public SnapLocation current_snap_loc; 
    private Vector2 target_pos;
    private bool dropable = false; // a grab object is dropable if there are snap transforms
    private float move_speed = 15f;

    // state
    private bool grabbed = false;
    private bool dropping = false;    



    // PUBLIC MODIFIERS

    new public void Start()
    {
        base.Start();

        // hide snap objects
        ShowSnapLocations(false);

        // snap to the initial location if one is set
        if (current_snap_loc != null)
        {
            target_pos = current_snap_loc.transform.position;
            current_snap_loc.SetOccupied(true);
        }

        // find/set whether dropable
        if (snap_locations.Count > 0)
        {
            dropable = true;
        }
    }
    public void Update()
    {
        if (IsInPlace()) return;

        // set target position to mouse when grabbed
        if (grabbed)
        {
            Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target_pos = mouse_pos;
        }


        // update position
        Vector3 target_pos_v3 = new Vector3(target_pos.x, target_pos.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, target_pos_v3, Time.deltaTime * move_speed);

        if (dropping && Vector3.Distance(transform.position, target_pos_v3) <= 0.1f)
        {
            // set in place after reaching snap location (when dropping)
            transform.position = target_pos_v3;
            dropping = false;
        }

    }
    new public void OnTriggerEnter2D(Collider2D collider)
    {
        // on hand hover (cannot be hovered while grabbed)
        if (collider.transform == hand.transform && !grabbed)
        {
            hovered = true;

            if (hand.HoldingItem())
            {
                // if the held item can interact with this item
                if (CanBeUsedOnBy(hand.GetHeldItem().tag) && hand.ItemUseAllowed())
                {
                    // display the interaction tooltip
                    hand.SetActionToolTip(interactable_item_tooltips[hand.GetHeldItem().name]);
                }
            }
            else
            {
                // show pickup tooltip
                hand.SetActionToolTip(pick_up_tooltip);  
            }

            // update graphics
            ChangeGraphicsByState();
        }
    }
    new public void OnTriggerExit2D(Collider2D collider)
    {
        // on hand unhover (cannot be hovered while grabbed)
        if (collider.transform == hand.transform && !grabbed)
        {
            hovered = false;

            // update graphics and tooltip
            ChangeGraphicsByState();
            hand.SetActionToolTip("");
        }
    }

    /// <summary>
    /// Do something when this item is used on some other (target) item.
    /// Returns whether some interaction was done.
    /// </summary>
    /// <param name="target"></param>
    public virtual bool Use(Item target)
    {
        if (target != null && target.CanBeUsedOnBy(this.tag))
        {
            return true;
        }
        else return false;
    }
    /// <summary>
    /// Causes the object to position itself at the mouse position until dropped (Drop())
    /// </summary>
    public virtual void Grab()
    {
        grabbed = true;
        hovered = false;

        // unoccupy current snap location (if at one)
        if (current_snap_loc != null)
        {
            current_snap_loc.SetOccupied(false);
        }

        // show snap objects
        ShowSnapLocations(true);

        // update graphics
        ChangeGraphicsByState();

        // draw on top of other items
        current_graphics_obj.GetComponent<SpriteRenderer>().sortingLayerName = GrabbedSortingLayer;
    }
    /// <summary>
    /// Causes the object to position itself at the closest snap location
    /// Returns whether the item was actually dropped (could be not dropable)
    /// 
    /// requires: - if dropable, snap_locations is not empty
    ///           - snap_locations contains non null elements
    /// </summary>
    public virtual bool Drop()
    {
        if (!dropable) return false;

        // find the closest snap location 
        float min_dist = float.MaxValue;
        SnapLocation closest_snap_loc = current_snap_loc;

        foreach (SnapLocation loc in snap_locations)
        {
            if (loc.IsOccupied()) continue; // not an option

            float dist = Vector2.Distance(transform.position, loc.transform.position);
            if (dist < min_dist)
            {
                min_dist = dist;
                closest_snap_loc = loc;
            }
        }

        // return if no available snap locations
        if (closest_snap_loc == null || closest_snap_loc.IsOccupied()) return false;

        // snap to the chosen snap location
        target_pos = closest_snap_loc.transform.position;
        current_snap_loc = closest_snap_loc;
        current_snap_loc.SetOccupied(true);
        OnSnapLocationChosen(closest_snap_loc);

        // hide snap objects
        ShowSnapLocations(false);

        // update state
        grabbed = false;
        dropping = true;

        // update graphics
        ChangeGraphicsByState();

        // stop drawing on top of other items
        current_graphics_obj.GetComponent<SpriteRenderer>().sortingLayerName = NormalSortingLayer;

        return true;
    }


    // PRIVATE MODIFIERS

    /// <summary>
    /// Show or hide snap objects (snap objects can be used as item placement indicators).
    /// </summary>
    /// <param name="show"></param>
    private void ShowSnapLocations(bool show)
    {
        foreach (SnapLocation snap_loc in snap_locations)
        {
            if (show && !snap_loc.IsOccupied())
                snap_loc.SetActive(true);
            else if (!show)
                snap_loc.SetActive(false);
        }
    }
    protected virtual void OnSnapLocationChosen(SnapLocation loc) { }



    // PUBLIC ACCESSORS

    public bool IsDropable() { return dropable; }
    public bool IsGrabbed() { return grabbed; }
    public bool IsInPlace() { return !grabbed && !dropping; }
    public bool IsDropping() { return dropping; }

}
