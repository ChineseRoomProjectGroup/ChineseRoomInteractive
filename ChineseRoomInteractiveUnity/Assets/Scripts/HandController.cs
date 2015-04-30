using UnityEngine;
using System.Collections;

public enum HandState { Free, HoldingItem };

public class HandController : MonoBehaviour 
{
    public Transform tip_position;
    private HandState state = HandState.Free;
    private GrabObject held_item;


    // PUBLIC MODIFIERS

    public void Start()
    {
        Cursor.visible = false; // hide default OS cursor
    }
	public void Update()
    {
        //-----hand following mouse-------------------------------------
        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mouse_pos - (Vector2)tip_position.localPosition;


        // object drag and drop control
        UpdateItemAction();
        


        //-----on right button click
        if (Input.GetMouseButtonDown(1))
        {
            // change the sprite to hand_eraser
        }
        else
        {
            // change the sprite to hand_up
        }
    }


    // PRIVATE MODIFIERS

    private void UpdateItemAction()
    {
        bool input_action = Input.GetMouseButtonDown(0);
        bool input_eraser = Input.GetMouseButtonDown(1);


        // Pick up item, drop item, do something with item
        if (input_action)
        {
            if (state == HandState.Free)
            {
                // Pick up an item (if over one)
                Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D overlap_col = Physics2D.OverlapPoint(mouse_pos);

                if (overlap_col != null)
                {                    
                    GrabObject item = overlap_col.GetComponent<GrabObject>();
                    if (item != null)
                    {
                        GrabItem(item);
                    }
                }
            }
            else if (state == HandState.HoldingItem)
            {
                // Drop the item
                DropHeldItem();
            }
        }

    }

    private void GrabItem(GrabObject item)
    {
        item.Grab();
        held_item = item;
        state = HandState.HoldingItem;

        // Graphics

        // change the sprite to hand_up
    }
    private void DropHeldItem()
    {
        held_item.Drop();
        held_item = null;
        state = HandState.Free;

        // Graphics

        // change the sprite to hand_down
    }
}
