using UnityEngine;
using System.Collections;

public class HandTooltip : MonoBehaviour
{
    private HandController hand;
    private Vector2 offset;


    public void Awake()
    {
        // get HandController reference
        hand = FindObjectOfType<HandController>();
        if (hand == null) Debug.LogError("HandController object not found");

        // get offset from hand position
        offset = transform.position;
    }
    public void Update()
    {
        transform.position = (Vector2)hand.transform.position + offset;
    }
}
