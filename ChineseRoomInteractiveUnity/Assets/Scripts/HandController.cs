using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public enum HandState { Free, HoldingItem };

public class HandController : MonoBehaviour 
{
    public Transform tip_position;
    public Text tooltip;
    private HandState state = HandState.Free;
    private GrabItem held_item, held_item_last, hovered_item;

    // whether dropping / using the held item is allowed
    private bool allow_item_use = true;

    // Events
    public event EventHandler event_state_change;


    // PUBLIC MODIFIERS

    public void Start()
    {
        Cursor.visible = false; // hide default OS cursor
        SetActionToolTip("");
    }
	public void Update()
    {
        //-----hand following mouse-------------------------------------
        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mouse_pos - (Vector2)tip_position.localPosition;


        // item drag and drop / interaction control
        UpdateItemUsage();
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        // save hovered item
        GrabItem item = collider.GetComponent<GrabItem>();
        if (item != null && item != held_item)
            hovered_item = item;
    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        // remove hovered item reference
        GrabItem item = collider.GetComponent<GrabItem>();
        if (item == hovered_item && item != held_item)
            hovered_item = null;
    }

    public void SetActionToolTip(string text)
    {
        tooltip.text = text;
    }
    public void AllowItemUse(bool allow)
    {
        allow_item_use = allow;
    }


    // PRIVATE MODIFIERS

    private void UpdateItemUsage()
    {
        bool input_action = Input.GetMouseButtonDown(0);


        // Pick up item, drop item, do something with item
        if (input_action)
        {
            if (state == HandState.Free)
            {
                // Pick up an item (if over one)
                if (hovered_item != null)
                {
                    GrabItem(hovered_item);
                    hovered_item = null;
                }              
            }
            else if (state == HandState.HoldingItem && allow_item_use)
            {
                // Use held item on hovered item (if there is one)
                // or drop the item (if there isn't a hovered item)
                if (hovered_item != null)
                {
                    // if the item can be used, use it, otherwise, drop it
                    if (!held_item.Use(hovered_item)) DropHeldItem();
                }
                else
                    DropHeldItem();
            }
        }

    }

    private void GrabItem(GrabItem item)
    {
        item.Grab();
        held_item_last = held_item;
        held_item = item;
        state = HandState.HoldingItem;
        if (event_state_change != null) event_state_change(this, EventArgs.Empty);

        SetActionToolTip(""); // NEED ORGANIZATION IMPROVEMENT


        // Graphics

        // change the sprite to hand_up
    }
    private void DropHeldItem()
    {
        held_item.Drop();
        held_item = null;
        state = HandState.Free;
        if (event_state_change != null) event_state_change(this, EventArgs.Empty);

        // Graphics

        // change the sprite to hand_down
    }


    // PUBLIC ACCESSORS

    public GrabItem GetHeldItem()
    {
        return held_item;
    }
    public GrabItem GetLastHeldItem()
    {
        return held_item_last;
    }
    public GrabItem GetHoveredItem()
    {
        return hovered_item;
    }
    public HandState GetHandState()
    {
        return state;
    }
    public bool JustGrabbed(GrabItem item)
    {
        return GetHeldItem() == item && GetLastHeldItem() != item;
    }
    public bool JustDropped(GrabItem item)
    {
        return GetLastHeldItem() == item && GetHeldItem() != item;
    }
    public bool ItemUseAllowed()
    {
        return allow_item_use;
    }
}
