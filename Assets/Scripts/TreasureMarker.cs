using UnityEngine;

public class TreasureMarker : MonoBehaviour
{
    public enum Owner { None, Blue, Red }
    public Owner ownedBy = Owner.None;

    public void SetVisible(bool isVisible)
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = isVisible;
        }
    }
}
