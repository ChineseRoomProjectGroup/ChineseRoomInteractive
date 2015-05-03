using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Outbox : Item
{
    new public void Start()
    {
        base.Start();

        interactable_item_tags = new List<string>() { "Letter" };

        // tool tips
        interactable_item_tooltips = new Dictionary<string, string>()
        { 
            {"Letter", "Send letter"}
        };
    }


}
