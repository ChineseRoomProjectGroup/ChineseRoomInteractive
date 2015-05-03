using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Item : MonoBehaviour 
{
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
                    hand.SetActionToolTip(interactable_item_tooltips[hand.GetHeldItem().name]);
                }
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        // on hand unhover
        if (collider.transform == hand.transform)
        {
            hovered = false;
            hand.SetActionToolTip("");
        }
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
