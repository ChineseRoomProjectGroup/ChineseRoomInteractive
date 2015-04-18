using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour 
{

    public Transform tip_position;

	public void Update()
    {
        //-----hand following mouse-------------------------------------
        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mouse_pos - (Vector2)tip_position.localPosition;

        //-----on left button click, hand change to down position-------
        if (Input.GetMouseButtonDown(0))
        {
            // change the sprite to hand_down
        }
        else
        {
            // change the sprite to hand_up
        }

        // 
    }
}
