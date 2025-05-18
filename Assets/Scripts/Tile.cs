using UnityEngine;

public class Tile : MonoBehaviour
{
    public float towerHeight = 1.4f;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnMouseDown()
    {
        if (gameManager.currentPhase != GameManager.GamePhase.Playing) return;

        GameObject currentPlayer = gameManager.GetCurrentPlayerObject();
        if (currentPlayer == null || gameManager.RemainingMoves <= 0) return;

        Vector3 tilePos = transform.position;
        Vector3 playerPos = currentPlayer.transform.position;
        playerPos.y = tilePos.y;

        float dist = Vector3.Distance(tilePos, playerPos);
        if (dist < 0.1f) {return;}

        if (dist <= 1.5f)
        { currentPlayer.GetComponent<PlayerController>().MoveTo(tilePos); }
    }

    public Vector3 GetNextStackPosition()
    {
        int count = GetStackCount();
        return transform.position + Vector3.up * (towerHeight * count + 0.5f);
    }

    public int GetStackCount()
    {
        int count = 0;
        float scanHeight = 10f;

        Collider[] hits = Physics.OverlapBox(
            transform.position + Vector3.up * scanHeight * 0.5f,
            new Vector3(0.45f, scanHeight * 0.5f, 0.45f)
        );

        foreach (Collider c in hits)
        {
            if (c.GetComponent<TowerFloor>())
                count++;
        }

        return count;
    }
}
