using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Message
{
    public string text;
    public float seconds;
    public bool until_next;

    public Message(string text, float seconds)
    {
        this.text = text;
        this.seconds = seconds;
        this.until_next = false;
    }
    public Message(string text)
    {
        this.text = text;
        this.seconds = 0;
        this.until_next = true;
    }
}

