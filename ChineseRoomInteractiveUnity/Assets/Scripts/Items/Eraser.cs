using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Eraser : GrabItem
{
    new public void Start()
    {
        interactable_item_tags = new List<string>() { };

        // tool tips
        pick_up_tooltip = "Grab eraser";
        interactable_item_tooltips = new Dictionary<string, string>() { };

        base.Start();
    }

    public override bool Use(GrabItem target)
    {
        if (!base.Use(target)) return false;


        Paper paper = target.GetComponent<Paper>();
        if (paper != null)
        {
            paper.RemoveLastChar();
            return true;
        }


        // no interaction happened
        return false;
    }
}
