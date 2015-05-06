using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class Messenger : MonoBehaviour
{
    public Text text;

    private Queue<Message> message_queue = new Queue<Message>();
    private Message current_message = null;
    private bool clearing = false; // whether should fade out the current message
    
    // visual
    private float fade_time = 0.5f;
    private float seconds_per_char = 0.001f; // for typing transition


    public void Start()
    {
        text.text = "";
    }
    public void Message(string text, float seconds)
    {
        Message(text, seconds, true);
    }
    public void Message(string text, float seconds, bool clobber)
    {
        if (text == "")
        {
            Clear();
            return;
        }

        message_queue.Enqueue(new Message(text, seconds));
        if (current_message == null || clobber) DispNextMessageFromQueue();
    }
    /// <summary>
    /// The message will remain until another message is requested.
    /// </summary>
    /// <param name="text"></param>
    public void Message(string text)
    {
        Message(text, true);
    }
    /// <summary>
    /// The message will remain until another message is requested.
    /// </summary>
    /// <param name="text"></param>
    public void Message(string text, bool clobber)
    {
        if (text == "")
        {
            Clear();
            return;
        }

        message_queue.Enqueue(new Message(text));
        if (current_message == null || clobber) DispNextMessageFromQueue();
    }
    public void Clear()
    {
        if (current_message != null) clearing = true;
    }

    /// <summary>
    /// Will immediately stop any currently displaying message if a new message is
    /// waiting in the queue.
    /// </summary>
    private void DispNextMessageFromQueue()
    {
        if (message_queue.Count == 0) return;
        current_message = message_queue.Dequeue();

        StopCoroutine("DisplayMessage");
        StartCoroutine("DisplayMessage");
    }
    private IEnumerator DisplayMessage()
    {
        // transition in
        SetTextAlpha(1);
        text.text = "";

        for (int i = 0; i < current_message.text.Length; ++i)
        {
            text.text = current_message.text.Substring(0, i+1);
            yield return new WaitForSeconds(seconds_per_char);
        }

        // if this message should display until there is another message to show, just keep trying
        // to show the next message
        if (current_message.until_next)
        {
            while (true)
            {
                // fade out this message
                if (clearing)
                {
                    clearing = false;
                    break;
                }
                DispNextMessageFromQueue();
                yield return null;
            }
        }

        // wait for life time (minus fade time and in transition time)
        yield return new WaitForSeconds(current_message.seconds);

        // fade out
        float fade_timer = fade_time;
        while (fade_timer > 0)
        {
            fade_timer -= Time.deltaTime;
            SetTextAlpha(fade_timer / fade_time);
            yield return null;
        }
        SetTextAlpha(0);


        // new messages now allowed
        current_message = null;
        DispNextMessageFromQueue();
    }
    private void SetTextAlpha(float a)
    {
        Color c = text.color;
        c.a = a;
        text.color = c;
    }
	
}
