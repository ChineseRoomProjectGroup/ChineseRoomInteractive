using UnityEngine;
using System.Collections;

public class Pencil : GrabItem {

    bool input_action = Input.GetMouseButtonDown(0);

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if (input_action) // pencil drops
        {
       
            if(true) // pencil drops on an answer slot on paper
            {
                // the symbol list pops up
            }
        }
	}
}
