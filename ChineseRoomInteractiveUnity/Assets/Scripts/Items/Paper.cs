using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;


public class Paper : GrabItem
{
    public Canvas text_canvas_obj;
    private Text text_obj;


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


        // get references
        text_obj = text_canvas_obj.GetComponentInChildren<Text>();
        if (text_obj == null)
            Debug.LogError("Reference to text not set");

        base.Start();
    }
    
    public void AddText(string text)
    {
        text_obj.text += text;

        // SHOULD CHECK FOR PAPER OVERFLOW
    }
    public void RemoveLastChar()
    {
        text_obj.text = text_obj.text.Substring(0, text_obj.text.Length - 1);
    }


    protected override bool SetGraphicsObject(Transform new_graphics_obj)
    {
        if (!base.SetGraphicsObject(new_graphics_obj)) return false;

        text_canvas_obj.transform.SetParent(new_graphics_obj, false);
        return true;
    }
}
