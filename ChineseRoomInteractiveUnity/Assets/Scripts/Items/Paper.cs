using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class Paper : GrabItem
{
    public bool never_write = true; // whether can ever be writable
    public Canvas text_canvas_obj;
    public Text text_obj;


    new public void Start()
    {
        interactable_item_tags = new List<string>() { "Pencil", "Eraser" };

        // tool tips
        pick_up_tooltip = "Grab paper";
        interactable_item_tooltips = new Dictionary<string, string>()
        { 
            {"Pencil", "Write Chinese"},
            {"Eraser", "Erase"}
        };

        base.Start();
    }
    
    public void AddText(string text)
    {
        if (!Writable()) return;

        text_obj.text += text;

        // SHOULD CHECK FOR PAPER OVERFLOW
    }
    public void RemoveLastChar()
    {
        if (!Writable()) return;

        text_obj.text = text_obj.text.Substring(0, text_obj.text.Length - 1);
    }

    protected override bool SetGraphicsObject(Transform new_graphics_obj)
    {
        if (!base.SetGraphicsObject(new_graphics_obj)) return false;

        // keep the text on the current graphics object
        text_canvas_obj.transform.SetParent(new_graphics_obj, false);
        return true;
    }
    /// <summary>
    /// Returns whether the paper's text can currently be modified
    /// </summary>
    /// <returns></returns>
    protected virtual bool Writable()
    {
        return !never_write;
    }


    public string GetText()
    {
        return text_obj.text;
    }
    public override bool CanBeUsedOnBy(string use_item_tag)
    {
        // cannot use anything on this paper if it is not writable
        return base.CanBeUsedOnBy(use_item_tag) && Writable();
    }

    
}
