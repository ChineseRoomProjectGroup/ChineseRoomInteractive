using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Book : GrabItem 
{
    new public void Start()
    {
        interactable_item_tags = new List<string>() { };

        // tool tips
        pick_up_tooltip = "Grab book";
        interactable_item_tooltips = new Dictionary<string, string>() { };

        // check references


        base.Start();
    }

}
