using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public enum PlaceableType { None, TowerFloor, Treasure }
    public PlaceableType selectedType = PlaceableType.None;

    public GameObject towerFloorPrefab;
    public GameObject treasurePrefab;

    public Material redMat;
    public Material blueMat;

    public Camera redCamera;
    public Camera blueCamera;

    public int towerFloorCount = 10;
    public int treasureCount = 1;

    public enum Player { Blue, Red }
    public Player currentPlayer = Player.Blue;

    private bool bluePlacedTreasure = false;
    private bool redPlacedTreasure = false;

    public void SelectType(string type)
    {
        selectedType = (PlaceableType)System.Enum.Parse(typeof(PlaceableType), type);
    }

    public GameObject GetSelectedPrefab()
    {
        switch (selectedType)
        {
            case PlaceableType.TowerFloor: return towerFloorPrefab;
            case PlaceableType.Treasure:
                if (currentPlayer == Player.Blue && !bluePlacedTreasure)
                    return treasurePrefab;
                if (currentPlayer == Player.Red && !redPlacedTreasure)
                    return treasurePrefab;
                return null;
            default: return null;
        }
    }

    public void DecrementCount()
    {
        switch (selectedType)
        {
            case PlaceableType.TowerFloor: towerFloorCount--; break;
            case PlaceableType.Treasure:
                if (currentPlayer == Player.Blue) bluePlacedTreasure = true;
                else if (currentPlayer == Player.Red) redPlacedTreasure = true;
                treasureCount--; break;
        }
    }

    public bool CanPlace()
    {
        switch (selectedType)
        {
            case PlaceableType.TowerFloor: return towerFloorCount > 0;
            case PlaceableType.Treasure:
                if (currentPlayer == Player.Blue && !bluePlacedTreasure) return true;
                if (currentPlayer == Player.Red && !redPlacedTreasure) return true;
                return false;
        }
        return false;
    }

    public void EndTurn()
    {
        selectedType = PlaceableType.None;
        currentPlayer = (currentPlayer == Player.Blue) ? Player.Red : Player.Blue;
    }
}
