using UnityEngine;
using System.Collections;

public class MainMenuPage : UIMenuPage 
{
    public FadeScreenPage fade_page;

    public void ButtonQuitYes()
    {
        fade_page.on_transitioned_in = () => Application.Quit();
        fade_page.TransitionIn(1, 0, Color.black);

        TransitionOut();
    }
	
}
