using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabObject : MonoBehaviour 
{
    public List<Transform> snap_transforms;

    private Vector2 target_pos;
    private bool dropping = false;
    private bool grabbed = false;
    private bool dropable = false; // a grab object is dropable if there are snap transforms

    private float move_speed = 15f;


    // PUBLIC MODIFIERS

    public void Start()
    {
        // find/set whether dropable
        if (snap_transforms.Count > 0)
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

    /// <summary>
    /// Causes the object to position itself at the mouse position until dropped (Drop())
    /// </summary>
    public void Grab()
    {
        grabbed = true;
    }
    /// <summary>
    /// Causes the object to position itself at the closest snap location
    /// 
    /// requires: - if dropable, snap_transforms is not empty
    ///           - snap_transforms contains non null elements
    /// </summary>
    public void Drop()
    {
        if (!dropable) return;

        grabbed = false;
        dropping = true;


        // find the closest snap position 
        float min_dist = float.MaxValue;
        Transform closest_t = snap_transforms[0];

        foreach (Transform snap_t in snap_transforms)
        {
            float dist = Vector2.Distance(transform.position, snap_t.position);
            if (dist < min_dist)
            {
                min_dist = dist;
                closest_t = snap_t;
            }
        }
        target_pos = closest_t.position;
    }


    // PUBLIC ACCESSORS

    public bool IsDropable() { return dropable; }
    public bool IsGrabbed() { return grabbed; }
    public bool IsInPlace() { return !grabbed && !dropping; }
}
