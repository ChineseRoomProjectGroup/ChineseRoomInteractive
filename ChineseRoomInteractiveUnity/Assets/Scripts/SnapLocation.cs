using UnityEngine;
using System.Collections;

public class SnapLocation : MonoBehaviour
{
    private bool occupied;
    

    /// <summary>
    /// Set the location to be active - indicator on this object will be visible when active.
    /// </summary>
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
    public void SetOccupied(bool occupied)
    {
        this.occupied = occupied;
    }


    public bool IsOccupied()
    {
        return occupied;
    }
}
