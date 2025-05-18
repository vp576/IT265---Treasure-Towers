using UnityEngine;

public class GoldRevealManager : MonoBehaviour
{
    public KeyCode revealKey = KeyCode.G;
    public PreGameManager preGameManager;

    void Update()
    {
        TreasureMarker.Owner current = (preGameManager.currentTurn == PreGameManager.PlayerTurn.Blue)
            ? TreasureMarker.Owner.Blue
            : TreasureMarker.Owner.Red;

        bool isHoldingRevealKey = Input.GetKey(revealKey);
        ShowOwnedNuggets(current, isHoldingRevealKey);
    }

    void ShowOwnedNuggets(TreasureMarker.Owner player, bool visible)
    {
        TreasureMarker[] allMarkers = FindObjectsOfType<TreasureMarker>();
        foreach (var marker in allMarkers)
        {
            marker.SetVisible(marker.ownedBy == player && visible);
        }
    }
}
