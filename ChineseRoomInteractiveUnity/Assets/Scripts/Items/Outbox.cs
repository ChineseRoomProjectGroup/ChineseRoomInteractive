using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Outbox : Item
{
    public string correct_output = "ABC";
    public Messenger messangar;


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
            // message
            messangar.Message("OK! Let's send along your thoughtful reply.", 2f);

            // next phase
            FlowManager.OnCorrectOutputGiven();
            return true;
        }


        // provide helpful message
        messangar.Message("That doesn't sound like a good reply in Chinese..." +
            " we wouldn't want to send that in a letter! Have another look in the rule books.", 6f);

        return false;
    }  

}
