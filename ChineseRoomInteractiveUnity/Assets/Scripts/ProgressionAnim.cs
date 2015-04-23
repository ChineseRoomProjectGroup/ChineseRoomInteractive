using UnityEngine;
using System.Collections;

public class ProgressionAnim : MonoBehaviour 
{
    public Animator animator;

    public void Next()
    {
        animator.SetTrigger("Next");
    }
	
}
