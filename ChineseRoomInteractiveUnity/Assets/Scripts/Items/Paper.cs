using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Paper : GrabItem
{
    new public void Start()
    {
        interactable_item_names = new List<string>() { "Pencil", "Eraser" };

        // tool tips
        pick_up_tooltip = "Grab paper";
        interactable_item_tooltips = new Dictionary<string, string>()
        { 
            {"Pencil", "Write Chinese"},
            {"Eraser", "Erase"}
        };

        base.Start();
    }
    
}
