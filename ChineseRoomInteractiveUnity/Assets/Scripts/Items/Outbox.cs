using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Outbox : Item
{
    public string correct_output = "ABC";
    public Animator scene_animator;

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

    /// <summary>
    /// Returns whether the letter was accepted (correct output).
    /// Progresses demo to next phase if output is good.
    /// </summary>
    /// <param name="letter"></param>
    /// <returns></returns>
    public bool TrySendOutput(Letter letter)
    {
        if (letter.GetText() == correct_output)
        {
            // next phase
            scene_animator.SetTrigger("Next");
            return true;
        }


        // provide helpful message


        return false;
    }


}
