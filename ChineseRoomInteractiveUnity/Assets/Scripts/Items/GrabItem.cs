using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabItem : MonoBehaviour 
{
    /// Graphics
    
    // Graphics objects for different states
    // normal - held or in world
    // hover - hovered by free hand, hovered by hand with item useable on this item
    public Transform graphics_normal, graphics_hover;
    protected Transform current_graphics_obj;

    private string GrabbedSortingLayer = "Hand UI";
    private string NormalSortingLayer = "Foreground";


    /// Interaction and Tooltips

    // the tags of items that can interact with this item
    protected List<string> interactable_item_tags = new List<string>() { };

    // item name (that wants to interact with this item) : tooltip
    protected Dictionary<string, string> interactable_item_tooltips = new Dictionary<string, string>() { };
    protected string pick_up_tooltip = "Grab item";


    /// General

    // Objects whose positions are placement locations for grabbed items
    // Can be used as placement indicators (snap objects will be active only when the GrabItem is grabbed)
    public List<Transform> snap_objects;

    protected HandController hand; // reference to the hand
    private Vector2 target_pos;

    private bool dropable = false; // a grab object is dropable if there are snap transforms
    private float move_speed = 15f;

    // state
    private bool grabbed = false;
    private bool dropping = false;
    private bool hovered = false;
    
    



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


        // hide snap objects
        ShowSnapObjects(false);

        // find/set whether dropable
        if (snap_objects.Count > 0)
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
    public void OnTriggerEnter2D(Collider2D collider)
    {
        // on hand hover (not while grabbed)
        if (collider.transform == hand.transform && !grabbed)
        {
            hovered = true;

            if (hand.HoldingItem())
            {
                // if the held item can interact with this item
                if (interactable_item_tags.Contains(hand.GetHeldItem().tag) && hand.ItemUseAllowed())
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
    public void OnTriggerExit2D(Collider2D collider)
    {
        // on hand unhover (not while grabbed)
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
        hovered = false;

        // show snap objects
        ShowSnapObjects(true);

        // update graphics
        ChangeGraphicsByState();

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
        int closest_snap_object_i = 0;

        for (int i = 0; i < snap_objects.Count; ++i)
        {
            float dist = Vector2.Distance(transform.position, snap_objects[i].position);
            if (dist < min_dist)
            {
                min_dist = dist;
                closest_snap_object_i = i;
            }
        }
        target_pos = snap_objects[closest_snap_object_i].position;
        OnSnapLocationChosen(closest_snap_object_i);


        // hide snap objects
        ShowSnapObjects(false);

        // update graphics
        ChangeGraphicsByState();

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

    protected virtual void OnSnapLocationChosen(int snap_object_index) { }



    // PUBLIC ACCESSORS

    public bool IsDropable() { return dropable; }
    public bool IsGrabbed() { return grabbed; }
    public bool IsInPlace() { return !grabbed && !dropping; }
    public bool IsDropping() { return dropping; }
    public bool IsHovered() { return hovered; }

}
