using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabItem : MonoBehaviour 
{
    // Graphics objects for different states
    // normal - held or in world
    // hover - hovered by free hand, hovered by hand with item useable on this item
    // highlight - hand holds an item useable on this item
    public Transform graphics_normal, graphics_hover, graphics_highlight;
    private Transform current_graphics_obj;

    // true if the hand holds an item that can interact with this
    // item - should highlight
    private bool held_item_can_interact = false;

    // Objects whose positions are placement locations for grabbed items
    // Can be used as placement indicators (snap objects will be active only when the GrabItem is grabbed)
    public List<Transform> snap_objects;

    private HandController hand; // reference to the hand

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
            SetGraphicsObject(graphics_hover);
        }
    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        // on hand unhover (not while grabbed)
        if (collider.transform == hand.transform && !grabbed)
        {
            SetGraphicsObject(held_item_can_interact ? graphics_highlight : graphics_normal);
        }
    }

    /// <summary>
    /// Do something when this item is used on some other (target) item.
    /// 
    /// requires: target must be non-null
    /// </summary>
    /// <param name="target"></param>
    public virtual void Use(GrabItem target)
    {

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
    private void SetGraphicsObject(Transform new_graphics_obj)
    {
        if (current_graphics_obj == null || new_graphics_obj == null) return;

        current_graphics_obj.gameObject.SetActive(false);
        current_graphics_obj = new_graphics_obj;
        current_graphics_obj.gameObject.SetActive(true);
    }

    // EVENTS

    protected virtual void OnHandStateChange(object sender, System.EventArgs e)
    {
        if (hand == null) return;

        if (hand.GetHandState() == HandState.HoldingItem &&
            (hand.GetHeldItem().name == "Pencil" || hand.GetHeldItem().name == "Eraser"))
        {
            held_item_can_interact = true;
            SetGraphicsObject(graphics_highlight);
        }
        else
        {
            held_item_can_interact = false;
            SetGraphicsObject(graphics_normal);
        }
    }


    // PUBLIC ACCESSORS

    public bool IsDropable() { return dropable; }
    public bool IsGrabbed() { return grabbed; }
    public bool IsInPlace() { return !grabbed && !dropping; }
}
