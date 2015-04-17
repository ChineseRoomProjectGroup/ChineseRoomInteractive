using UnityEngine;
using System.Collections;

public class HandController : MonoBehaviour 
{

    public Transform tip_position;

	public void Update()
    {
        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mouse_pos - (Vector2)tip_position.localPosition;

    }
}
