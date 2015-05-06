using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Item : MonoBehaviour 
{
    /// Graphics

    // Graphics objects for different states
    // normal - held or in world
    // hover - hovered by free hand, hovered by hand with item useable on this item
    public Transform graphics_normal, graphics_hover;
    protected Transform current_graphics_obj;


    /// Interaction and Tooltips

    // the tags of items that can interact with this item
    protected List<string> interactable_item_tags = new List<string>() { };

    // item name (that wants to interact with this item) : tooltip
    protected Dictionary<string, string> interactable_item_tooltips
        = new Dictionary<string, string>() { };


    /// General

    protected HandController hand; // reference to the hand

    // state
    protected bool hovered = false;



    // PUBLIC MODIFIERS

    public void Awake()
    {
        // get HandController reference
        hand = FindObjectOfType<HandController>();
        if (hand == null) Debug.LogError("HandController object not found");
    }
    public void Start()
    {
        // update graphics
        ChangeGraphicsByState();
        if (current_graphics_obj == null)
            Debug.Log("First used graphics object is not assigned");
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        // on hand hover
        if (collider.transform == hand.transform)
        {
            hovered = true;

            if (hand.HoldingItem())
            {
                // if the held item can interact with this item
                if (CanBeUsedOnBy(hand.GetHeldItem().tag) && hand.ItemUseAllowed())
                {
                    // display the interaction tooltip
                    hand.SetActionToolTip(interactable_item_tooltips[hand.GetHeldItem().tag]);
                }
            }
        }

        // update graphics
        ChangeGraphicsByState();
    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        // on hand unhover
        if (collider.transform == hand.transform)
        {
            hovered = false;

            // update graphics and tooltip
            ChangeGraphicsByState();
            hand.SetActionToolTip("");
        }
    }


    // PRIVATE MODIFIERS

    protected virtual void ChangeGraphicsByState()
    {
        if (hovered) SetGraphicsObject(graphics_hover);
        else SetGraphicsObject(graphics_normal);
    }
    /// <summary>
    /// Returns whether graphics object was set (won't be if objects are null)
    /// </summary>
    /// <param name="new_graphics_obj"></param>
    /// <returns></returns>
    protected virtual bool SetGraphicsObject(Transform new_graphics_obj)
    {
        if (new_graphics_obj == null) return false;

        if (current_graphics_obj != null)
            current_graphics_obj.gameObject.SetActive(false);

        current_graphics_obj = new_graphics_obj;
        current_graphics_obj.gameObject.SetActive(true);

        return true;
    }


    // PUBLIC ACCESSORS

    public bool IsHovered() { return hovered; }
    /// <summary>
    /// Can an item of specified tag be used on this item.
    /// </summary>
    /// <param name="use_item_tag"></param>
    /// <returns></returns>
    public virtual bool CanBeUsedOnBy(string use_item_tag)
    {
        return interactable_item_tags.Contains(use_item_tag);
    }

}
