using UnityEngine;

public class TowerFloor : MonoBehaviour
{
    public enum Owner { None, Blue, Red }

    public Owner placedBy = Owner.None; // Who owns this tower floor
    public bool hasTreasure = false;    // Is this the treasure floor?

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            // Color-code the floor based on owner
            if (placedBy == Owner.Blue)
                rend.material.color = Color.blue;
            else if (placedBy == Owner.Red)
                rend.material.color = Color.red;
        }
    }
} 
