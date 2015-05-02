using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pencil : GrabItem 
{
    public SymbolPalette symbol_palette;
    private bool in_use = false;

    new public void Start()
    {
        interactable_item_tags = new List<string>() { };

        // tool tips
        pick_up_tooltip = "Grab pencil";
        interactable_item_tooltips = new Dictionary<string, string>() { };

        // check references
        if (symbol_palette == null)
            Debug.LogError("Reference to SymbolPalette not set");

        base.Start();
    }

    public override bool Use(GrabItem target)
    {
        if (!base.Use(target)) return false;


        // Write on paper
        Paper paper = target.GetComponent<Paper>();
        if (paper != null)
        {
            symbol_palette.CreatePalette(paper);
            in_use = true;
            hand.AllowItemUse(false);

            return true;
        }


        // no interaction happened
        return false;
    }

    public void Update()
    {
        // freeze the pencil until text input is finished (palette is inactive)
        if (in_use)
        {
            if (!symbol_palette.gameObject.activeSelf)
            {
                in_use = false;
                hand.AllowItemUse(true);
            }
            else return;
        }


        base.Update();
    }
}
