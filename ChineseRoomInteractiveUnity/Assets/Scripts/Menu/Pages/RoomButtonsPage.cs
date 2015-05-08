using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoomButtonsPage : UIMenuPage 
{
    public UIMenuPage help_page;

    public void ButtonQuit()
    {
        // will eventually go back to main menu (through FlowManager)
        //Application.Quit();
    }
    public void OpenHelpPageDelayed(float delay)
    {
        if (help_page != null) help_page.TransitionIn(default_seconds, delay);
    }
    public void ButtonHelp()
    {
        if (help_page != null) help_page.TransitionIn();
    }
}
