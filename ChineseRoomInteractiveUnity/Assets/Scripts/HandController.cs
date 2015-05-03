using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;


public class HandController : MonoBehaviour 
{
    public Transform tip_position;
    public Text tooltip;
    private bool holding_item = false;
    private GrabItem held_item, held_item_last;
    private Item hovered_item; // need not be a grabbable item

    // whether dropping / using the held item is allowed
    private bool allow_item_use = true;


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
        Item item = collider.GetComponent<Item>();
        if (item != null && item != held_item)
            hovered_item = item;
    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        // remove hovered item reference
        Item item = collider.GetComponent<Item>();
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
    public void DropHeldItem()
    {
        if (!held_item.Drop()) return;
        held_item = null;
        holding_item = false;
    }


    // PRIVATE MODIFIERS

    private void UpdateItemUsage()
    {
        bool input_action = Input.GetMouseButtonDown(0);


        // Pick up item, drop item, do something with item
        if (input_action)
        {
            if (!holding_item)
            {
                // Pick up an item (if over one)
                if (hovered_item != null)
                {
                    GrabItem gi = hovered_item.GetComponent<GrabItem>();
                    if (gi != null)
                    {
                        GrabItem(gi);
                        hovered_item = null;
                    }
                }              
            }
            else if (holding_item && allow_item_use)
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
        holding_item = true;

        SetActionToolTip(""); // NEED ORGANIZATION IMPROVEMENT


        // Graphics

        // change the sprite to hand_up
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
    public Item GetHoveredItem()
    {
        return hovered_item;
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
    public bool HoldingItem()
    {
        return holding_item;
    }
}
