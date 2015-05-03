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
    
    // visual
    private float fade_time = 0.5f;
    private float seconds_per_char = 0.001f; // for typing transition


    public void Start()
    {
        text.text = "";
    }
    public void Message(string text, float seconds)
    {
        message_queue.Enqueue(new Message(text, seconds));
        DispNextMessageFromQueue();
    }

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
