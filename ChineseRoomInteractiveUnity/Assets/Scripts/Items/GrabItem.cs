using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabItem : MonoBehaviour 
{
    /// Graphics
    
    // Graphics objects for different states
    // normal - held or in world
    // hover - hovered by free hand, hovered by hand with item useable on this item
    // highlight - hand holds an item useable on this item
    public Transform graphics_normal, graphics_hover, graphics_highlight;
    protected Transform current_graphics_obj;

    private string GrabbedSortingLayer = "Hand UI";
    private string NormalSortingLayer = "Foreground";


    /// Tooltips
    
    protected string pick_up_tooltip = "Grab item";

    // item name (that wants to interact with this item) : tooltip
    protected Dictionary<string, string> interactable_item_tooltips = new Dictionary<string, string>() { };


    /// Interaction

    // true if the hand holds an item that can interact with this
    // item - should highlight
    private bool held_item_can_interact = false;

    // the tags of items that can interact with this item
    protected List<string> interactable_item_tags = new List<string>() { };


    /// General

    // Objects whose positions are placement locations for grabbed items
    // Can be used as placement indicators (snap objects will be active only when the GrabItem is grabbed)
    public List<Transform> snap_objects;

    protected HandController hand; // reference to the hand
    private Vector2 target_pos;
    private bool dropping = false;
    private bool grabbed = false;
    private bool dropable = false; // a grab object is dropable if there are snap transforms
    private float move_speed = 15f;



    // PUBLIC MODIFIERS

    public void Awake()
    {
        // get HandController reference
        hand = FindObjectOfType<HandController>();
        if (hand == null) Debug.LogError("HandController object not found");
    }
    public void Start()
    {
        // start with normal graphics
        current_graphics_obj = graphics_normal;

        // hide snap objects
        ShowSnapObjects(false);

        // find/set whether dropable
        if (snap_objects.Count > 0)
        {
            dropable = true;
        }

        // hook up to hand events
        hand.event_state_change += new System.EventHandler(OnHandStateChange);
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
    public void OnTriggerEnter2D(Collider2D collider)
    {
        // on hand hover (not while grabbed)
        if (collider.transform == hand.transform && !grabbed)
        {
            if (hand.GetHandState() == HandState.HoldingItem)
            {
                // if the held item can interact with this item
                if (held_item_can_interact && hand.ItemUseAllowed())  // NEED ORGANIZATION IMPROVEMENT - should be part of held_item_can_interact, hand state...
                {
                    // show the hover graphics
                    SetGraphicsObject(graphics_hover);

                    // display the interaction tooltip
                    hand.SetActionToolTip(interactable_item_tooltips[hand.GetHeldItem().name]);
                }
            }
            else if (hand.GetHandState() == HandState.Free)
            {
                // show the hover graphics
                SetGraphicsObject(graphics_hover);

                // show pickup tooltip
                hand.SetActionToolTip(pick_up_tooltip);  
            }    
        }
    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        // on hand unhover (not while grabbed)
        if (collider.transform == hand.transform && !grabbed)
        {
            SetGraphicsObject(held_item_can_interact ? graphics_highlight : graphics_normal);
            hand.SetActionToolTip("");
        }
    }

    /// <summary>
    /// Do something when this item is used on some other (target) item.
    /// Returns whether some interaction was done.
    /// </summary>
    /// <param name="target"></param>
    public virtual bool Use(GrabItem target)
    {
        if (target != null && target.interactable_item_tags.Contains(this.tag))
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

        // show snap objects
        ShowSnapObjects(true);

        // set graphics back to normal
        SetGraphicsObject(graphics_normal);

        // draw on top of other items
        current_graphics_obj.GetComponent<SpriteRenderer>().sortingLayerName = GrabbedSortingLayer;
    }
    /// <summary>
    /// Causes the object to position itself at the closest snap location
    /// Returns whether the item was actually dropped (could be not dropable)
    /// 
    /// requires: - if dropable, snap_objects is not empty
    ///           - snap_objects contains non null elements
    /// </summary>
    public virtual bool Drop()
    {
        if (!dropable) return false;

        grabbed = false;
        dropping = true;

        // stop drawing on top of other items
        current_graphics_obj.GetComponent<SpriteRenderer>().sortingLayerName = NormalSortingLayer;


        // find the closest snap position 
        float min_dist = float.MaxValue;
        Transform closest_obj = snap_objects[0];

        foreach (Transform snap_obj in snap_objects)
        {
            float dist = Vector2.Distance(transform.position, snap_obj.position);
            if (dist < min_dist)
            {
                min_dist = dist;
                closest_obj = snap_obj;
            }
        }
        target_pos = closest_obj.position;


        // hide snap objects
        ShowSnapObjects(false);

        return true;
    }


    // PRIVATE MODIFIERS

    /// <summary>
    /// Show or hide snap objects (snap objects can be used as item placement indicators).
    /// </summary>
    /// <param name="show"></param>
    private void ShowSnapObjects(bool show)
    {
        foreach (Transform snap_object in snap_objects)
        {
            snap_object.gameObject.SetActive(show);
        }
    }
    /// <summary>
    /// Returns whether graphics object was set (won't be if objects are null)
    /// </summary>
    /// <param name="new_graphics_obj"></param>
    /// <returns></returns>
    protected virtual bool SetGraphicsObject(Transform new_graphics_obj)
    {
        if (current_graphics_obj == null || new_graphics_obj == null) return false;

        current_graphics_obj.gameObject.SetActive(false);
        current_graphics_obj = new_graphics_obj;
        current_graphics_obj.gameObject.SetActive(true);

        return true;
    }


    // EVENTS

    private void OnHandStateChange(object sender, System.EventArgs e)
    {
        // if the hand is now holding an item that can interact with this one
        if (hand.GetHandState() == HandState.HoldingItem &&
            (interactable_item_tags.Contains(hand.GetHeldItem().tag)))
        {
            // save that the held item can interact with this
            held_item_can_interact = true;

            // switch to highlighted graphics
            SetGraphicsObject(graphics_highlight);
        }
        else
        {
            // save that the held item (if there is one) cannot interact with this
            held_item_can_interact = false;

            // switch to normal graphics
            SetGraphicsObject(graphics_normal);
        }
    }


    // PUBLIC ACCESSORS

    public bool IsDropable() { return dropable; }
    public bool IsGrabbed() { return grabbed; }
    public bool IsInPlace() { return !grabbed && !dropping; }
}
