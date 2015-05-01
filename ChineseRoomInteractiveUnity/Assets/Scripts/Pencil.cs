﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pencil : GrabItem 
{
    new public void Start()
    {
        interactable_item_names = new List<string>() { };

        // tool tips
        pick_up_tooltip = "Grab pencil";
        interactable_item_tooltips = new Dictionary<string, string>() { };

        base.Start();
    }
}
